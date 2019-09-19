using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{

    public class CategoryCountService : QueryServiceBase, ICategoryCountService {


        public CategoryCountService(IFacetSetting config, IRepositoryRegistry context, IQuerySetupCompiler builder) : base(context, builder)
        {
            Config = config;
        }

        public IFacetSetting Config { get; }

        public Dictionary<string, CategoryCountItem> Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery=null)
        {
            Facet facet = Context.Facets.GetByCode(facetCode);
            string sql = Compile(facet, facetsConfig, intervalQuery);
            var values =  Query(sql).ToList();
            Dictionary<string, CategoryCountItem> data = values.ToDictionary(z => Coalesce(z.Category, "(null)"));
            return data;
        }

        protected virtual List<CategoryCountItem> Query(string sql) => throw new NotSupportedException();

        protected virtual string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery) => throw new NotSupportedException();
    }

}
