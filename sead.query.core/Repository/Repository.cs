using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace SeadQueryCore {
    public class Repository<TEntity, K> : IRepository<TEntity, K> where TEntity : class
    {
        protected readonly DomainModelDbContext Context;

        public Repository(DomainModelDbContext context)
        {
            this.Context = context;
        }

        protected IQueryable<TEntity> GetSet()
        {
            return GetInclude(Context.Set<TEntity>());
        }

        protected virtual IQueryable<TEntity> GetInclude(IQueryable<TEntity> set)
        {
            return set;
        }

        public TEntity Get(K id)
        {
            return Context.Set<TEntity>().Find(id);
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
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null)
        {
            using (var reader = Context.Database.ExecuteSqlQuery(sql).DbDataReader)
            {
                return reader.Select(selector).Take(1).FirstOrDefault();
            }
        }

        public List<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector)
        {
            using (var reader = Context.Database.ExecuteSqlQuery(sql).DbDataReader)
            {
                return reader.Select(selector).ToList();
            }
        }
    }
}
