using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly.Registry;
using Sfa.Tl.Matching.Data.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class SqlBulkInsertRepository<T> : IBulkInsertRepository<T> where T : BaseEntity, new()
    {
        private const int DefaultCommandTimeout = 1200;

        private readonly ILogger<SqlBulkInsertRepository<T>> _logger;
        private readonly MatchingConfiguration _matchingConfiguration;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public SqlBulkInsertRepository(
            ILogger<SqlBulkInsertRepository<T>> logger,
            MatchingConfiguration matchingConfiguration,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _logger = logger;
            _matchingConfiguration = matchingConfiguration;
            _policyRegistry = policyRegistry;
        }

        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            var dataTable = entities.ToDataTable();

            var (retryPolicy, retryPolicyContext) = _policyRegistry.GetRetryPolicy(_logger);

            await retryPolicy.ExecuteAsync(async _ =>
            {
                using var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TransactionScopeAsyncFlowOption.Enabled);
                await using (var connection = new SqlConnection(_matchingConfiguration.SqlConnectionString))
                {
                    connection.Open();

                    await using var transaction = connection.BeginTransaction();
                    var truncateCommand =
                        new SqlCommand($"TRUNCATE TABLE {typeof(T).Name};", connection, transaction);
                    truncateCommand.ExecuteNonQuery();

                    using var bulkCopy = CreateSqlBulkCopy(connection, transaction, dataTable);
                    var isSuccessful = false;
                    try
                    {
                        await bulkCopy.WriteToServerAsync(dataTable);

                        isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            $"{typeof(GenericRepository<T>).Name} - Error inserting {typeof(T).Name} data into the database. Internal Exception : {ex} ");
                        throw;
                    }
                    finally
                    {
                        if (isSuccessful)
                        {
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }

                        connection.Close();
                    }
                }
                transactionScope.Complete();
            }, retryPolicyContext);
        }

        public async Task<int> MergeFromStagingAsync(bool deleteMissingRows = true)
        {
            var numberOfRecordsAffected = 0;

            var (retryPolicy, retryPolicyContext) = _policyRegistry.GetRetryPolicy(_logger);

            await retryPolicy.ExecuteAsync(async _ =>
            {
                using var transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(DefaultCommandTimeout), TransactionScopeAsyncFlowOption.Enabled);
                await using (var connection = new SqlConnection(_matchingConfiguration.SqlConnectionString))
                {
                    connection.Open();

                    await using var transaction = connection.BeginTransaction();
                    var isSuccessful = false;
                    try
                    {
                        var mergeSql = GetMergeSql(deleteMissingRows);

                        var mergeCommand = new SqlCommand(mergeSql, connection, transaction)
                        {
                            CommandTimeout = DefaultCommandTimeout
                        };

                        numberOfRecordsAffected = await mergeCommand.ExecuteNonQueryAsync();

                        isSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccessful = false;
                        _logger.LogError($"{typeof(GenericRepository<T>).Name} - Error inserting {typeof(T).Name} data into the database. Internal Exception : {ex} ");
                        throw;
                    }
                    finally
                    {
                        if (isSuccessful)
                        {
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }
                        connection.Close();
                    }
                }
                transactionScope.Complete();
            }, retryPolicyContext);

            return numberOfRecordsAffected;
        }

        private static string GetMergeSql(bool deleteMissingRows)
        {
            var sourceType = typeof(T);
            var source = sourceType.Name;

            var target = source.Replace("Staging", string.Empty);
            var targetType = sourceType.Assembly.GetTypes().Single(t => t.Name == target);

            var columnNameList = sourceType.GetBulkInsertProperties().Select(prop => prop.Name).ToList();

            var targetColumnList = string.Join(", ", columnNameList);

            var sourceColumnList = string.Join(", ", columnNameList.Select(col => $"SOURCE.{col}"));

            var fromSourceToTargetMappingForUpdate = $"{string.Join(", ", columnNameList.Select(col => $"{col} = SOURCE.{col}"))}, ModifiedBy = SOURCE.CreatedBy, ModifiedOn = GETUTCDATE()";

            var sourceCompareColumn = sourceType.GetProperties().Where(prop => prop.GetCustomAttribute<MergeKeyAttribute>(false) != null).Select(prop => prop.Name).Single();
            var targetCompareColumn = targetType.GetProperties().Where(prop => prop.GetCustomAttribute<MergeKeyAttribute>(false) != null).Select(prop => prop.Name).Single();

            var sql = $"MERGE INTO {target} AS TARGET " +
                      $"USING ( SELECT * FROM {source} ) AS SOURCE ON SOURCE.{sourceCompareColumn} = TARGET.{targetCompareColumn} " +
                      "WHEN MATCHED AND ( TARGET.ChecksumCol <> SOURCE.ChecksumCol ) THEN " +
                      $"UPDATE SET {fromSourceToTargetMappingForUpdate} " +
                      "WHEN NOT MATCHED BY TARGET THEN " +
                      $"INSERT ( {targetColumnList} ) VALUES ( {sourceColumnList} ) " +
                      (deleteMissingRows ? "WHEN NOT MATCHED BY SOURCE THEN DELETE" : "") +
                      ";";

            return sql;
        }

        private SqlBulkCopy CreateSqlBulkCopy(SqlConnection connection, SqlTransaction transaction, DataTable dataTable)
        {
            var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
            {
                BatchSize = 100000,
                BulkCopyTimeout = 0,
                DestinationTableName = $"dbo.{typeof(T).Name}"
            };

            var properties = typeof(T).GetBulkInsertProperties();

            foreach (var prop in properties)
            {
                if (!dataTable.Columns.Contains(prop.Name)) continue; // ignore target column which is available source columns list
                bulkCopy.ColumnMappings.Add(prop.Name, prop.Name);
            }

            if (properties.Count != dataTable.Columns.Count)
                _logger.LogError("Source and Destination Columns do not Match");

            return bulkCopy;
        }
    }
}