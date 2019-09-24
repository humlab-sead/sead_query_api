using System;

namespace SeadQueryCore
{

    public abstract class QuerySeadSpecification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);
    }
}
