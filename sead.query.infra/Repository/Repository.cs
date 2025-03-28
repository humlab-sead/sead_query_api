using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    // FIXME DbSet implements Repository pattern - so why invent it again...?
    public class Repository<TEntity, K>(IRepositoryRegistry registry) : IRepository<TEntity, K> where TEntity : class
    {
        public IFacetContext Context  { get {return Registry.Context;} }
        public readonly IRepositoryRegistry Registry = registry;

        public virtual FacetContext FacetContext
        {
            get { return (FacetContext)Context; }
        }

        protected virtual IFacetContext GetContext()
        {
            return Context;
        }

        protected virtual DbContext GetDbContext()
        {
            return (DbContext)Context;
        }

        protected virtual DbSet<TEntity> GetDbSet()
        {
            return GetContext().Set<TEntity>();
        }

        protected virtual DbDataReader ExecuteSqlQuery(string sql)
        {
            return GetDbContext().Database.ExecuteSqlQuery(sql).DbDataReader;
        }

        protected IQueryable<TEntity> GetSet()
        {
            return GetInclude(GetDbSet());
        }

        protected virtual IQueryable<TEntity> GetInclude(IQueryable<TEntity> set)
        {
            return set;
        }

        public TEntity Get(K id)
        {
            return GetDbSet().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSet().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            GetDbSet().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            GetDbSet().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            GetDbSet().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            GetDbSet().RemoveRange(entities);
        }

        public T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null)
        {
            using (var reader = ExecuteSqlQuery(sql))
            {
                return reader.Select(selector).Take(1).FirstOrDefault();
            }
        }

        public List<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector)
        {
            using (var reader = ExecuteSqlQuery(sql))
            {
                return reader.Select(selector).ToList();
            }
        }
    }
}
