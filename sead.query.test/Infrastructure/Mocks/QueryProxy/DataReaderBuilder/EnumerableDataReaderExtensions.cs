using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace SQT.Infrastructure
{

    public static class EnumerableDataReaderExtensions
    {
        /// <summary>
        /// Converts a list of entities to a IDataReader using reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IDataReader ToDataReader<T>(this IEnumerable<T> items) where T : class, new()
        {
            var properties = typeof(T).GetProperties();
            var lookup = properties.ToDictionary(p => p.Name);
            DataTable table = new DataTable();

            foreach (var p in properties)
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);

            foreach (T item in items)
            {
                DataRow row = table.NewRow();
                foreach (var prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table.CreateDataReader();
        }

        public static IDataReader ToDataReader(this IEnumerable<ExpandoObject> items)
        {
            DataTable table = new DataTable();

            foreach (IDictionary<string, object> item in items)
            {

                if (table.Columns.Count == 0)
                {
                    foreach (var key in item.Keys)
                        table.Columns.Add(key, item[key].GetType());
                }

                var row = table.NewRow();

                foreach (var key in item.Keys)
                    row[key] = item?[key] ?? DBNull.Value;

                table.Rows.Add(row);
            }
            return table.CreateDataReader();
        }
    }
}