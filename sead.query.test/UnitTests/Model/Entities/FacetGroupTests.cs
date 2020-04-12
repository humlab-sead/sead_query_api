using Moq;
using SeadQueryCore;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetGroupTests : DisposableFacetContextContainer
    {
        public static List<object[]> TestData = new List<object[]>() {
            new object[] {
                typeof(FacetGroup),
                999,
                new Dictionary<string, object>() {
                    { "FacetGroupId", 999 },
                    { "FacetGroupKey", "DOMAIN" },
                    { "DisplayTitle", "Domain facets"},
                    { "IsApplicable", false},
                    { "IsDefault", false}
                }
            }
        };

        public FacetGroupTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Find_FromRepository_IsComplete(Type type, object id, Dictionary<string, object> expected)
        {
            // Act
            var entity = FacetContext.Find(type, new object[] { id });
            // Assert
            Assert.NotNull(entity);
            Asserter.EqualByDictionary(type, expected, entity);
        }
    }
}
