using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryTest.RepositoryMocks
{
    public class FakeFacetContext : FacetContext, IFacetContext
    {
        public FakeFacetContext(IQueryBuilderSetting config) : base(config)
        {
            throw new NotImplementedException("Has yet to find a reason for fake context");
        }
    }
}
