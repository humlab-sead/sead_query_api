using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryFacetDomain {

    public interface IRepository2 {

    }

    public interface IRepository<TEntity> : IRepository2 where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
