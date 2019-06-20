using QuerySeadDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using QuerySeadDomain.Model;

namespace QuerySeadAPI.Services
{
    public class AppServiceBase {

        public IFacetSetting Config { get; set; }
        public IUnitOfWork Context { get; set; }
        public ICache Cache { get; set; }

        public AppServiceBase(IQueryBuilderSetting config, IUnitOfWork context, ICache cache)
        {
            Config = config.Facet;
            Context = context;
            Cache = cache;
        }

    }
}
