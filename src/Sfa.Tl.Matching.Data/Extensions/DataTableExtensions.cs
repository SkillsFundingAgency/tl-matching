using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sfa.Tl.Matching.Data.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> entities)
        {
            var properties = typeof(T).GetProperties();
            var dataTable = new DataTable();

            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);

            foreach (var entity in entities)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
        }
    }
}