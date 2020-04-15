using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public class ServiceBase
    {
        public IRepositoryRegistry Registry { get; set; }

        public ServiceBase(IRepositoryRegistry context)
        {
            Registry = context;
        }
    }

    public class QueryServiceBase : ServiceBase {
        public IQuerySetupBuilder QuerySetupBuilder { get; set; }

        public QueryServiceBase(IRepositoryRegistry context, IQuerySetupBuilder builder) : base(context)
        {
            QuerySetupBuilder = builder;
        }

    }
}
