using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore
{

    public class GraphRoute
    {
        public List<TableRelation> Items { get; set; } = new List<TableRelation>();

        public GraphRoute()
        {
        }

        public GraphRoute(IEnumerable<TableRelation> items)
        {
            Items = items.ToList();
        }

        public bool Contains(TableRelation item)
        {
            return Items.Any(x => x.SourceTable.TableId == item.SourceTable.TableId && x.TargetTable.TableId == item.TargetTable.TableId);
        }

        public GraphRoute ReduceBy(List<GraphRoute> routes)
        {
            return new GraphRoute(Items.Where(z => !GraphRouteUtility.ExistsAny(routes, z)).ToList());
        }

        public override string ToString()
        {
            return String.Join("\n", Items.Select(z => $"{z.SourceName};{z.TargetName};{z.Weight}"));
        }

        public List<string> Trail()
        {
            if (Items.Count > 0)
            {
                return Items.Select(z => z.TargetName).Prepend(Items[0].SourceName).ToList();
            }
            return new List<string>();
        }
    }
}
