using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{
    using Route = List<TableRelation>;

    public static class EdgesExtension
    {
        public static TableRelation GetEdge(this Route edges, string sourceTable, string targetTable)
        {
            return edges.FirstOrDefault(x => x.SourceName == sourceTable && x.TargetName == targetTable);
        }

        public static TableRelation GetEdge(this Route edges, int sourceTableId, int targetTableId)
        {
            return edges.FirstOrDefault(x => x.SourceTableId == sourceTableId && x.TargetTableId == targetTableId);
        }

        public static bool HasEdge(this Route route, TableRelation item)
        {
            return route.Any(x => x.SourceTableId == item.SourceTableId && x.TargetTableId == item.TargetTableId);
        }

        public static bool HasEdge(this Route route, string sourceName, string targetName)
        {
            return route.Any(x => x.SourceName == sourceName && x.TargetName == targetName);
        }

        public static bool HasEdge(this List<Route> routes, TableRelation item)
        {
            return routes.Any(x => x.HasEdge(item));
        }

        public static Route ReduceEdges(this Route route, List<Route> routes)
        {
            return route.Where(z => !routes.HasEdge(z)).ToList();
        }

        public static List<Route> ReduceEdges(this List<Route> routes)
        {
            List<Route> reduced_routes = [];
            foreach (var route in routes)
            {
                Route reduced_route = route.ReduceEdges(reduced_routes);
                if (reduced_route.Count > 0)
                {
                    reduced_routes.Add(reduced_route);
                }
            }
            return reduced_routes;
        }

        public static Route ReversedEdges(this Route route)
        {
            return route
                .Where(z => z.SourceId != z.TargetId)
                .Select(x => x.Reverse())
                .Where(z => !route.Any(w => w.Equals(z))).ToList();
        }


        public static Route GetFlattenEdges(this List<Route> routes)
            => [.. routes.SelectMany(route => route).OrderByDescending(z => z.TargetTable.IsUdf)];


        public static List<Tuple<int, int, int>> ToTuples(this Route edges)
        {
            return edges.Select(x => Tuple.Create(x.SourceId, x.TargetId, x.Weight)).ToList();
        }

        public static List<(int, int, int)> ToValueTuples(this Route edges)
        {
            return edges.Select(x => (x.SourceId, x.TargetId, x.Weight)).ToList();
        }

        public static string ToEdgeString(this Route route)
        {
            return string.Join("\n", route.Select(z => $"{z.SourceName};{z.TargetName};{z.Weight}"));
        }

        public static string ToEdgeString(this List<Route> routes)
        {
            return string.Join("\n", routes.Select(z => $"{z.ToEdgeString()}"));
        }

        public static List<string> ToTrail(this Route route)
        {
            if (route.Count > 0)
            {
                return route.Select(z => z.TargetName).Prepend(route[0].SourceName).ToList();
            }
            return [];
        }

        public static string ToCSV(this Route relations)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableRelation relation in relations)
                sb.Append($"{relation.SourceName};{relation.TargetName};{relation.Weight}\n");
            return sb.ToString();
        }

        public static TableRelation Find(this Route route, string sourceName, string targetName)
        {
            return route.FirstOrDefault(x => x.SourceName == sourceName && x.TargetName == targetName);
        }

        public static TableRelation Find(this Route route, int sourceId, int targetId)
        {
            return route.FirstOrDefault(x => x.SourceTableId == sourceId && x.TargetTableId == targetId);
        }

        public static Route ToEdges(this Route route, IEnumerable<int> trail)
            => trail.PairWise(route.Find).ToList();

        public static Table FindNode(this Route route, string nodeName)
        {
            foreach (var edge in route)
            {
                if (edge.SourceTable?.TableOrUdfName == nodeName)
                    return edge.SourceTable;
                if (edge.TargetTable?.TableOrUdfName == nodeName)
                    return edge.TargetTable;
            }
            return null;
        }

        public static Dictionary<string, Table> GetNodes(this Route route)
        {
            var nodes = new Dictionary<string, Table>();
            foreach (var edge in route)
            {
                if (!nodes.ContainsKey(edge.SourceName))
                    nodes[edge.SourceName] = edge.SourceTable;
                if (!nodes.ContainsKey(edge.TargetName))
                    nodes[edge.TargetName] = edge.TargetTable;
            }
            return nodes;
        }
        
    }

    public class TableRelation
    {
        public int TableRelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }
        //public string ExtraConstraint { get; set; }

        public string SourceColumName { get; set; }
        public string TargetColumnName { get; set; }

        [JsonIgnore] public int SourceId { get { return SourceTableId; } }
        [JsonIgnore] public int TargetId { get { return TargetTableId; } }

        [JsonIgnore] private Table _SourceTable, _TargetTable;

        public Table SourceTable { get { return _SourceTable; } set { _SourceTable = value; SourceTableId = value?.TableId ?? SourceTableId; } }
        public Table TargetTable { get { return _TargetTable; } set { _TargetTable = value; TargetTableId = value?.TableId ?? TargetTableId; } }

        [JsonIgnore] public string SourceName { get { return SourceTable?.TableOrUdfName ?? ""; } }
        [JsonIgnore] public string TargetName { get { return TargetTable?.TableOrUdfName ?? ""; } }

        public TableRelation Clone()
        {
            return new TableRelation()
            {
                TableRelationId = TableRelationId + 1000,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
                SourceColumName = SourceColumName,
                TargetColumnName = TargetColumnName
            };
        }

        public TableRelation Reverse()
        {
            var x = Clone();
            x.TableRelationId = -x.TableRelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
            (x.SourceColumName, x.TargetColumnName) = (x.TargetColumnName, x.SourceColumName);
            return x;
        }

        public TableRelation Alias(Table node, Table alias)
        {
            var x = Clone();
            if (node.TableId == SourceTable.TableId)
                x.SourceTable = alias;
            else
                x.TargetTable = alias;
            return x;
        }

        public Tuple<string, string> Key => new(SourceName, TargetName);
        public Tuple<int, int> IdKey => new(SourceTableId, TargetTableId);

        public bool IsOf(TableRelation x)
        {
            return x != null && IsOf(x.SourceName, x.TargetName);
        }

        public bool IsOf(string sourceName, string targetName)
        {
            return SourceName == sourceName && TargetName == targetName;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object x)
        {
            return IsOf(x as TableRelation);
        }

        public bool InvolvesAny(int[] tableIds)
        {
            return tableIds.Contains(SourceTableId) || tableIds.Contains(TargetTableId);
        }
    }
}
