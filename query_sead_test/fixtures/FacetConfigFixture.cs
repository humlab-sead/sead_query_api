using QuerySeadDomain;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuerySeadTests.fixtures
{


    public class FacetConfigURI
    {
        private Regex tupleRegex = new Regex(@"^[\(](\d+)[\,](\d+)[\)]$");

        /// <summary>
        /// Uri format: "taget-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        /// </summary>
        public string UriConfig { get; set; }
        public List<List<string>> ExpectedRoutes { get; set; }
        public int ExpectedCount { get; set; }
        public List<(int, int)> ExpectedData { get; set; }
        public string TargetCode { get; set; }
        public string TriggerCode { get; set; }
        public Dictionary<string, List<FacetConfigPick>> FacetCodes { get; set; }

        public FacetConfigURI(string uriConfig, List<List<string>> expectedTrails = null, int expectedCount = 0)
        {
            UriConfig = uriConfig;
            ExpectedRoutes = expectedTrails?.Select(z => TestRoute.ToPairs(z)).ToList();
            ExpectedCount = expectedCount;
            var parts = uriConfig.Split(':').ToList();
            var codes = parts[0].Split("@");
            TargetCode = codes[0];
            TriggerCode = codes.Length > 1 ? codes[1] : TargetCode;
            var facetConfigs =  parts.Count > 1 ? parts[1] : TargetCode;
            FacetCodes = facetConfigs
                .Split('/')
                .Where(z => z.Trim() != "")
                .Select(z => z.Split("@"))
                .Select(z => (FacetCode: z[0].Trim(), Picks: z.Length > 1 ? ParsePicks(z[1]) : null))
                .ToDictionary(z => z.FacetCode, z => z.Picks);
        }

        public List<FacetConfigPick> ParsePicks(string data)
        {
            Match m  = tupleRegex.Match(data);
            if (m.Success && m.Groups.Count == 3) {
                return FacetConfigPick.CreateLowerUpper(Decimal.Parse(m.Groups[1].Value), Decimal.Parse(m.Groups[2].Value));
            }
            return data.Split(",").Select(z => new FacetConfigPick(EPickType.discrete, z)).ToList();
        }

    }

    public class FacetConfigGenerator
    {

        public IContainer Container { get; set; }
        public IUnitOfWork Context { get; set; }
        public FacetConfigFixtureData Data { get; set; }

        public FacetConfigGenerator()
        {
            Container = new TestDependencyService().Register();
            Context = Container.Resolve<IUnitOfWork>();
            Data = new FacetConfigFixtureData();
        }

        public FacetsConfig2 GenerateFacetsConfig(string targetCode, string triggerCode, List<FacetConfig2> facetConfigs)
        {
            return new FacetsConfig2(Context)
            {
                RequestId = "1",
                Language = "",
                TargetCode = targetCode,
                RequestType = "populate",
                TriggerCode = triggerCode,
                FacetConfigs = facetConfigs
            };
        }

        public FacetConfig2 GenerateFacetConfig(string facetCode, int position, List<FacetConfigPick> picks = null, string filter = "")
        {
            return new FacetConfig2(Context)
            {
                FacetCode = facetCode,
                Position = position,
                TextFilter = filter,
                Picks = picks ?? new List<FacetConfigPick>()
            };
        }

        public List<FacetConfigPick> GenerateDiscreteFacetPicks(List<int> ids)
        {
            return ids.Select(z => new FacetConfigPick(EPickType.discrete, z.ToString(), z.ToString())).ToList();
        }

        public FacetsConfig2 GenerateSingleFacetsConfigWithoutPicks(string facetCode)
        {
            return GenerateFacetsConfig(facetCode, facetCode, new List<FacetConfig2>() { GenerateFacetConfig(facetCode, 0, new List<FacetConfigPick>()) });
        }

        public FacetsConfig2 GenerateByConfig(FacetConfigURI config)
        {
            var position = 0;
            var facetConfigs = config.FacetCodes.Select(z => GenerateFacetConfig(z.Key, position++, z.Value)).ToList();
            return GenerateFacetsConfig(config.TargetCode, config.TargetCode, facetConfigs);
        }

        public FacetsConfig2 GenerateByUri(string uri, List<List<string>> expectedTrails = null, int expectedCount = 0)
        {
            return GenerateByConfig(new FacetConfigURI(uri, expectedTrails, expectedCount));
        }

    }

    public class TestRoute
    {
        public List<string> Trail { get; set; }
        public List<string> Pairs { get { return ToPairs(Trail); } }
        public TestRoute(List<string> trail)
        {
            Trail = trail;
        }
        public static List<string> ToPairs(List<string> trail)
        {
            return trail.Take(trail.Count - 1).Select((e, i) => e + "/" + trail[i + 1]).ToList();
        }

        public static List<string> ToPairs(params string[] trail)
        {
            return ToPairs(trail.ToList());
        }

    }
}
