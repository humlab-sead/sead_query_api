//using Microsoft.EntityFrameworkCore;
//using SeadQueryCore;
//using SeadQueryInfra;
//using SeadQueryTest.Fixtures;
//using SeadQueryTest.Infrastructure;
//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Reflection;

//namespace SeadQueryTest.Mocks
//{
//    public interface IFakeFacetContextSeeder
//    {
//        IFacetContext Seed(FacetContext context);
//    }

//    public class FakeFacetContextJsonSeeder : IFakeFacetContextSeeder
//    {
//        public string Folder { get; }

//        public FakeFacetContextJsonSeeder()
//        {
//            Folder = ScaffoldUtility.JsonDataFolder();
//        }

//        public IFacetContext Seed(FacetContext context)
//        {
//            var reader = new JsonReaderService(new IgnoreJsonAttributesResolver());
//            foreach (var type in ScaffoldUtility.GetModelTypes()) {
//                Seed(type, context, reader);
//            }
//            // context.SaveChanges();
//            return context;
//        }
//        public void Seed<T>(FacetContext context, JsonReaderService reader) where T : class, new()
//        {
//            var items = reader.Deserialize<T>(Folder).ToArray();

//            context.AddRange(items);
//            //foreach (var item in items) {
//            //    var entry = context.Entry(item);
//            //    entry.State = EntityState.Added;
//            //    foreach (var member in entry.Members) {
//            //        member.EntityEntry.State = EntityState.Unchanged;
//            //    }
//            //    try {
//            //        context.SaveChanges();
//            //    } catch {
//            //        throw;
//            //    }
//            //}
            
//        }

//        public void Seed(Type type, FacetContext context, JsonReaderService reader)
//        {
//            //MethodInfo method = typeof(FakeFacetContextJsonSeeder).GetMethod("SeedType");
//            //MethodInfo generic = method.MakeGenericMethod(type);

//            var method = typeof(FakeFacetContextJsonSeeder).GetMethods()
//                .Single(x => x.Name == "Seed" && x.IsGenericMethodDefinition)
//                .MakeGenericMethod(type);

//            method.Invoke(this, new object[] { context, reader });
//        }
//    }
//}
