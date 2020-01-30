using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    /// <summary>
    /// Contains client data sent to server upon facet load content and load result requests.
    /// </summary>
    public class FacetsConfig2 {

        public class UserPickData
        {
            public string FacetCode { get; set; }
            public EFacetType FacetType { get; set; }
            public List<decimal> PickValues { get; set; }
            public string Title { get; set; }
        }

        /// <summary>
        /// Client request identity. Defined by client and value is returned without change.
        /// </summary>
        public string RequestId { get; set; } = "";

        [JsonIgnore]
        public string Language { get; set; } = "";
        /// <summary>
        /// Specifies request language. Only english supported in new version
        /// </summary>
        public string RequestType { get; set; } = "";       // Request specifier ("populate", ...)

        public string TargetCode { get; set; } = "";        // Target facet code i.e. facet for which new data is requested
        public string TriggerCode { get; set; } = "";       // Facet code that triggerd the request (some preceeding facet)

        [JsonIgnore]
        private List<FacetConfig2> facetConfigs;
        public List<FacetConfig2> FacetConfigs
        {                                                                               // Current client facet configurations
            get {
                return facetConfigs;
            }
            set {
                facetConfigs = value?.OrderBy(z => z.Position).ToList();
            }
        }

        public Facet TargetFacet { get; set; }
        public Facet TriggerFacet { get; set; }


        [JsonIgnore]
        public FacetConfig2 TargetConfig
        {
            get => GetConfig(TargetCode);
        }

        public FacetConfig2 GetConfig(string facetCode)         => FacetConfigs.FirstOrDefault(x => x.FacetCode == facetCode);
        public List<string> GetFacetCodes()                     => FacetConfigs.Select(x => x.FacetCode).ToList();
        public List<FacetConfig2> GetFacetConfigsWithPicks()    => FacetConfigs.Where(x => x.Picks.Count > 0).ToList();
        public List<string> GetFacetCodesWithPicks()            => GetFacetConfigsWithPicks().Select(x => x.FacetCode).ToList();

        public List<FacetConfig2> GetFacetConfigsAffectedBy(Facet targetFacet, List<string> facetCodes)
        {
            return facetCodes
                .Select(z => GetConfig(z))
                .Where(c => (c?.HasPicks() ?? false) && c.Facet.IsAffectedBy(facetCodes, targetFacet))
                .ToList();
        }

        public FacetsConfig2 ClearPicks()
        {
            FacetConfigs.ForEach(z => z.ClearPicks());
            return this;
        }

        public Dictionary<string, UserPickData> CollectUserPicks(string onlyCode = "")
        {

            Func<FacetConfig2, bool> filter() => x => (empty(onlyCode) || onlyCode == x.FacetCode) && (x.Picks.Count > 0);
            var values = new Dictionary<string, UserPickData>();
            foreach (var config in FacetConfigs.Where(filter())) {
                values[config.FacetCode] = new UserPickData() {
                    FacetCode = config.FacetCode,
                    PickValues = config.GetPickValues(),
                    FacetType = config.Facet.FacetTypeId,
                    Title = config.Facet.DisplayTitle
                };
            }
            return values;
        }

        public bool HasPicks(EFacetType facetType = EFacetType.Unknown)
        {
            return FacetConfigs.Any(z => z.Picks.Count > 0 && (facetType == EFacetType.Unknown || facetType == z.Facet.FacetTypeId));
        }

        public string GetPicksCacheId()
        {
            StringBuilder key = new StringBuilder("");
            foreach (var x in this.GetFacetConfigsWithPicks()) {
                key.AppendFormat("{0}_{1}", x.FacetCode, string.Join("_", x.GetPickValues(true).ToArray()));
            }
            return key.ToString();
        }

        public string GetCacheId()
        {
            //filter = ConfigRegistry::getFilterByText() ? this.targetFacet.textFilter : "no_text_filter";
            return TargetCode + "_" + string.Join("", GetFacetCodes()) +
                    "_" + GetPicksCacheId() +
                    "_" + Language + "_" + GetTargetTextFilter();
        }

        public string GetTargetTextFilter()
        {
            return (TargetConfig?.TextFilter ?? "").Trim();
        }

        public FacetTable GetFacetTable(string name)
        {
            foreach (var facetConfig in FacetConfigs) {
                if (facetConfig.GetFacetTable(name) != default(FacetTable)) {
                    return facetConfig.GetFacetTable(name);
                }
            }
            return default;
        }

    }
}
