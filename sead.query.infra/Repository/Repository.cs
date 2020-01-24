using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace SeadQueryInfra
{

    public class Repository<TEntity, K> : IRepository<TEntity, K> where TEntity : class
    {
        public readonly IFacetContext Context;

        public Repository(IFacetContext context)
        {
            this.Context = context;
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
