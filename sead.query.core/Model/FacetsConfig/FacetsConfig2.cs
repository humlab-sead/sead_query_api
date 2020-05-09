using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
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

        /// <summary>
        /// Facet domain under which current request is valid. Specifies a domain facet.
        /// </summary>
        public string DomainCode { get; set; } = "";
        /// <summary>
        /// Request specifier ("populate", ...)
        /// </summary>
        public string RequestType { get; set; } = "";       // Request specifier ("populate", ...)

        public string TargetCode { get; set; } = "";        // Target facet code i.e. facet for which new data is requested

        /// <summary>
        /// Facet code that triggered the request (some preceeding facet)
        /// Not used by Core Logic, pass-through, round-trip variable
        /// </summary>
        public string TriggerCode { get; set; } = "";       //

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

        [JsonIgnore]
        public Facet DomainFacet { get; set; }
        /// <summary>
        /// The target facet of interest for which we want to produce content or result.
        /// </summary>
        public Facet TargetFacet { get; set; }


        [JsonIgnore]
        public FacetConfig2 TargetConfig
        {
            get => GetConfig(TargetCode);
        }

        public FacetConfig2 GetConfig(string facetCode)                 => FacetConfigs.FirstOrDefault(x => x.FacetCode == facetCode);
        public FacetConfig2 GetConfig(Facet facet)                      => GetConfig(facet.FacetCode);

        public List<FacetConfig2> GetConfigs(List<string> facetCodes)   => FacetConfigs.Where(c => facetCodes.Contains(c.FacetCode)).ToList();

        public List<string> GetFacetCodes()                             => FacetConfigs.Select(x => x.FacetCode).ToList();

        public List<FacetConfig2> GetFacetConfigsWithPicks()            => FacetConfigs.Where(x => x.Picks.Count > 0).ToList();

        public bool IsPriorTo(FacetConfig2 facetConfig, List<string> facetCodes, Facet targetFacet)
        {
            if (!facetConfig.HasConstraints()) {
                // FIXME Is this a relevant condition?
                return false;
            }

            if (targetFacet.FacetCode == facetConfig.FacetCode)
                return targetFacet.FacetType.ReloadAsTarget;

            return facetCodes.IndexOf(targetFacet.FacetCode) > facetCodes.IndexOf(facetConfig.FacetCode);

        }

        public List<FacetConfig2> GetFacetConfigsAffectedBy(Facet facet, List<string> facetCodes)
        {
            facetCodes = (facetCodes ?? GetFacetCodes()).AddIfMissing(facet.FacetCode);
            return GetConfigs(facetCodes)
                .Where(c => IsPriorTo(c, facetCodes, facet))
                    .ToList();
        }

        public List<FacetConfig2> GetInvolvedConfigs(Facet facet, List<string> facetCodes=null)
            => GetFacetConfigsAffectedBy(facet, facetCodes);

        public List<Facet> GetInvolvedFacets(Facet facet, List<string> facetCodes=null)
            => GetInvolvedConfigs(facet, facetCodes)
                .Facets().AddUnion(facet)
                    .ToList();

        public List<string> GetInvolvedTables(Facet facet, List<string> facetCodes=null, List<string> extraTables=null)
            => GetInvolvedFacets(facet, facetCodes)
                .TableNames().NullableUnion(extraTables)
                        .Distinct().ToList();

        /// <summary>
        /// Returns the facets (list of FacetConfig) involved in a query given FacetsConfig2 `facetsConfig` configuration and target facet `targetCode`
        ///    => if facetsCode preceeds targetFacet AND has picks
        /// </summary>
        /// <param name="targetCode"></param>
        /// <param name="facetCodes"></param>
        /// <returns></returns>
        public List<FacetConfig2> GetConfigsThatAffectsTarget(string targetCode, List<string> facetCodes)
        {
            if (!facetCodes.Contains(targetCode))
                throw new ArgumentOutOfRangeException($"{nameof(targetCode)} ({targetCode}) not in {nameof(facetCodes)}");

            List<FacetConfig2> configs = new List<FacetConfig2>();

            foreach (var currentCode in facetCodes) {

                var currentConfig = GetConfig(currentCode);
                var currentFacet = currentConfig.Facet;

                if (! (currentConfig.HasPicks() || currentConfig.HasEnforcedConstraints())) {
                    /* If the facet doesn't have constraints then it cannot affect the target facet */
                    continue;
                }

                if (facetCodes.IndexOf(currentCode) < facetCodes.IndexOf(targetCode)) {
                    /* Has constraints and preceeds in chain */
                    configs.Add(currentConfig);
                } else
                if (currentCode == targetCode) {
                    /* Range facets also affects themselves */
                    if (currentFacet.FacetType.ReloadAsTarget) {
                        configs.Add(currentConfig);
                    }
                }
            }

            return configs;
        }

        public FacetsConfig2 ClearPicks()
        {
            FacetConfigs.ForEach(z => z.ClearPicks());
            return this;
        }

        public Dictionary<string, UserPickData> CollectUserPicks(string onlyCode = "")
        {

            Func<FacetConfig2, bool> filter() => x => (onlyCode.IsEmpty() || onlyCode == x.FacetCode) && (x.Picks.Count > 0);
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
                    "_" + DomainCode + "_" + GetTargetTextFilter();
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

        public bool HasDomainCode() =>
            !string.IsNullOrEmpty(DomainCode);

        public FacetConfig2 CreateDomainConfig() =>
            HasDomainCode() ? FacetConfigFactory.Create(DomainFacet, 0) : null;

    }
}
