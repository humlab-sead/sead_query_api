using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public class ServiceBase
    {
        public IFacetSetting Config { get; set; }
        public IUnitOfWork Context { get; set; }

        public ServiceBase(IQueryBuilderSetting config, IUnitOfWork context)
        {
            Config = config.Facet;
            Context = context;
        }

    }

    public class QueryServiceBase : ServiceBase {
        public IQuerySetupBuilder QueryBuilder { get; set; }

        public QueryServiceBase(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context)
        {
            QueryBuilder = builder;
        }

    }
}
