using System.Collections.Generic;

namespace SeadQueryCore.QueryBuilder;

using Route = List<TableRelation>;

public interface IJoinsClauseCompiler
{
    List<string> Compile(List<Route> routes, FacetsConfig2 facetsConfig);
}
