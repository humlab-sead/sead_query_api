using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore.QueryBuilder
{
    public class QuerySetupHelper
    {
        /// <summary>
        /// This function waas previously used to determmine type of join. If HasUserPick then INNER else LEFT.
        /// The logic behind this is not clear since putting a where-constraint on target enforces INNER join.
        /// This is also the reason why criterias were grouped by table and stored in a dictionary.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="tableCriterias"></param>
        /// <returns></returns>
        public static bool HasUserPicks(TableRelation edge, Dictionary<string, string> tableCriterias)
        {
            // FIXME: SHould SourceName really be considered here...?
            return /* tableCriterias.ContainsKey(edge.SourceName) || */ tableCriterias.ContainsKey(edge.TargetName);
        }

        //protected Dictionary<string, string> CompilePickCriterias(Facet targetFacet, List<FacetConfig2> involvedConfigs)
        //{
        //    // Compute criteria clauses for user picks for each affected facet
        //    var criterias = involvedConfigs
        //        .Select(config => (
        //            (
        //                Tablename: config.Facet.TargetTable.ResolvedAliasOrTableOrUdfName,
        //                Criteria: PickCompiler(config).Compile(targetFacet, config.Facet, config))
        //            )
        //         )
        //        .ToList();

        //    // Group and concatenate the criterias for each table
        //    Dictionary<string, string> pickCriterias = GroupCriteriaByTable(criterias);

        //    return pickCriterias;
        //}

        // FIXME: Dictionary no longer used, so this function will be removed
        //public static Dictionary<string, string> GroupCriteriaByTable(List<(string Tablename, string Criteria)> criterias) => criterias
        //    .GroupBy(
        //        p => p.Tablename,
        //        p => p.Criteria,
        //        (key, g) => new
        //        {
        //            TableName = key,
        //            Criterias = g.ToList()
        //        }
        //    )
        //    .ToDictionary(
        //        z => z.TableName,
        //        z => $"({z.Criterias.Combine(" AND ")})"
        //    );

    }
}
