using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class RepositoryRegistry : IRepositoryRegistry
    {
        public FacetContext Context { get; private set; }
        protected Dictionary<Type, IRepository2> Repositories { get; private set; }

        public RepositoryRegistry(IFacetContext _context)
        {
            Context = (FacetContext)_context;
            Repositories = CreateRepositories();
        }

        protected Dictionary<Type, IRepository2> CreateRepositories()
        {
            var repositories = new Dictionary<Type, IRepository2>()
            {
                { typeof(IFacetRepository),         new FacetRepository(Context) },
                { typeof(ITableRelationRepository), new TableRelationRepository(Context) },
                { typeof(ITableRepository),         new TableRepository(Context) },
                { typeof(IResultSpecificationRepository),        new ResultSpecificationRepository(Context) },
                { typeof(IFacetGroupRepository),    new FacetGroupRepository(Context) },
                { typeof(IFacetTypeRepository),     new FacetTypeRepository(Context) },
                { typeof(IFacetTableRepository),    new FacetTableRepository(Context) },
                { typeof(IViewStateRepository),     new ViewStateRepository(Context) }
            };
            return repositories;
        }

        //public IEnumerable<Type> GetRepositoryTypes()
        //{
        //    return InfraUtility.GetTypesThatIsAssignableFrom<IRepository2>("sead.query.infra");
        //}

        //protected Dictionary<Type, IRepository2> CreateRepositories2()
        //{
        //    return GetRepositoryTypes()
        //        .Select(type => (IRepository2)Activator.CreateInstance(type, Context))
        //        .ToDictionary(z => GetInterface(z));
        //}

        //private static Type GetInterface(IRepository2 instance)
        //{
        //    return instance.GetType()
        //        .GetInterfaces()
        //        .Where(t => t.IsInterface && t != typeof(IRepository2) && typeof(IRepository2).IsAssignableFrom(t))
        //        .FirstOrDefault();
        //}

        public T GetRepository<T>() where T : IRepository2
        {
            return (T)GetRepository(typeof(T));
        }

        public IRepository2 GetRepository(Type type)
        {
            if (!Repositories.TryGetValue(type, out var value))
                throw new KeyNotFoundException(type.Name);
            return (IRepository2)value;
        }

        public virtual IFacetRepository Facets => GetRepository<IFacetRepository>();
        public virtual ITableRelationRepository TableRelations => GetRepository<ITableRelationRepository>();
        public virtual ITableRepository Tables => GetRepository<ITableRepository>();
        public virtual IResultSpecificationRepository Results => GetRepository<IResultSpecificationRepository>();
        public virtual IFacetGroupRepository FacetGroups => GetRepository<IFacetGroupRepository>();
        public virtual IFacetTypeRepository FacetTypes => GetRepository<IFacetTypeRepository>();
        public virtual IViewStateRepository ViewStates => GetRepository<IViewStateRepository>();
        public virtual IFacetTableRepository FacetTables => GetRepository<IFacetTableRepository>();

        public int Commit() => Context.SaveChanges();
        public void Dispose() => Context.Dispose();
    }

    public static class QueryDynamicExt
    {
        public static IEnumerable<T> Populate<T>(this DbDataReader dr) where T : class
        {
            while (dr.Read())
            {
                var item = Activator.CreateInstance<T>();
                var index = 0;
                foreach (var property in typeof(T).GetProperties())
                {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(dr.GetValue(index), convertTo), null);
                    index++;
                }
                yield return item;
            }
        }

        public static T Populate2<T>(this DbDataReader dr, T instance) where T : class
        {
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (!dr.IsDBNull(dr.GetOrdinal(property.Name)))
                {
                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(item, Convert.ChangeType(dr[property.Name], convertTo), null);
                }
            }
            return item;
        }

        public static IEnumerable<dynamic> QueryDynamic2(this DbContext dbContext, string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                AssignParams(Parameters, cmd);
                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;
                    }
                }
            }
        }

        private static void AssignParams(Dictionary<string, object> Parameters, DbCommand cmd)
        {
            foreach (KeyValuePair<string, object> param in Parameters)
            {
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
