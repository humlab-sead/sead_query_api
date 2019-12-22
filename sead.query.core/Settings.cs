using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore
{
    // public interface IStoreSetting {
    //     string ConnectionString { get; set; }
    //     bool UseRedisCache { get; set; }
    // }

    public class FacetSetting : IFacetSetting {
        public string CountTable { get; set; }
        public string CountColumn { get; set; }
    }

    public class StoreSetting /*: IStoreSetting*/ {
        public string Host { get; set; }
        public string Database { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseRedisCache { get; set; } = false;
        public string CacheHost { get; set; } = "";
        public int CachePort { get; set; } = 0;
    }

    public class QueryBuilderSetting: IQueryBuilderSetting {
        // https://msdn.microsoft.com/en-us/magazine/mt632279.aspx
        public QueryBuilderSetting()
        {

        }
        public QueryBuilderSetting(FacetSetting facet, StoreSetting store)
        {
            Facet = facet;
            Store = store;
        }
        public FacetSetting Facet { get; set; }
        public StoreSetting Store { get; set; }
    }
}
