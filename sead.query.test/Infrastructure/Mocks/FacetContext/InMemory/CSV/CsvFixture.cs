// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Linq;
// using CsvHelper;
// using CsvHelper.Configuration;

// public class CsvFacetContextDataFixture : IDisposable
// {
//     private readonly Lazy<Dictionary<Type, IEnumerable<object>>> _lazyItems;
//     public Dictionary<Type, IEnumerable<object>> Items => _lazyItems.Value;
//     public string Folder { get; }

//     public CsvFacetContextDataFixture(string folder)
//     {
//         Folder = folder;
//         _lazyItems = new Lazy<Dictionary<Type, IEnumerable<object>>>(Load);
//     }

//     private Dictionary<Type, IEnumerable<object>> Load()
//     {
//         var items = new Dictionary<Type, IEnumerable<object>>();
//         // Loop over all types in Dictionary instead of using ScaffoldUtility.GetModelTypes()
//         foreach (var type in ScaffoldUtility.GetModelTypes())
//         {
//             // call the generic Deserialize<T>
//             var method = typeof(CsvFacetContextDataFixture)
//                 .GetMethod(nameof(Deserialize), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
//                 .MakeGenericMethod(type);

//             var list = (IEnumerable<object>)method.Invoke(this, new object[] { Folder });
//             items[type] = list;
//         }
//         return items;
//     }

//     // this method will be invoked via reflection
//     private IEnumerable<T> Deserialize<T>(string folder) where T : class, new()
//     {
//         var file = Path.Combine(folder, $"{typeof(T).Name}.csv");
//         if (!File.Exists(file))
//             throw new FileNotFoundException($"CSV fixture not found: {file}");

//         using var reader = new StreamReader(file);
//         using var csv    = new CsvReader(reader, CultureInfo.InvariantCulture);

//         // If you need custom mapping (e.g. rename columns), you can configure here:
//         // var map = new DefaultClassMap<T>();
//         // map.AutoMap(CultureInfo.InvariantCulture);
//         // csv.Context.RegisterClassMap(map);

//         // this will read header first, then map each row to T
//         return csv.GetRecords<T>().ToList();
//     }

//     public void Dispose()
//     {
//         // nothing to clean up here, unless you hold resources
//     }

//     protected override void OnModelCreating(ModelBuilder builder)
// {
//     // 1) Apply all your normal entity‐to‐table configuration
//     base.OnModelCreating(builder);

//     // 2) Build a Type→TableName map for the PUBLIC schema
//     var tableMap = builder.Model
//         .GetEntityTypes()
//         .Where(et => et.GetSchema() == "public")               // if you only care public
//         .ToDictionary(
//             et => et.ClrType,
//             et => et.GetTableName()                            // relational metadata extension
//         );

//     // 3) Now seed via Csv, keyed by the real table name
//     foreach (var kvp in tableMap)
//     {
//         var clrType  = kvp.Key;
//         var table    = kvp.Value;                              // e.g. "order_items"
//         var csvPath  = Path.Combine(Fixture.Folder, $"{table}.csv");

//         if (!File.Exists(csvPath))
//             throw new FileNotFoundException($"Missing CSV for {clrType.Name} → {csvPath}");

//         // Deserialize<T> overload that takes filename
//         var method = typeof(CsvReaderService)
//             .GetMethod(nameof(CsvReaderService.Deserialize), BindingFlags.Instance | BindingFlags.Public)
//             .MakeGenericMethod(clrType);

//         // pass in folder + explicit filename
//         var records = (IEnumerable<object>)method.Invoke(
//             Fixture.Reader, 
//             new object[] { Fixture.Folder, $"{table}.csv" }
//         );

//         builder.Entity(clrType).HasData(records);
//     }
// }
// }
