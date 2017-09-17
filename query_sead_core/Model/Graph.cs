using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuerySeadDomain
{
    public class GraphNode {

        public int TableId { get; set; }
        public string TableName { get; set; }

    }

    public class GraphEdge {

        public int RelationId { get; set; }
        public int SourceTableId { get; set; }
        public int TargetTableId { get; set; }
        public int Weight { get; set; }

        public string SourceColumnName { get; set; }
        public string TargetColumnName { get; set; }

        [JsonIgnore] private GraphNode _SourceTable, _TargetTable;

        public GraphNode SourceTable { get { return _SourceTable; } set { _SourceTable = value; SourceTableId = value?.TableId ?? SourceTableId;  } }
        public GraphNode TargetTable { get { return _TargetTable; } set { _TargetTable = value; TargetTableId = value?.TableId ?? TargetTableId; } }

        [JsonIgnore] public string SourceTableName { get { return SourceTable.TableName; } }
        [JsonIgnore] public string TargetTableName { get { return TargetTable.TableName; } }

        public GraphEdge Clone()
        {
            return new GraphEdge() {
                RelationId = RelationId + 1000,
                Weight = Weight,
                SourceTableId = SourceTableId,
                TargetTableId = TargetTableId,
                SourceTable = SourceTable,
                TargetTable = TargetTable,
                SourceColumnName = SourceColumnName,
                TargetColumnName = TargetColumnName
            };
        }

        public GraphEdge Reverse()
        {
            var x = Clone();
            x.RelationId = -x.RelationId;
            (x.SourceTableId, x.TargetTableId) = (x.TargetTableId, x.SourceTableId);
            (x.SourceTable, x.TargetTable) = (x.TargetTable, x.SourceTable);
            (x.SourceColumnName, x.TargetColumnName) = (x.TargetColumnName, x.SourceColumnName);
            return x;
        }

        public GraphEdge Alias(GraphNode node, GraphNode alias)
        {
            var x = Clone();
            if (node.TableId == SourceTable.TableId)
                x.SourceTable = alias;
            else
                x.TargetTable = alias;
            return x;
        }

        public Tuple<string, string> Key { get { return new Tuple<string, string>(SourceTableName, TargetTableName); } }

        public bool EqualAs(GraphEdge x)
        {
            //return (SourceTableId == x.SourceTableId) && (TargetTableId == x.TargetTableId);
            return (SourceTableName == x.SourceTableName) && (TargetTableName == x.TargetTableName);
        }

        public string ToStringPair()
        {
            return $"{SourceTableName}/{TargetTableName}";
        }
    }

    public class GraphRoute
    {

        public List<GraphEdge> Items { get; set; } = new List<GraphEdge>();

        public GraphRoute(List<GraphEdge> items)
        {
            Items = items;
        }

        public bool Contains(GraphEdge item)
        {
            return Items.Any(x => x.SourceTable.TableId == item.SourceTable.TableId && x.TargetTable.TableId == item.TargetTable.TableId);
        }

        public GraphRoute ReduceBy(List<GraphRoute> routes)
        {
            return new GraphRoute(Items.Where(z => !Utility.ExistsAny(routes, z)).ToList());
        }

        public override string ToString()
        {
            return String.Join("\n", Items.Select(z => $"{z.SourceTableName};{z.TargetTableName};{z.Weight}"));
        }

        public class Utility
        {

            public static bool ExistsAny(List<GraphRoute> routes, GraphEdge item)
            {
                return routes.Any(x => x.Contains(item));
            }

            public static List<GraphRoute> Reduce(List<GraphRoute> routes)
            {
                List<GraphRoute> reduced_routes = new List<GraphRoute>();
                foreach (var route in routes)
                {
                    GraphRoute reduced_route = route.ReduceBy(reduced_routes);
                    if (reduced_route.Items.Count > 0)
                    {
                        reduced_routes.Add(reduced_route);
                    }
                }
                return reduced_routes;
            }

            public static string ToString(List<GraphRoute> routes)
            {
                return String.Join("\n", routes.Select(z => $"{routes.IndexOf(z)};{z.ToString()}"));
            }

        }
    }

}
