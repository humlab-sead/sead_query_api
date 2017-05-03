using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadDomain.QueryBuilder
{

    // TODO Convert to non-static functions
    public class QuerySetupService {

        //***************************************************************************************************************************************************
        /*
        Function: compileQuery - the core of the dynamic query builder.

        It's input are:
          1) the facet (code) that triggered the action
          2) the user selected filters preceeding the triggering facet
      
        It also uses an array of filters that are used to filter each facet result by text

        Parameters:
        * facetsConfig     The current client facet view state (all params, selections, text_filter, positions of facets etc)
        * facetCode        The target facet that the query populates/computes counts etc for.
        * extra_tables     Any extra tables that should be part of the query, the function uses the tables via get_joins to join the tables
        * facetCodes   The list of the facets (facet codes) currently active in the client view
        Logics:
        *  Get all selection preceding the target facet.
        *  Make query-where statements depending on which type of facets (range or discrete)
        Exceptions:
        * a - Discrete facets should be filtered by all selection from preceeding (but not including) the target facet.
        * b - Range facets should also be filtered by range-facets itself, although the bound should be expanded to show values outside the limiting range.
        Returns:
            Query object with SQL-parts
        */
        public static QuerySetup setup(IUnitOfWork context, FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables, List<string> facetCodes)
        {
            FacetsGraph graph = new FacetsGraph(context);

            QuerySetupBuilder query_builder = new QuerySetupBuilder(context, graph);
            QuerySetup querySetup = query_builder.Build(facetsConfig, facetCode, extraTables, facetCodes);
            return querySetup;
        }

        public static QuerySetup setup(IUnitOfWork context, FacetsConfig2 facetsConfig, string facetCode, List<string> extraTables = null)
        {
            List<string> facetCodes = facetsConfig.GetFacetCodes();
            if (facetCodes.Contains(facetCode)) {
                facetCodes.Add(facetCode);
            }
            if (extraTables == null) {
                extraTables = new List<string>();
            }
            QuerySetup querySetup = setup(context, facetsConfig, facetCode, extraTables, facetCodes);
            return querySetup;
        }
    }

}
