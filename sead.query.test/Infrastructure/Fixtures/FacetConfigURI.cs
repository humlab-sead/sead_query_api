using SeadQueryCore;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeadQueryTest.fixtures
{
    public class FacetConfigURI
    {
        private Regex tupleRegex = new Regex(@"^[\(](\d+)[\,](\d+)[\)]$");

        /// <summary>
        /// Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
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
            ExpectedRoutes = expectedTrails?.Select(z => RouteGenerator.ToPairs(z)).ToList();
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
}
