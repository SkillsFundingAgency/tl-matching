using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Logging;
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

        public SqlBulkInsertRepository(ILogger<SqlBulkInsertRepository<T>> logger, MatchingConfiguration matchingConfiguration)
        {
            _logger = logger;
            _matchingConfiguration = matchingConfiguration;
        }

        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            var dataTable = entities.ToDataTable();

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(_matchingConfiguration.SqlConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var truncateCommand = new SqlCommand($"TRUNCATE TABLE {typeof(T).Name};", connection, transaction)
                        {
                            CommandTimeout = DefaultCommandTimeout
                        };

                        truncateCommand.ExecuteNonQuery();

                        using (var bulkCopy = CreateSqlBulkCopy(connection, transaction, dataTable))
                        {
                            var isSuccessful = false;
                            try
                            {
                                await bulkCopy.WriteToServerAsync(dataTable);

                                isSuccessful = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{typeof(GenericRepository<T>).Name} - Error inserting {typeof(T).Name} data into the database. Internal Exception : {ex} ");
                                throw;
                            }
                            finally
                            {
                                if (isSuccessful)
                                {
                                    transaction.Commit();
                                }
                                // ReSharper disable once RedundantIfElseBlock
                                else
                                {
                                    transaction.Rollback();
                                }
                                connection.Close();
                            }
                        }
                    }
                }
                transactionScope.Complete();
            }
        }

        public async Task<int> MergeFromStagingAsync()
        {
            int numberOfRecordsAffected;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(_matchingConfiguration.SqlConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var isSuccessful = false;
                        try
                        {
                            var mergeSql = GetMergeSql();

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
                                transaction.Commit();
                            }
                            // ReSharper disable once RedundantIfElseBlock
                            else
                            {
                                transaction.Rollback();
                            }
                            connection.Close();
                        }
                    }
                }
                transactionScope.Complete();
            }

            return numberOfRecordsAffected;
        }

        private static string GetMergeSql()
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

            return $"MERGE INTO {target} AS TARGET " +
                   $"USING ( SELECT * FROM {source} ) AS SOURCE ON SOURCE.{sourceCompareColumn} = TARGET.{targetCompareColumn} " +
                   "WHEN MATCHED AND ( TARGET.ChecksumCol <> SOURCE.ChecksumCol ) THEN " +
                   $"UPDATE SET { fromSourceToTargetMappingForUpdate } " +
                   "WHEN NOT MATCHED BY TARGET THEN " +
                   $"INSERT ( {targetColumnList} ) VALUES ( {sourceColumnList} ) " +
                   "WHEN NOT MATCHED BY SOURCE THEN DELETE;";
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