using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQT.Scaffolding;

public static class IEnumerableExtensions
{
    public static string BuildString<T>(this IEnumerable<T> self, string delim = ",", string apos = "")
    {
        return string.Join(delim, self.Select(x => $"{apos}{x}{apos}"));
    }
}


public static class ScaffoldUtility
{

    public static string GetRootFolder(string subfolder = "")
    {
        string path = AppDomain.CurrentDomain.BaseDirectory;
        var parts = new List<string>(path.Split(Path.DirectorySeparatorChar));
        var pos = parts.FindLastIndex(x => string.Equals("bin", x));
        string folder = string.Join(Path.DirectorySeparatorChar.ToString(), parts.GetRange(0, pos));
        if (!string.IsNullOrEmpty(subfolder))
        {
            folder = Path.Combine(folder, subfolder);
        }
        return folder;
    }

    public static string GetDataFolder(string format = "")
    {
        string root = GetRootFolder();
        if (string.IsNullOrEmpty(format))
        {
            return Path.Combine(root, "Infrastructure", "Data");
        }
        else
        {
            return Path.Combine(root, "Infrastructure", "Data", format);
        }
    }

    public static string GetPostgresDataFolder()
    {
        return GetRootFolder("Infrastructure/Mocks/FacetContext/PostgreSQL/Data/initdb.d");
    }

    public static ICollection<Type> GetModelTypes()
    {
        return [
                typeof(ResultFieldType),
                typeof(ResultField),
                typeof(ResultViewType),
                typeof(FacetType),
                typeof(FacetGroup),
                typeof(Table),
                typeof(FacetClause),
                typeof(FacetChild),
                typeof(FacetTable),
                typeof(TableRelation),
                typeof(ResultSpecificationField),
                typeof(ResultSpecification),
                typeof(ViewState),
                typeof(Facet)
            ];
    }


    public static IPathFinder DefaultRouteFinder(IRepositoryRegistry registry)
    {
        return new PathFinder(registry.Relations.GetEdges());
    }
    public static IPathFinder DefaultRouteFinder(IFacetContext testContext)
    {
        using (var registry = new RepositoryRegistry(testContext))
        {
            var g = DefaultRouteFinder(registry);
            return g;
        }
    }
}