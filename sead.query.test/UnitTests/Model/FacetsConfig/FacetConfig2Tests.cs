using Moq;
using Newtonsoft.Json;
using SeadQueryCore;
using SQT.Infrastructure;
using System.Collections.Generic;
using Xunit;

namespace SQT.Model
{
    [Collection("UsePostgresFixture")]
    public class FacetConfig2Tests : MockerWithFacetContext
    {
        public FacetConfig2Tests() : base()
        {
        }

        [Fact]
        public void HasPicks_WhenRangeFacetHasPicks_IsTrue()
        {
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = [new FacetConfigPick(3M), new FacetConfigPick(52M)]
            };
            var result = facetConfig2.HasPicks();

            Assert.True(result);
        }

        [Fact]
        public void HasPicks_WhenFacetHasNoPicks_IsFalse()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = new List<FacetConfigPick>()
            };
            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPicks_WhenDiscreteFacetHasPicks_IsTrue()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateByList(new List<int>() { 1, 2, 3 })
            };
            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ClearPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateByList(new List<int>() { 1, 2, 3 })
            };

            // Act
            facetConfig2.ClearPicks();

            // Assert
            Assert.False(facetConfig2.HasPicks());
        }

        [Fact]
        public void GetPickValues_WhenHasPicks_Success()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateByList(new List<int>() { 1, 2, 3 })
            };
            const bool sort = false;

            // Act
            var result = facetConfig2.GetPickValues(sort);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(1, result[0]);
            Assert.Equal(2, result[1]);
            Assert.Equal(3, result[2]);
        }

        [Fact]
        public void Polygons_WhenDeserialized_PicksAreGroupedByPolygon()
        {
            // Arrange
            var json = @"{
                ""facetCode"": ""sites_polygon"",
                ""position"": 1,
                ""polygons"": [
                    [ [1.5, 2.5], [3.5, 4.5], [5.5, 6.5] ],
                    [ [11.5, 12.5], [13.5, 14.5], [15.5, 16.5] ]
                ]
            }";

            // Act
            var facetConfig2 = JsonConvert.DeserializeObject<FacetConfig2>(json);

            // Assert
            Assert.Equal(12, facetConfig2.GetPickCount());
            Assert.Equal(2, facetConfig2.GetPolygonCount());

            var groups = facetConfig2.GetPickValueGroups();
            Assert.Equal(2, groups.Count);
            Assert.Equal(new List<decimal>() { 1.5M, 2.5M, 3.5M, 4.5M, 5.5M, 6.5M }, groups[0]);
            Assert.Equal(new List<decimal>() { 11.5M, 12.5M, 13.5M, 14.5M, 15.5M, 16.5M }, groups[1]);
        }

        [Fact]
        public void Coordinates_WhenDeserialized_PicksBelongToASinglePolygon()
        {
            // Arrange
            var json = @"{
                ""facetCode"": ""sites_polygon"",
                ""position"": 1,
                ""coordinates"": [ [1.5, 2.5], [3.5, 4.5], [5.5, 6.5] ]
            }";

            // Act
            var facetConfig2 = JsonConvert.DeserializeObject<FacetConfig2>(json);

            // Assert
            var groups = facetConfig2.GetPickValueGroups();
            Assert.Equal(6, facetConfig2.GetPickCount());
            Assert.Equal(1, facetConfig2.GetPolygonCount());
            Assert.Single(groups);
            Assert.Equal(new List<decimal>() { 1.5M, 2.5M, 3.5M, 4.5M, 5.5M, 6.5M }, groups[0]);
        }

        [Fact]
        public void GetPickValueGroups_WhenPicksHaveNoPolygonIndex_ReturnsSingleGroup()
        {
            // Arrange
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "dummy_code",
                Facet = new Mock<Facet>().Object,
                Position = 0,
                Picks = FacetConfigPick.CreateByList([1, 2, 3])
            };

            // Act
            var result = facetConfig2.GetPickValueGroups();

            // Assert
            Assert.Equal(1, facetConfig2.GetPolygonCount());
            Assert.Single(result);
            Assert.Equal(new List<decimal>() { 1M, 2M, 3M }, result[0]);
        }

        [Fact]
        public void GetJoinTables_StateUnderTest_Success()
        {
            // Arrange
            var facet = Registry.Facets.GetByCode("result_facet");
            var facetConfig2 = new FacetConfig2
            {
                FacetCode = "result_facet",
                Facet = facet,
                Position = 0,
                Picks = FacetConfigPick.CreateByList([1, 2, 3])
            };
            // Act
            var result = facetConfig2.GetJoinTables();

            // Assert
            var expected = new List<string>() { "tbl_analysis_entities", "tbl_datasets", "tbl_physical_samples" };
            Assert.Equal(expected, result);
        }
    }
}
