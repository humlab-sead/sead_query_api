﻿using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace QuerySeadDomain {

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
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

        public TEntity Get(int id)
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

        //QueryRow("", r => new { CatName = r.GetString(0), CarDOB = r.GetDateTime(1), CatStatus = r.GetInt32(2) });
        public T QueryRow<T>(string sql, Func<DbDataReader, T> selector = null)
        {
            return Context.Database.ExecuteSqlQuery(sql).DbDataReader.Select(selector).Take(1).FirstOrDefault();
        }

        public IEnumerable<T> QueryRows<T>(string sql, Func<DbDataReader, T> selector)
        {
            return Context.Database.ExecuteSqlQuery(sql).DbDataReader.Select(selector).ToList();
        }

    }
}
