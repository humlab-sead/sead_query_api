using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public class ServiceBase
    {
        public IFacetSetting Config { get; set; }
        public IRepositoryRegistry Context { get; set; }

        public ServiceBase(IQueryBuilderSetting config, IRepositoryRegistry context)
        {
            Config = config.Facet;
            Context = context;
        }

    }

    public class QueryServiceBase : ServiceBase {
        public IQuerySetupBuilder QuerySetupBuilder { get; set; }

        public QueryServiceBase(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder) : base(config, context)
        {
            QuerySetupBuilder = builder;
        }

    }
}
