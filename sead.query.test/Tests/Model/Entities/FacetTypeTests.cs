using Moq;
using SeadQueryCore;
using SeadQueryInfra.DataAccessProvider;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTypeTests
    {
        public static List<object[]> TestData = new List<object[]>() {
            new object[] {
                typeof(FacetType),
                EFacetType.Geo,
                new Dictionary<string, object>() {
                    { "FacetTypeId", EFacetType.Geo },
                    { "FacetTypeName", "geo" },
                    { "ReloadAsTarget", true }
                }
            }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        {
            // Arrange
            using (var context = (FacetContext)JsonSeededFacetContextFactory.Create()) {
                // Act
                var entity = context.Find(type, new object[] { id });
                // Assert
                Assert.NotNull(entity);
                Asserter.EqualByDictionary(type, expected, entity);
            }
        }
    }
}
