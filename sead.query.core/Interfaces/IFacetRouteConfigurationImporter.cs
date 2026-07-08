namespace SeadQueryCore;

public interface IFacetRouteConfigurationImporter
{
    void ValidateFileSchemaOnly(string filePath);

    void ValidateFile(string filePath);

    void ImportFromFile(string filePath);
}
