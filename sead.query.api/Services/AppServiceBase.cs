using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using SeadQueryCore.Model;

namespace SeadQueryAPI.Services
{
    public class AppServiceBase {

        public IFacetSetting Config { get; set; }
        public IRepositoryRegistry Context { get; set; }

        public AppServiceBase(ISetting config, IRepositoryRegistry context)
        {
            Config = config.Facet;
            Context = context;
        }

    }
}
