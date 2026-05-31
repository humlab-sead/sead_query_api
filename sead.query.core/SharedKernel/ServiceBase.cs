using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    public class ServiceBase
    {
        public IRepositoryRegistry Registry { get; set; }

        public IFacetRepository Facets => Registry.Facets;
        public IResultSpecificationRepository Results => Registry.Results;

        public ServiceBase(IRepositoryRegistry context)
        {
            Registry = context;
        }
    }
}
