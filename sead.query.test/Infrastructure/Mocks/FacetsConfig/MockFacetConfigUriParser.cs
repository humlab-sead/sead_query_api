using SeadQueryCore;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SQT.Mocks
{
    /// <summary>
    /// Parses a URI that specifies a facetsConfig setup.
    /// The URI must be of format:
    ///     "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
    /// Picks are comma separated. For geopolygon facets several polygons can be
    /// specified, separated by ';'.
    /// </summary>
    internal class MockFacetConfigUriParser
    {
        private static Regex tupleRegex = new Regex(@"^[\(](\d+)[\,](\d+)[\)]$");

        /// <summary>
        /// Placeholder for parsed URI of format:
        ///     "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        /// </summary>
        public class UriData
        {
            public string Domain { get; set; }
            public string Uri { get; set; }
            public string TargetCode { get; set; }
            public Dictionary<string, List<FacetConfigPick>> FacetCodes { get; set; }
        }

        /// <summary>
        /// Parsed URI of format:
        ///     "[domain://]target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>@UriData instance with parsed data</returns>
        public UriData Parse(string uri)
        {
            var domain = "";
            var domainParts = uri.Split("://").ToList();

            if (domainParts.Count > 1)
            {
                domain = domainParts[0];
                uri = domainParts[1];
            }

            var parts = uri.Split(':').ToList();
            var codes = parts[0].Split("@");
            var targetCode = codes[0];
            // var triggerCode = codes.Length > 1 ? codes[1] : targetCode;
            var facetConfigCodes = parts.Count > 1 ? parts[1] : targetCode;

            var facetCodes = facetConfigCodes
                .Split('/')
                .Where(z => !String.IsNullOrEmpty(z?.Trim()))
                .Select(z => z.Split("@"))
                .Select(z => (
                    FacetCode: z[0].Trim(),
                    Picks: z.Length > 1 ? ParsePicks(z[1]) : null
                 ))
                .ToDictionary(z => z.FacetCode, z => z.Picks);

            return new UriData
            {
                Domain = domain,
                Uri = uri,
                TargetCode = targetCode,
                FacetCodes = facetCodes
            };
        }

        private List<FacetConfigPick> ParsePicks(string data)
        {
            var cultureInfo = new CultureInfo("en-US");

            Match m = tupleRegex.Match(data);

            if (m.Success && m.Groups.Count == 3)
            {
                var lower = Decimal.Parse(m.Groups[1].Value, NumberStyles.Any, cultureInfo);
                var upper = Decimal.Parse(m.Groups[2].Value, NumberStyles.Any, cultureInfo);

                return [
                new FacetConfigPick(lower),
                new FacetConfigPick(upper)
                ];
            }

            /* Several polygons (geopolygon facets) are separated by ';' e.g. "1,1,2,2,3,3;4,4,5,5,6,6" */
            return data
                .Split(";")
                .SelectMany((polygon, index) => polygon
                    .Split(",")
                    .Select(z => new FacetConfigPick(z) { PolygonIndex = index }))
                .ToList();
        }
    }
}
