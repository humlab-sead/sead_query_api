using QuerySeadDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySeadDomain
{

    public interface IDeleteBogusPickService {
        FacetsConfig2 Delete(FacetsConfig2 facetsConfig);
    }

    public class DeleteBogusPickService : QueryServiceBase, IDeleteBogusPickService {

        public DeleteBogusPickService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        //***************************************************************************************************************************************************
        /*
        public:  Delete
        Removes invalid selections e.g. hidden selections still being sent from the client.
        The client keep them since they can be visible when the filters changes
        This is only applicable for discrete facets (range facet selection are always visible)
        */
        public FacetsConfig2 Delete(FacetsConfig2 facetsConfig)
        {
            foreach (string facetCode in facetsConfig.GetFacetCodes()) {

                var config = facetsConfig.GetConfig(facetCode);

                if (config.Facet.FacetTypeId != EFacetType.Discrete || config.Picks.Count == 0) {
                    continue;
                }

                config.Picks = GetValidPicks(facetsConfig, facetCode, config);
            }
            return facetsConfig;
        }

        private List<FacetConfigPick> GetValidPicks(FacetsConfig2 facetsConfig, string facetCode, FacetConfig2 config)
        {
            QuerySetup query = QueryBuilder.Build(facetsConfig, facetCode);

            string sql = ValidPicksSqlQueryBuilder.Compile(query, config.Facet, config.GetPickValues());

            List<FacetConfigPick> rows = Context.QueryRows(sql, x => new FacetConfigPick(EFacetPickType.discrete, x.GetString(0), x.GetString(1))).ToList();
            return rows;
        }
    }
}
