using System;
using System.Collections.Generic;
using static SeadQueryCore.Utility;
using System.Linq;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetup {
        public FacetConfig2 TargetConfig;
        public FacetDefinition Facet;
        public List<GraphRoute> Routes;
        public List<GraphRoute> ReducedRoutes;

        public List<string> Joins;
        public List<string> Criterias;

        public string CategoryTextFilter { get { return TargetConfig?.TextFilter ?? "";  } }

        public QuerySetup(FacetConfig2 targetConfig, FacetDefinition facet, List<string> sqlJoins, Dictionary<string, string> criterias, List<GraphRoute> routes, List<GraphRoute> reducedRoutes)
        {
            TargetConfig = targetConfig;
            Facet = facet;
            Routes = routes;
            ReducedRoutes = reducedRoutes;
            Joins = sqlJoins;
            Criterias = criterias.Select(x => "(" + x.Value + ")").AppendIf(Facet.QueryCriteria).ToList();
        }
    }

    public class ResultQuerySetup
    {
        public List<ResultAggregateField> Fields { get; set; }
        public List<ResultField> ResultFields => Fields.Select(z => z.ResultField).ToList();
        public List<string> DataTables => Fields.Select(z => z.ResultField.TableName).Where(t => t != null).ToList();

        public List<(string, string)> AliasPairs { get; set; }
        public List<string> DataFields { get; set; }
        public List<string> GroupByFields { get; set; }
        public List<string> InnerGroupByFields { get; set; }
        public List<string> SortFields { get; set; }

        public ResultQuerySetup(List<ResultAggregateField> fields)
        {
            var aliases = fields.Select((field, i) => new { Field = field, Alias = "alias_" + (i+1).ToString() });
            Fields = fields;
            InnerGroupByFields = aliases.Select(p => p.Alias).ToList();
            GroupByFields = aliases.Where(z => z.Field.FieldType.IsGroupByField).Select(z => z.Alias).ToList();
            AliasPairs = aliases.Select(z => ((z.Field.ResultField.ColumnName, z.Alias))).ToList();
            SortFields = aliases.Where(z => z.Field.FieldType.IsSortField).Select(z => z.Alias).ToList();
            DataFields = aliases.Where(z => z.Field.FieldType.IsResultValue).Select(z => z.Field.FieldType.Compiler.Compile(z.Alias)).ToList();
        }

        public bool IsEmpty => (Fields?.Count ?? 0) == 0;
    }
}
