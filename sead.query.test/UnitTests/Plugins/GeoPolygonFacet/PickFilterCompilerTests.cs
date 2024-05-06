using SeadQueryCore;
using SeadQueryCore.Plugin.GeoPolygon;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace SQT.SqlCompilers
{
    [Collection("SeadJsonFacetContextFixture")]
    public class GeoPolygonPickFilterCompilerTests(SeadJsonFacetContextFixture fixture) : DisposableFacetContextContainer(fixture)
    {
        [Fact]
        public void Compile_WhenNoPicks_ReturnsEmptyString()
        {
            var compiler = new GeoPolygonPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites_polygon");
            Facet currentFacet = Registry.Facets.GetByCode("country");
            var config = new FacetConfig2(targetFacet, 1, "", []);

            var result = compiler.Compile(targetFacet, currentFacet, config);

            Assert.Equal(currentFacet.Criteria, result);
        }

        [Fact]
        public void Compile_WhenTargetAndCurrentFacetAreTheSame_ReturnsEmptyString()
        {
            var compiler = new GeoPolygonPickFilterCompiler();
            Facet targetFacet = Registry.Facets.GetByCode("sites_polygon");
            Facet currentFacet = targetFacet;
            var config = new FacetConfig2(targetFacet, 1, "A = B", FacetConfigPick.CreateByList([0, 0, 0, 1, 1, 1, 1, 0]));

            var result = compiler.Compile(targetFacet, currentFacet, config);

            Assert.Equal(
                "ST_Within(" +
                    "ST_MakePoint(tbl_sites.latitude_dd, tbl_sites.longitude_dd), " +
                    "ST_MakePolygon(" +
                        "ST_MakeLine(" +
                            "ARRAY[ST_MakePoint(0, 0), ST_MakePoint(0, 1), ST_MakePoint(1, 1), ST_MakePoint(1, 0), ST_MakePoint(0, 0)]" +
                        ")" +
                    ")" +
                ")", result
            );
        }

        [Fact]
        public void Compile_WhenHasPicksAndTargetAndCurrentFacetAreNotTheSame_ReturnsCriteria()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var compiler = new GeoPolygonPickFilterCompiler();
                var picks = new List<int>() { 1, 2, 3 };
                Facet targetFacet = Registry.Facets.GetByCode("sites_polygon");
                Facet currentFacet = Registry.Facets.GetByCode("country");
                FacetConfig2 config = new FacetConfig2(targetFacet, 1, "", FacetConfigPick.CreateByList(picks));

                var result = compiler.Compile(targetFacet, currentFacet, config);
            });

            Assert.Equal("Invalid polygon sizes 3", ex.Message);

        }
    }
}
