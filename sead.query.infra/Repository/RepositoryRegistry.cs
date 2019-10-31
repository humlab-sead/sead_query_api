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
using SeadQueryCore;

namespace SeadQueryInfra {


    public class RepositoryRegistry : IRepositoryRegistry {

        private readonly FacetContext context;

        public RepositoryRegistry(IFacetContext _context)
        {
            context = (FacetContext)_context;
            Facets = new FacetRepository(context);
            TableRelations = new TableRelationRepository(context);
            Tables = new TableRepository(context);
            Results = new ResultRepository(context);
            FacetGroups = new FacetGroupRepository(context);
            FacetTypes = new FacetTypeRepository(context);
            FacetTables = new FacetTableRepository(context);
            ViewStates = new ViewStateRepository(context);
        }

        public IFacetRepository Facets { get; private set; }
        public ITableRelationRepository TableRelations { get; private set; }
        public ITableRepository Tables { get; private set; }
        public IResultRepository Results { get; private set; }
        public IFacetGroupRepository FacetGroups { get; private set; }
        public IFacetTypeRepository FacetTypes { get; private set; }
        public IViewStateRepository ViewStates { get; private set; }

        public IFacetTableRepository FacetTables { get; private set; }

        public int Commit() => context.SaveChanges();
        public void Dispose() => context.Dispose();

        public T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null)
        {
            using (var reader = context.Database.ExecuteSqlQuery(sql).DbDataReader) {
                return reader.Select(selector).Take(1).FirstOrDefault();
            }
        }

        public DbDataReader Query(string sql)
        {
            // FIXME: call dispose?
            return context.Database.ExecuteSqlQuery(sql).DbDataReader;
        }

        public List<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector)
        {
            using (var reader = context.Database.ExecuteSqlQuery(sql).DbDataReader) {
                return reader.Select(selector).ToList();
            }
        }

        public List<Key2Value<K, V>> QueryKeyValues2<K, V>(string sql, int keyIndex = 0, int valueIndex1 = 1, int valueIndex2 = 2)
        {
            using (var reader = context.Database.ExecuteSqlQuery(sql).DbDataReader)
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
