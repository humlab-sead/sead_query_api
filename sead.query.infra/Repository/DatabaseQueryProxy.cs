using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class DatabaseQueryProxy(DbContext context) : ITypedQueryProxy, IDynamicQueryProxy
    {
        public DbContext Context { get; } = context;

        public T QueryRow<T>(string sql, Func<IDataReader, T> selector = null)
        {
            using (var reader = Context.Database.ExecuteSqlQuery(sql).DbDataReader)
            {
                return reader.Select(selector).Take(1).FirstOrDefault();
            }
        }

        /// <summary>
        /// Exceutes a generic query. built from
        /// Only used in ResultContentService.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>IDataReader</returns>
        public IDataReader Query(string sql)
        {
            return Context.Database.ExecuteSqlQuery(sql).DbDataReader;
        }

        public List<T> QueryRows<T>(string sql, Func<IDataReader, T> selector)
        {
            using (var reader = Context.Database.ExecuteSqlQuery(sql).DbDataReader)
            {
                return reader.Select(selector).ToList();
            }
        }

        public List<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2)
        {
            using (var reader = Context.Database.ExecuteSqlQuery(sql).DbDataReader)
            {
                try
                {
                    return reader.Select(x => new Key2Value<K, V>(
                        x.GetFieldValue<K>(keyIndex),
                        x.GetFieldValue<V>(valueIndex1),
                        x.GetFieldValue<V>(valueIndex2))
                    ).ToList();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
