using System;
using System.IO;
using SeadQueryCore;

namespace SeadQueryAPI.Services;

public sealed class FacetRouteConfigurationImportCommand
{
    private readonly IFacetRouteConfigurationImporter _importer;

    public FacetRouteConfigurationImportCommand(IFacetRouteConfigurationImporter importer)
    {
        _importer = importer ?? throw new ArgumentNullException(nameof(importer));
    }

    public void Run(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Facet route configuration import requires a file path.", nameof(filePath));

        _importer.ImportFromFile(ResolvePath(filePath));
    }

    private static string ResolvePath(string filePath)
    {
        var absolutePath = Path.GetFullPath(filePath);
        if (File.Exists(absolutePath))
        {
            return absolutePath;
        }

        var parentRelativePath = Path.GetFullPath(Path.Combine("..", filePath));
        return File.Exists(parentRelativePath) ? parentRelativePath : absolutePath;
    }
}
