using DataAccessPostgreSqlProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Data;
using System.Reflection;

namespace QueryFacetDomain {

    public interface IUnitOfWork {

        EdgeRepository Edges { get; }
        FacetRepository Facets { get; }
        NodeRepository Nodes { get; }
        ResultRepository Results { get; }

        int Commit();
        void Dispose();
        IEnumerable<KeyValuePair<K, V>> QueryKeyValuePairs<K, V>(string sql, Func<DbDataReader, K> keySelector, Func<DbDataReader, V> valueSelector);
        IEnumerable<Key1Value<K, V>> QueryKeyValues<K, V>(string sql, int keyIndex = 0, int valueIndex = 1);
        IEnumerable<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2);
        T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null);
        IEnumerable<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector);
        DbDataReader Query(string sql);

        IEnumerable<dynamic> QueryDynamic(string sql, Func<DbDataReader, dynamic> selector);
        IEnumerable<T> QueryPopulate<T>(string sql) where T : class;
    }

    [SerializableAttribute]
    public struct Key1Value<K, V> {
        public Key1Value(K k, V v)
        {
            Key = k; Value = v;
        }
        K Key { get; set; }
        V Value { get; set; }
    }

    [SerializableAttribute]
    public struct Key2Value<K, V> {
        public Key2Value(K k, V v1, V v2)
        {
            Key = k; Value1 = v1; Value2 = v2;
        }
        K Key { get; set; }
        V Value1 { get; set; }
        V Value2 { get; set; }
    }

    public class UnitOfWork : IUnitOfWork {


        private readonly DomainModelDbContext context;

        public UnitOfWork(DomainModelDbContext _context)
        {
            context = _context;
            Facets = new FacetRepository(context);
            Edges = new EdgeRepository(context);
            Nodes = new NodeRepository(context);
            Results = new ResultRepository(context);
        }

        public FacetRepository Facets { get; private set; }
        public EdgeRepository Edges { get; private set; }
        public NodeRepository Nodes { get; private set; }
        public ResultRepository Results { get; private set; }

        public int Commit() => context.SaveChanges();
        public void Dispose() => context.Dispose();

        public T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null)
        {
            return context.Database.ExecuteSqlQuery(sql).DbDataReader.Select(selector).Take(1).FirstOrDefault();
        }

        public DbDataReader Query(string sql)
        {
            return context.Database.ExecuteSqlQuery(sql).DbDataReader;
        }

        public IEnumerable<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector)
        {
            return context.Database.ExecuteSqlQuery(sql).DbDataReader.Select(selector);
        }

        public IEnumerable<KeyValuePair<K, V>> QueryKeyValuePairs<K, V>(string sql, Func<DbDataReader, K> keySelector, Func<DbDataReader, V> valueSelector)
        {
            return context.Database.ExecuteSqlQuery(sql)
                .DbDataReader.Select(x => new KeyValuePair<K, V>(keySelector(x), valueSelector(x)))
            .ToList();
        }

        public IEnumerable<Key1Value<K, V>> QueryKeyValues<K, V>(string sql, int keyIndex = 0, int valueIndex = 1) // Func<DbDataReader, KeyValuePair<K, V>> selector)
        {
            return context.Database.ExecuteSqlQuery(sql)
                .DbDataReader.Select(x => new Key1Value<K, V>(x.GetFieldValue<K>(keyIndex), x.GetFieldValue<V>(valueIndex)))
            .ToList();
        }

        public IEnumerable<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2)
        {
            return context.Database.ExecuteSqlQuery(sql)
                .DbDataReader.Select(x => new Key2Value<K, V>(x.GetFieldValue<K>(keyIndex), x.GetFieldValue<V>(valueIndex1), x.GetFieldValue<V>(valueIndex2))
            ).ToList();
        }

        public IEnumerable<dynamic> QueryDynamic(string sql, Func<DbDataReader, dynamic> selector)
        {
            return context.Database.ExecuteSqlQuery(sql).DbDataReader.Select(x => selector(x)); //.ToList();
        }

        public IEnumerable<T> QueryPopulate<T>(string sql) where T : class
        {
            return context.Database.ExecuteSqlQuery(sql).DbDataReader.Populate<T>();
        }

    }

    public static class QueryDynamicExt {

        public static IEnumerable<T> Populate<T>(this DbDataReader dr) where T : class
        {
            var results = new List<T>();
            var properties = typeof(T).GetProperties();
            while (dr.Read()) {
                var item = Activator.CreateInstance<T>();
                var index = 0;
                foreach (var property in typeof(T).GetProperties()) {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(dr.GetValue(index), convertTo), null);
                    index++;
                }
                yield return item;
            }
        }

        public static T Populate2<T>(this DbDataReader dr, T instance) where T : class
        {
            var results = new List<T>();
            var properties = typeof(T).GetProperties();
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties()) {
                if (!dr.IsDBNull(dr.GetOrdinal(property.Name))) {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(dr[property.Name], convertTo), null);
                }
            }
            return item;
        }

        public static IEnumerable<dynamic> QueryDynamic2(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                AssignParams(Parameters, cmd);
                using (var dataReader = cmd.ExecuteReader()) {
                    while (dataReader.Read()) {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }

        private static void AssignParams(Dictionary<string, object> Parameters, DbCommand cmd)
        {
            foreach (KeyValuePair<string, object> param in Parameters) {
                DbParameter dbParameter = cmd.CreateParameter();
                dbParameter.ParameterName = param.Key;
                dbParameter.Value = param.Value;
                cmd.Parameters.Add(dbParameter);
            }
        }

        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}
