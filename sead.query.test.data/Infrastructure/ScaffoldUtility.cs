using Newtonsoft.Json;
using sead.query.test.data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sead.query.test.data.Infrastructure
{
    public static class ScaffoldUtility
    {
        public static string GetTestDataFolder()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var parts = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var pos = parts.FindLastIndex(x => string.Equals("bin", x));
            string root = String.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(0, pos));
            return Path.Combine(root, "Data");
        }

        public static ICollection<Type> GetModelTypes()
        {
            return new List<Type>() {
                typeof(Facet),
                typeof(FacetClause),
                typeof(FacetGroup),
                typeof(FacetTable),
                typeof(FacetType),
                typeof(GraphTable),
                typeof(GraphTableRelation),
                typeof(ResultAggregate),
                typeof(ResultAggregateField),
                typeof(ResultField),
                typeof(ResultFieldType),
                typeof(ResultViewType),
                typeof(ViewState)
            };
        }
    }
}
