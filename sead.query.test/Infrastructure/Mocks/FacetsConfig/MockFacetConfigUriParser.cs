﻿using SeadQueryCore;
using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SeadQueryTest.Mocks
{
    internal class MockFacetConfigUriParser
    {
        private static Regex tupleRegex = new Regex(@"^[\(](\d+)[\,](\d+)[\)]$");

        /// <summary>
        /// Uri format: "target-facet[@trigger-facet]:(facet-code[@picks])(/facet-code[@picks])*
        /// </summary>

        public class UriData
        {
            public string Uri { get; set; }
            public string TargetCode { get; set; }
            public string TriggerCode { get; set; }
            public Dictionary<string, List<FacetConfigPick>> FacetCodes { get; set; }
        }

        public UriData Parse(string uri)
        {
            var parts = uri.Split(':').ToList();
            var codes = parts[0].Split("@");
            var targetCode = codes[0];
            var triggerCode = codes.Length > 1 ? codes[1] : targetCode;
            var facetConfigCodes =  parts.Count > 1 ? parts[1] : targetCode;
            var facetCodes = facetConfigCodes
                .Split('/')
                .Where(z => !String.IsNullOrEmpty(z?.Trim()))
                .Select(z => z.Split("@"))
                .Select(z => (FacetCode: z[0].Trim(), Picks: z.Length > 1 ? ParsePicks(z[1]) : null))
                .ToDictionary(z => z.FacetCode, z => z.Picks);

            return new UriData {
                Uri = uri,
                TargetCode = targetCode,
                TriggerCode = triggerCode,
                FacetCodes = facetCodes
            };
        }

        private List<FacetConfigPick> ParsePicks(string data)
        {
            var cultureInfo = new CultureInfo("en-US");

            Match m  = tupleRegex.Match(data);

            if (m.Success && m.Groups.Count == 3) {

                var lower = Decimal.Parse(m.Groups[1].Value, NumberStyles.Any, cultureInfo);
                var upper = Decimal.Parse(m.Groups[2].Value, NumberStyles.Any, cultureInfo);

                return FacetConfigPick.CreateLowerUpper(lower, upper);
            }

            return data.Split(",").Select(z => new FacetConfigPick(EPickType.discrete, z)).ToList();
        }
    }
}