using System;

namespace SeadQueryCore
{

    public abstract class IsValidDomainEntitySpecification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);
    }
}
