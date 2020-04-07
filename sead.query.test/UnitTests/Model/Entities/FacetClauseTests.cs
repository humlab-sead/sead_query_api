using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetClauseTests : DisposableFacetContextContainer
    {
        public static List<object[]> TestData = new List<object[]>() {
            new object[] {
                typeof(FacetClause),
                4,
                new Dictionary<string, object>() {
                    { "FacetClauseId", 4 },
                    { "FacetId", 33  },
                    { "Clause", "facet.view_abundance.abundance is not null" }
                }
            }
        };

        public FacetClauseTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        {
            var entity = Context.Find(type, new object[] { id });
            Assert.NotNull(entity);
            Asserter.EqualByDictionary(type, expected, entity);
        }
    }
}
