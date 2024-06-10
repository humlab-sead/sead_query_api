using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using SeadQueryCore;
using System.Threading.Tasks;

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

        public List<T> QueryScalars<T>(string scalarSql)
        {
            using (var dr = Context.Database.ExecuteSqlQuery(scalarSql).DbDataReader)
            {
                if (dr.Read())
                {
                    return Enumerable.Range(0, dr.FieldCount)
                        .Select(i => dr.IsDBNull(i) ? default : dr.GetFieldValue<T>(i)).ToList();
                }
            }
            return null;
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

        public static async Task<(T, T)> GetRangeAsync<T>(IDataReader dr, int index)
        {
            var datareader = (DbDataReader)dr;
            NpgsqlRange<T> range = await datareader.GetFieldValueAsync<NpgsqlRange<T>>(index);
            if (range.IsEmpty)
                return (default(T), default(T));
            return (range.LowerBound, range.UpperBound);
        }


        public (T, T) GetRange<T>(IDataReader dr, int index)
        {
            var datareader = (DbDataReader)dr;
            NpgsqlRange<T> range = datareader.GetFieldValue<NpgsqlRange<T>>(index);
            if (range.IsEmpty)
                return (default, default);
            return (range.LowerBound, range.UpperBound);
        }


    }
}
