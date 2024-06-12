using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.QueryBuilder;

using Route = List<TableRelation>;

public class JoinsClauseCompiler(IRepositoryRegistry registry, IJoinSqlCompiler joinCompiler) : IJoinsClauseCompiler
{
    public IJoinSqlCompiler JoinCompiler { get; } = joinCompiler;
    public IRepositoryRegistry Registry { get; set; } = registry;

    private FacetTable GetFacetTableByNameOrAlias(FacetsConfig2 facetsConfig, TableRelation edge)
        => facetsConfig.GetFacetTable(edge.TargetName) ?? GetByAlias(edge.TargetName);

    public virtual List<string> Compile(List<Route> routes, FacetsConfig2 facetsConfig)
    {
        var joins = routes.GetFlattenEdges().Select(
                edge => JoinCompiler.Compile(edge, GetFacetTableByNameOrAlias(facetsConfig, edge), true)
            ).ToList();
        return joins;
    }

    private FacetTable GetByAlias(string aliasName) => Registry.FacetTables.GetByAlias(aliasName);

}
