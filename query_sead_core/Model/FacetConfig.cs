using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuerySeadDomain.QueryBuilder;
using static QuerySeadDomain.Utility;
using Newtonsoft.Json;

namespace QuerySeadDomain {

    public class FacetsConfig2 {

        public string RequestId { get; set; } = "";
        public string Language { get; set; } = "";
        public string RequestType { get; set; } = "";       // Request specifier ("populate", ...)
        public string TargetCode { get; set; } = "";        // Target facet code i.e. facet for which new data is requested
        public string TriggerCode { get; set; } = "";       // Facet code that triggerd the request (some preceeding facet)

        [JsonIgnore]
        public IUnitOfWork context { get; set; }

        [JsonIgnore]
        private List<FacetConfig2> facetConfigs;
        public List<FacetConfig2> FacetConfigs
        {                                                                               // Current client facet configurations
            get {
                return facetConfigs;
            }
            set {
                facetConfigs = value.OrderBy(z => z.Position).ToList();
            }
        }

        [JsonIgnore]
        public List<FacetConfig2> InactiveConfigs { get; set; }                         // Those having unset position

        public FacetDefinition TargetFacet                                              // Target facet definition
        {
            get => empty(TargetCode) ? null : context.Facets.GetByCode(TargetCode);
        }

        public FacetDefinition TriggerFacet
        {
            get => empty(TriggerCode) ? null : context.Facets.GetByCode(TriggerCode);
        }

        public FacetConfig2 TargetConfig
        {
            get => GetConfig(TargetCode);
  
        }

        public FacetsConfig2()
        {
        }

        public FacetsConfig2(IUnitOfWork context)
        {
        }

        public FacetsConfig2(IUnitOfWork context, string requestId, string language, List<FacetConfig2> facetConfigs, string requestType = "", string targetCode = "", string triggerCode = "") : this(context)
        {
            RequestId = requestId;
            Language = language;
            RequestType = requestType;
            TargetCode = targetCode;
            TriggerCode = triggerCode;
            FacetConfigs =  facetConfigs;
        }

        public FacetConfig2 GetConfig(string facetCode)         => FacetConfigs.FirstOrDefault(x => x.FacetCode == facetCode);
        public List<string> GetFacetCodes()                     => FacetConfigs.Select(x => x.FacetCode).ToList();
        public List<FacetConfig2> GetFacetConfigsWithPicks()    => FacetConfigs.Where(x => x.Picks.Count > 0).ToList();
        public List<string> GetFacetCodesWithPicks()            => GetFacetConfigsWithPicks().Select(x => x.FacetCode).ToList();

        public FacetsConfig2 DeletePicks()
        {
            FacetConfigs.ForEach(z => { z.ClearPicks(); });
            return this;
        }

        public class UserPickData {
            public string FacetCode { get; set; }
            public EFacetType FacetType { get; set; }
            public List<int> PickValues { get; set; }
            public string Title { get; set; }
        }

        public Dictionary<string, UserPickData> collectUserPicks(string onlyCode = "")
        {

            Func<FacetConfig2, bool> filter() => x => (empty(onlyCode) || onlyCode == x.FacetCode) && (x.Picks.Count > 0);
            Dictionary<string, UserPickData> pickCounts = new Dictionary<string, UserPickData>();
            foreach (var config in FacetConfigs.Where(filter())) {
                pickCounts.Add(config.FacetCode, new UserPickData() {
                    FacetCode = config.FacetCode,
                    PickValues = config.GetPickValues(),
                    FacetType = config.Facet.FacetTypeId,
                    Title = config.Facet.DisplayTitle
                });
                // FIXME: Is this used? Can be computed as GroupBy(FacetType).Sum(Selections.Count)
                //matrix['counts'][config.facet.facet_type] += count(config.picks);
            }
            return pickCounts;

        }

        public bool HasPicks(EFacetType facetType = EFacetType.Unknown)
        {
            return FacetConfigs.Any(z => z.Picks.Count > 0 && (facetType == EFacetType.Unknown || facetType == z.Facet.FacetTypeId));
        }

        //public void deleteBogusPicks()
        //{
        //    DeleteBogusPickService.DeleteBogusPicks(context, this);
        //}

        public string GetPicksCacheId()
        {
            StringBuilder key = new StringBuilder("");
            foreach (var x in this.GetFacetConfigsWithPicks()) {
                key.AppendFormat("{0}_{1}", x.FacetCode, string.Join("_", x.GetPickValues(true).ToArray()));
            }
            return key.ToString();
        }

        public string getCacheId()
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
        public int StartRow { get; set; } = 0;
        public int RowCount { get; set; } = 0;
        public string TextFilter { get; set; } = "";

        [JsonIgnore]
        public IUnitOfWork context { get; set; }

        public List<FacetConfigPick> Picks { get; set; }

        [JsonIgnore]
        public FacetDefinition Facet { get => context.Facets.GetByCode(FacetCode); }

        public FacetConfig2()
        {
        }

        public FacetConfig2(string facetCode, int position, int startRow, int rowCount, string filter, List<FacetConfigPick> picks)
        {
            FacetCode = facetCode;
            Position = position;
            StartRow = startRow;
            RowCount = rowCount;
            TextFilter = filter;
            Picks = picks;
        }

        public List<int> GetPickValues(bool sort = false)
        {
            List<int>  values = Picks.Select(x => x.PickValue).ToList();
            if (sort)
                values.Sort();
            return values;
        }

        public void ClearPicks()
        {
            Picks.Clear();
        }

        // TODO: Move to SqlQueryBuilder
        public string GetTextFilterClause()
        {
            return (TextFilter == "") ? "" : $" AND {Facet.CategoryNameExpr} ILIKE '{TextFilter}' ";
        }

        public (int StartRow, int RowCount) GetPage()
        {
            return (StartRow, RowCount);
        }

        public Dictionary<EFacetPickType, decimal> GetPickedLowerUpperBounds()
        {
            return Picks
                .GroupBy(x => x.PickType)
                .Select(g => new { Type = g.Key, Value = g.Max(z => (decimal)z.PickValue) })
                .ToDictionary(x => x.Type, y => y.Value);
        }

        public dynamic /*(decimal lower, decimal upper)*/ getStorageLowerUpperBounds()
        {
            return context.Facets.GetUpperLowerBounds(Facet);
        }

    }

    public enum EFacetPickType {
        unknown = 0,
        discrete = 1,
        lower = 2,
        upper = 3
    }

    public class FacetConfigPick {

        public EFacetPickType PickType;
        public int PickValue;
        public string Text;

        public FacetConfigPick(EFacetPickType type, int value, string text)
        {
            PickType = type;
            PickValue = value;
            Text = text;
        }
    }

}
