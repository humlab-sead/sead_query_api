using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SeadQueryTest.Infrastructure
{
    //public abstract class ExpectedData : IEnumerable<object[]>
    //{
    //    public abstract IEnumerator<object[]> GetEnumerator();

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}

    public class FacetTestBase
    {

        //protected static IContainer container;
        //protected string logDir = @"\temp\json\";
        protected readonly FacetContext _context;

        public FacetTestBase()
        {
            var contextOptions = new DbContextOptionsBuilder<FacetContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new FacetContext(contextOptions);

            _context.Database.EnsureCreated();

        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();

            _context.Dispose();
        }

        protected virtual void InitializeContext()
        {
            new FacetContextDefaultInitializer().Initialize(_context);
        }

        protected MethodInfo GetGenericMethodForType<T>(string name, Type type)
        {
            return typeof(T).GetMethod(name).MakeGenericMethod(new[] { type });
        }
        protected void PropertyValuesForEntityShouldBeEqualToExpected(Type type, object id, Dictionary<string, object> expected) // where T: class
        {
            // Arrange
            using (var context = ScaffoldUtility.DefaultFacetContext()) {

                // Act
                //var entity = context.Set<T>().Find(id);
                var entity = context.Find(type, new object[] { id });

                // Assert
                Assert.NotNull(entity);

                AssertPropertiesEquals(type, expected, entity);
            }
        }

        protected void AssertPropertiesEquals(Type type, Dictionary<string, object> expected, object entity)
        {
            foreach (KeyValuePair<string, object> entry in expected) {
                PropertyInfo targetProperty = type.GetProperty(entry.Key, BindingFlags.Public | BindingFlags.Instance);
                Assert.NotNull(targetProperty);
                Assert.Equal(entry.Value, targetProperty.GetValue(entity));
            }
        }

        protected void AssertPropertiesEquals(Dictionary<string, object> expected, object entity)
        {
            AssertPropertiesEquals(entity.GetType(), expected, entity);
        }

    }
}
