using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetClauseTests : FacetTestBase
    {
        public static List<object[]> TestData = new List<object[]>() {
            new object[] {
                typeof(FacetClause),
                4,
                new Dictionary<string, object>() {
                    { "FacetSourceTableId", 4 },
                    { "FacetId", 33  },
                    { "Clause", "metainformation.view_abundance.abundance is not null" }
                }
            }
        };

        [Theory]
        [MemberData(nameof(TestData))]
        public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        {
            // Arrange
            using (var context = ScaffoldUtility.JsonSeededFacetContext()) {
                // Act
                var entity = context.Find(type, new object[] { id });
                // Assert
                Assert.NotNull(entity);
                Asserter.EqualByDictionary(type, expected, entity);
            }
        }
    }
}
