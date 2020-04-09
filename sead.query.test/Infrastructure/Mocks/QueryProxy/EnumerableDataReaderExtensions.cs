using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryTest.Infrastructure
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

            foreach (T item in items) {
                DataRow row = table.NewRow();
                foreach (var prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            return table.CreateDataReader();
        }
    }
}