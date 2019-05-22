using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class SqlBulkRepository : ISqlBulkRepository
    {
        private readonly ILogger<SqlBulkRepository> _logger;
        private readonly MatchingDbContext _matchingDbContext;

        public SqlBulkRepository(ILogger<SqlBulkRepository> logger, MatchingDbContext matchingDbContext)
        {
            _logger = logger;
            _matchingDbContext = matchingDbContext;
        }

        public async Task BulkInsertIntoToStaging<T>(List<T> entities)
        {
            var dataTable = entities.ToDataTable();

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(_matchingDbContext.Database.GetDbConnection().ConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var deleteCommand = new SqlCommand($"DELETE FROM {typeof(T).Name}; DBCC CHECKIDENT ('dbo.{typeof(T).Name}',RESEED, 0);", connection, transaction);
                        deleteCommand.ExecuteNonQuery();

                        using (var bulkCopy = CreateSqlBulkCopy<T>(connection, transaction, dataTable))
                        {
                            var isSuccessful = false;
                            try
                            {
                                await bulkCopy.WriteToServerAsync(dataTable);

                                isSuccessful = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{nameof(SqlBulkRepository)} - Error inserting {typeof(T).Name} data into the database. Internal Exception : {ex} ");
                                throw;
                            }
                            finally
                            {
                                if (isSuccessful)
                                    transaction.Commit();
                                else
                                    transaction.Rollback();

                                connection.Close();
                            }
                        }
                    }
                }
                transactionScope.Complete();
            }
        }

        private SqlBulkCopy CreateSqlBulkCopy<T>(SqlConnection connection, SqlTransaction transaction, DataTable dataTable)
        {
            var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)
            {
                BatchSize = 100000,
                DestinationTableName = $"dbo.{typeof(T).Name}"
            };

            var properties = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in properties)
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