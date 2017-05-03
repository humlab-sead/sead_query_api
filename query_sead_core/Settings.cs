using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadDomain
{
    public interface IQueryBuilderSetting {
        FacetSetting Facet { get; set; }
        StoreSetting Store { get; set; }
    }

    public interface IFacetSetting {
        string DirectCountTable { get; set; }
        string DirectCountColumn { get; set; }
        string IndirectCountTable { get; set; }
        string IndirectCountColumn { get; set; }
        int ResultQueryLimit { get; set; }
        bool CategoryNameFilter { get; set; }
    }

    public interface IStoreSetting {
        string ConnectionString { get; set; }
        string CacheSeq { get; set; }
        string CacheDir { get; set; }
        int CurrentViewStateId { get; set; }
        string ViewStateTable { get; set; }
    }

    public class FacetSetting : IFacetSetting {
        public string DirectCountTable { get; set; }
        public string DirectCountColumn { get; set; }
        public string IndirectCountTable { get; set; }
        public string IndirectCountColumn { get; set; }
        public int ResultQueryLimit { get; set; } = 10000;
        public bool CategoryNameFilter { get; set; } = true;
    }

    public class StoreSetting : IStoreSetting {
        public string ConnectionString { get; set; }
        public string CacheSeq { get; set; } = "metainformation.file_name_data_download_seq";
        public string CacheDir { get; set; }
        public int CurrentViewStateId { get; set; } = 7;
        public string ViewStateTable { get; set; }
    }

    public class QueryBuilderSetting : IQueryBuilderSetting {
        // https://msdn.microsoft.com/en-us/magazine/mt632279.aspx
        public FacetSetting Facet { get; set; }
        public StoreSetting Store { get; set; }
    }
}
