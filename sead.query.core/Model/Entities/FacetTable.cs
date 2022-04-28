using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{
    /// <summary>
    /// A relational table associated to a facet
    /// </summary>
    public class FacetTable
    {
        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public virtual int SequenceId { get; set; }

        public int TableId { get; set; }
        public string UdfCallArguments { get; set; }
        public virtual string Alias { get; set; }

        public Table Table { get; set; }

        [JsonIgnore]
        public string TableOrUdfName { get { return Table?.TableOrUdfName; } }

        [JsonIgnore]
        public virtual Facet Facet { get; set; }

        [JsonIgnore]
        public bool HasAlias => !Alias.IsEmpty();

        [JsonIgnore]
        public string ResolvedAliasOrTableOrUdfName => Alias.IsEmpty() ? TableOrUdfName : Alias;

        [JsonIgnore]
        public string ResolvedTableOrUdfCall =>
            UdfCallArguments.IsEmpty() ? TableOrUdfName : $"{TableId}{UdfCallArguments}";

        [JsonIgnore]
        public string ResolvedSqlJoinName =>
            Alias.IsEmpty() ? ResolvedTableOrUdfCall : $"{ResolvedTableOrUdfCall} AS {Alias}";

    }
}
