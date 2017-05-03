using System;
using System.Collections.Generic;
using static QuerySeadDomain.Utility;
using System.Linq;

namespace QuerySeadDomain.QueryBuilder
{
    public class QuerySetup {

        public FacetDefinition facet;
        public List<string> fields;

        public string sql_fields;
        public string sql_table;
        public string sql_where;
        public string sql_where2;
        public string sql_joins;
        public string target_text_filter;

        public List<GraphRoute> none_reduced_routes;
        public List<GraphRoute> reduced_routes;

        public QuerySetup(FacetDefinition facet, string sql_joins, Dictionary<string, string> sql_filter_clauses, List<GraphRoute> none_reduced_routes, List<GraphRoute> reduced_routes, string target_text_filter)
        {

            this.facet = facet;

            this.fields = new List<string>() { facet.CategoryIdExpr, facet.CategoryNameExpr };
            this.sql_fields = String.Join(", ", this.fields);
            this.sql_table = facet.TargetTableName + " " + str_prefix("AS ", facet.AliasName, " ");
            this.sql_where = this.generateWhereClause(facet, sql_filter_clauses);
            this.sql_where2 = str_prefix("AND ", this.sql_where);

            this.sql_joins = sql_joins;
            this.target_text_filter = target_text_filter;
            this.none_reduced_routes = none_reduced_routes;
            this.reduced_routes = reduced_routes;

        }

        private string generateWhereClause(FacetDefinition facet, Dictionary<string, string> filter_clauses)
        {
            List<string> sql_where_clauses = filter_clauses.Select(x => "(" + x + ")\n").ToList();
            if (facet.Clauses.Count > 0) {
                sql_where_clauses.Add(facet.QueryCriteria);
            }
            sql_where = String.Join(" AND ", sql_where_clauses);
            return sql_where;
        }
    }

}
