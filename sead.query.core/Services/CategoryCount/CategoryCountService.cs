using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{

    public class CategoryCountService : QueryServiceBase, ICategoryCountService {

        protected IFacetRepository Repository { get; set; }
        public ITypedQueryProxy QueryProxy { get; }

        public class CategoryCountResult {
            public string SqlQuery { get; set; }
            public Dictionary<string, CategoryCountItem> Data { get; set; }
        }
        public CategoryCountService(IFacetSetting config, IRepositoryRegistry registry, IQuerySetupCompiler builder, ITypedQueryProxy queryProxy) : base(registry, builder)
        {
            Config = config;
            Repository = Registry.Facets;
            QueryProxy = queryProxy;
        }

        public IFacetSetting Config { get; }

        public virtual CategoryCountResult Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery=null)
        {
            Facet facet = Repository.GetByCode(facetCode);
            string sql = Compile(facet, facetsConfig, intervalQuery);
            var values =  Query(sql).ToList();
            var data = values.ToDictionary(z => Coalesce(z.Category, "(null)"));

            return new CategoryCountResult {
                SqlQuery = sql,
                Data = data
            };
        }

        protected virtual List<CategoryCountItem> Query(string sql)
        {
            return QueryProxy.QueryRows<CategoryCountItem>(sql,
                x => new CategoryCountItem()
                {
                    Category = GetCategory(x),
                    Count = GetCount(x),
                    Extent = GetExtent(x)
                }).ToList();
        }

        protected virtual string GetCategory(IDataReader x)         => throw new NotSupportedException();
        protected virtual int GetCount(IDataReader x)               => throw new NotSupportedException();
        protected virtual List<decimal> GetExtent(IDataReader x)    => throw new NotSupportedException();

        protected virtual string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery) => throw new NotSupportedException();
    }

}
