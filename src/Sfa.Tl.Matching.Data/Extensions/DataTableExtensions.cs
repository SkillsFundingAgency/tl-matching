using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Sfa.Tl.Matching.Data.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {

            var properties = typeof(T).GetBulkInsertProperties();

            var dataTable = new DataTable();

            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);

            foreach (var entity in entities)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
        }

        public static IList<PropertyInfo> GetBulkInsertProperties(this Type entityType)
        {
            var ignoredColumn = new[] { "ModifiedOn", "ModifiedBy" };

            return entityType.GetProperties()
                .Where(prop => prop.GetCustomAttribute<KeyAttribute>(false) == null
                           && prop.GetCustomAttribute<DatabaseGeneratedAttribute>(false) == null
                           && !ignoredColumn.Contains(prop.Name)).ToList();
        }
    }
}