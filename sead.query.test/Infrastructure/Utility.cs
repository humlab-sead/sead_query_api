using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;

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

    public static string GetPostgresDataFolder()
    {
        return GetRootFolder("Infrastructure/Mocks/FacetContext/PostgreSQL/Data/initdb.d");
    }

    public static string GetProjectRoot()
    {
        var assemblyDir = AppDomain.CurrentDomain.BaseDirectory;
        var probeFolder = new DirectoryInfo(assemblyDir);

        while (probeFolder != null)
        {
            if (Directory.EnumerateFiles(probeFolder.FullName, "*.csproj").Any())
            {
                return probeFolder.FullName;
            }
            probeFolder = probeFolder.Parent;
        }

        if (probeFolder == null)
        {
            throw new InvalidOperationException("Unable to determine project root.");
        }

        return probeFolder.FullName;
    }

    public static ICollection<Type> GetModelTypes()
    {
        return
        [
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
            typeof(Facet),
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

    public static string GetHostUserId()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows doesn't use UID/GID, so we return a default
            return "1000";
        }

        // For Linux/macOS: Use "id -u" to get current user ID
        var uid = ExecuteShellCommand("id -u");
        return string.IsNullOrWhiteSpace(uid) ? "1000" : uid.Trim();
    }

    public static string GetHostGroupId()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows doesn't use UID/GID, so we return a default
            return "1000";
        }

        // For Linux/macOS: Use "id -g" to get current group ID
        var gid = ExecuteShellCommand("id -g");
        return string.IsNullOrWhiteSpace(gid) ? "1000" : gid.Trim();
    }

    public static string ExecuteShellCommand(string command)
    {
        var processInfo = new ProcessStartInfo("bash", $"-c \"{command}\"")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = processInfo };
        process.Start();
        return process.StandardOutput.ReadToEnd();
    }
}
