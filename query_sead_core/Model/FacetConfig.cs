using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuerySeadDomain.QueryBuilder;
using static QuerySeadDomain.Utility;
using Newtonsoft.Json;

namespace QuerySeadDomain {

    public class QuerySeadException : Exception {

        public QuerySeadException(string msg, Exception ex) : base(msg, ex) { }
        public QuerySeadException(string msg) : base(msg) { }
        public QuerySeadException() : base() { }
    }

    public abstract class IsValidDomainEntitySpecification<T>
    {
        public abstract bool IsSatisfiedBy(T entity);
    }

    public class FacetsConfigSpecification : IsValidDomainEntitySpecification<FacetsConfig2>
    {

        public override bool IsSatisfiedBy(FacetsConfig2 facetsConfig)
        {
            if (!this.IsSatisfiedBy(facetsConfig.FacetConfigs))
            {
                return false;
            }
            if (facetsConfig.RequestId == "")
            {
                throw new QuerySeadException("Undefined request id");
            }
            if ((facetsConfig.TargetConfig ?? facetsConfig.TargetConfig) == null)
            {
                throw new QuerySeadException("Target facet is undefined");
            }
            if (facetsConfig.TriggerFacet == null)
            {
                throw new QuerySeadException("Trigger facet is undefined");
            }
            foreach (var facetCode in new List<string>() { facetsConfig.TargetFacet.FacetCode, facetsConfig.TriggerFacet.FacetCode })
            {
                if ( ! facetsConfig.FacetConfigs.Exists(z => z.FacetCode == facetCode))
                {
                    throw new QuerySeadException("Target or trigger facet code invalid (not found in any config)");
                }
            }
            return true;
        }

        public bool IsSatisfiedBy(List<FacetConfig2> configs)
        {
            if (0 == configs.Count())
            {
                throw new QuerySeadException("Facet chain is empty");
            }
            if (configs.Select(z => z.Position).Distinct().Count() != configs.Count())
            {
                throw new QuerySeadException("Facets' positions within facet chain are not unique");
            }
            if (configs.Select(z => z.FacetCode).Distinct().Count() != configs.Count())
            {
                throw new QuerySeadException("Facets' codes within facet chain are not unique");
            }
            return true;
        }
    }

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

        //public FacetsConfig2()
        //{
        //}
        [JsonConstructor]
        public FacetsConfig2(IUnitOfWork context)
        {
            Context = context;
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
        public IUnitOfWork Context { get; set; }

        [JsonIgnore]
        private List<FacetConfig2> facetConfigs;
        public List<FacetConfig2> FacetConfigs
        {                                                                               // Current client facet configurations
            get {
                return facetConfigs;
            }
            set {
                if (Context == null || new FacetsConfigSpecification().IsSatisfiedBy(value)) {
                    facetConfigs = value.OrderBy(z => z.Position).ToList();
                }
            }
        }

        public FacetsConfig2 SetContext(IUnitOfWork context)
        {
            Context = context;
            FacetConfigs.ForEach(z => z.Context = context);
            return this;
        }

        [JsonIgnore]
        public List<FacetConfig2> InactiveConfigs { get; set; }                         // Those having unset position

        private FacetDefinition targetFacet = null;
        private FacetDefinition triggerFacet = null;

        [JsonIgnore]
        public FacetDefinition TargetFacet                                              // Target facet definition
        {
            get => empty(TargetCode) ? null : (targetFacet ?? (targetFacet = Context?.Facets?.GetByCode(TargetCode)));
        }

        [JsonIgnore]
        public FacetDefinition TriggerFacet
        {
            get => empty(TriggerCode) ? null : (triggerFacet ?? (triggerFacet = Context?.Facets?.GetByCode(TriggerCode)));
        }

        [JsonIgnore]
        public FacetConfig2 TargetConfig
        {
            get => GetConfig(TargetCode);
        }

        public FacetConfig2 GetConfig(string facetCode)         => FacetConfigs.FirstOrDefault(x => x.FacetCode == facetCode);
        public List<string> GetFacetCodes()                     => FacetConfigs.Select(x => x.FacetCode).ToList();
        public List<FacetConfig2> GetFacetConfigsWithPicks()    => FacetConfigs.Where(x => x.Picks.Count > 0).ToList();
        public List<string> GetFacetCodesWithPicks()            => GetFacetConfigsWithPicks().Select(x => x.FacetCode).ToList();

        public List<FacetConfig2> GetFacetConfigsAffectedByFacet(List<string> facetCodes, FacetDefinition targetFacet)
        {
            var targetIndex = facetCodes.IndexOf(targetFacet.FacetCode);
            return facetCodes
                .Select(z => ((FacetCode: z, Config: GetConfig(z))))
                .Where(x => x.Config?.HasPicks() ?? false)
                .Where(x => x.Config.Facet.IsAffectedBy(facetCodes, targetFacet))
                .Select(x => x.Config)
                .ToList();
        }

        public FacetsConfig2 DeletePicks()
        {
            FacetConfigs.ForEach(z => { z.ClearPicks(); });
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
                // FIXME: Is this used? Can be computed as GroupBy(FacetType).Sum(Selections.Count)
                //matrix['counts'][config.facet.facet_type] += count(config.picks);
            }
            return values;

        }

        public bool HasPicks(EFacetType facetType = EFacetType.Unknown)
        {
            return FacetConfigs.Any(z => z.Picks.Count > 0 && (facetType == EFacetType.Unknown || facetType == z.Facet.FacetTypeId));
        }

        //public void deleteBogusPicks()
        //{
        //    new DeleteBogusPickService().Delete(this);
        //}

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
    }

    public class FacetConfig2 {

        public string FacetCode { get; set; } = "";
        public int Position { get; set; } = 0;
        public string TextFilter { get; set; } = "";

        // FIXM Refactor away dependency to Context
        [JsonIgnore]
        public IUnitOfWork Context { get; set; }    // FIXME Remove dependecy to Context

        public List<FacetConfigPick> Picks { get; set; }

        [JsonIgnore]
        public FacetDefinition Facet { get => Context?.Facets?.GetByCode(FacetCode); }

        [JsonConstructor]
        public FacetConfig2(IUnitOfWork context)
        {
            Context = context;
        }

        public FacetConfig2(string facetCode, int position, string filter, List<FacetConfigPick> picks)
        {
            FacetCode = facetCode;
            Position = position;
            TextFilter = filter;
            Picks = picks ?? new List<FacetConfigPick>();
        }

        public bool HasPicks() => (Picks?.Count ?? 0) > 0;
        public void ClearPicks() => Picks.Clear();

        public List<decimal> GetPickValues(bool sort = false)
        {
            List<decimal> values = Picks.Select(x => x.ToDecimal()).ToList();
            if (sort)
                values.Sort();
            return values;
        }

        public List<string> GetJoinTables()
        {
            var tables = Facet.ExtraTables.Select(z => z.TableName).ToList();
            tables.Add(Facet.ResolvedName);
            return tables;
        }

    }

    // FIXME Eliminate explicit EPickType - should be a configurable in case if new kinds of facets...
    public enum EPickType {
        unknown = 0,
        discrete = 1,
        lower = 2,
        upper = 3
    }

    public class FacetConfigPick {

        public EPickType PickType;
        public string PickValue;
        public string Text;

        public FacetConfigPick(EPickType type, string value, string text)
        {
            PickType = type;
            PickValue = value;
            Text = text;
        }

        public decimal ToDecimal()
        {
            return decimal.Parse(PickValue);
        }

        public int ToInt()
        {
            return int.Parse(PickValue);
        }

    }
}
