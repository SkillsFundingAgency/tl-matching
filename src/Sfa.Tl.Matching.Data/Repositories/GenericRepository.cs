using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ILogger _logger;
        private readonly MatchingDbContext _dbContext;

        public GenericRepository(ILogger<GenericRepository<T>> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> Count(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? await _dbContext.Set<T>().CountAsync(predicate) : 
                await _dbContext.Set<T>().CountAsync();
        }

        public virtual async Task<int> CreateMany(IList<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);

            int createdRecordsCount;
            try
            {
                createdRecordsCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return createdRecordsCount;
        }

        public virtual async Task<int> Create(T entity)
        {
            await _dbContext.AddAsync(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return entity.Id;
        }

        public virtual async Task Update(T entity)
        {
            _dbContext.Update(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task UpdateMany(IList<T> entities)
        {
            _dbContext.UpdateRange(entities);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task UpdateWithSpecifedColumnsOnly(T entity, params Expression<Func<T, object>>[] properties)
        {
            foreach (var property in properties)
                _dbContext.Entry(entity).Property(property).IsModified = true;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task Delete(T entity)
        {
            _dbContext.Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> Delete(int id)
        {
            var entity = new T
            {
                Id = id
            };

            _dbContext.Attach(entity);
            _dbContext.Remove(entity);

            int deletedRecordCount;
            try
            {
                deletedRecordCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return deletedRecordCount;
        }

        public virtual async Task DeleteMany(IList<T> entities)
        {
            _dbContext.RemoveRange(entities);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(navigationPropertyPath);

            return predicate != null ? queryable.Where(predicate) : queryable;
        }

        public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(navigationPropertyPath);

            return await queryable.SingleOrDefaultAsync(predicate);
        }

        private IQueryable<T> GetQueryableWithIncludes(Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<T>().AsQueryable();

            if (navigationPropertyPath.Any())
            {
                queryable = navigationPropertyPath.Aggregate(queryable, (current, navProp) => current.Include(navProp));
            }

            return queryable;
        }

        public async Task BulkInsert(List<T> entities, string connectionString = "")
        {
            var dataTable = entities.ToDataTable();

            if (string.IsNullOrEmpty(connectionString))
                connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        //var deleteCommand = new SqlCommand($"DELETE FROM {typeof(T).Name}; DBCC CHECKIDENT ('dbo.{typeof(T).Name}',RESEED, 0);", connection, transaction);
                        var deleteCommand = new SqlCommand($"DELETE FROM {typeof(T).Name};", connection, transaction);
                        deleteCommand.ExecuteNonQuery();

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
                                //string pattern = @"\d+";
                                //Match match = Regex.Match(ex.Message.ToString(), pattern);
                                //var index = Convert.ToInt32(match.Value) -1;

                                //FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                //var sortedColumns = fi.GetValue(bulkCopy);
                                //var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                //FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                //var metadata = itemdata.GetValue(items[index]);

                                //var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                //var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                //throw new DataFormatException(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));

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

        public async Task<int> MergeFromStaging(string connectionString = "")
        {
            int numberOfRecordsAffected;
            if (string.IsNullOrEmpty(connectionString))
                connectionString = _dbContext.Database.GetDbConnection().ConnectionString;

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        var isSuccessful = false;
                        try
                        {
                            var mergeSql = GetMergeSql();

                            var mergeCommand = new SqlCommand(mergeSql, connection, transaction) { CommandTimeout = 120 };

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

        public virtual async Task UpdateManyWithSpecifedColumnsOnly(IList<T> entities, params Expression<Func<T, object>>[] properties)
        {
            foreach (var entity in entities)
            foreach (var property in properties)
                _dbContext.Entry(entity).Property(property).IsModified = true;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }
    }
}