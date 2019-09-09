using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.IO;

namespace SeadQueryTest.Infrastructure.Scaffolding
{

    public static class ScaffoldUtility
    {
        public static string GetRootFolder()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var parts = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var pos = parts.FindLastIndex(x => string.Equals("bin", x));
            string root = String.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(0, pos));
            return root;
        }

        public static ICollection<Type> GetModelTypes()
        {
            return new List<Type>() {
                    typeof(Facet),
                    typeof(FacetClause),
                    typeof(FacetGroup),
                    typeof(FacetTable),
                    typeof(FacetType),
                    typeof(GraphNode),
                    typeof(GraphTableRelation),
                    typeof(ResultAggregate),
                    typeof(ResultAggregateField),
                    typeof(ResultField),
                    typeof(ResultFieldType),
                    typeof(ResultViewType),
                    typeof(ViewState)
                };
        }

        public static FacetContext DefaultFacetContext()
        {
            var folder = Path.Combine(GetRootFolder(), "Infrastructure", "Data");
            return new FacetContextFixtureSeededByFolder(folder).FacetContext;
        }
    }
}
