using Moq;
using SeadQueryCore;
using SQT.Infrastructure;
using SQT.Mocks;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.Model
{
    [Collection("SeadJsonFacetContextFixture")]
    public class FacetGroupTests : DisposableFacetContextContainer
    {
        public static List<object[]> TestData = new() {
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

        public FacetGroupTests(SeadJsonFacetContextFixture fixture) : base(fixture)
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
