using Microsoft.EntityFrameworkCore;
using Npgsql;
using SeadQueryInfra;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SQT.Scaffolding;

public static class Utility
{
    public static DbContextOptionsBuilder<FacetContext> GetDbContextOptionBuilder(string hostName, string databaseName)
    {
        var connectionString = ConnectionStringFactory.Create(hostName, databaseName);
        var optionsBuilder = new DbContextOptionsBuilder<FacetContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return optionsBuilder;
    }


    public static async Task LoadData(string schema, string csvDir, string connectionString)
    {
        if (!Directory.Exists(csvDir))
            return;

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        await using var transaction = await conn.BeginTransactionAsync();

        await using (var cmd = new NpgsqlCommand("set constraints all deferred;", conn, transaction))
        {
            await cmd.ExecuteNonQueryAsync();
        }

        foreach (var csvFile in Directory.EnumerateFiles(csvDir, "*.csv"))
        {
            var tableName = Path.GetFileNameWithoutExtension(csvFile);
            var headerLine = File.ReadLines(csvFile).FirstOrDefault();
            var columns = headerLine.Split(',').Select(c => c.Trim()).ToArray();

            var copySql = $"COPY {schema}.\"{tableName}\" ({string.Join(", ", columns)}) FROM STDIN (FORMAT csv, HEADER true)";
            await using var importer = conn.BeginTextImport(copySql);

            using var reader = File.OpenText(csvFile);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                await importer.WriteAsync(line + "\n");
            }

            await importer.FlushAsync();
        }
        await transaction.CommitAsync();
    }
    
}
