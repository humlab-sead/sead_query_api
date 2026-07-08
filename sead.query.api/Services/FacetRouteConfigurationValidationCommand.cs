using System;
using System.IO;
using SeadQueryCore;

namespace SeadQueryAPI.Services;

public sealed class FacetRouteConfigurationValidationCommand
{
    private readonly IFacetRouteConfigurationImporter _importer;

    public FacetRouteConfigurationValidationCommand(IFacetRouteConfigurationImporter importer)
    {
        _importer = importer ?? throw new ArgumentNullException(nameof(importer));
    }

    public void Run(string filePath, bool offline = false)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Facet route configuration validation requires a file path.", nameof(filePath));

        var resolvedPath = ResolvePath(filePath);

        if (offline)
        {
            _importer.ValidateFileSchemaOnly(resolvedPath);
            return;
        }

        _importer.ValidateFile(resolvedPath);
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
