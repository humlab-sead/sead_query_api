using System;
using System.Collections.Generic;

namespace SQT.Models
{
    public partial class FacetTable
    {
        public int FacetTableId { get; set; }
        public int FacetId { get; set; }
        public int SequenceId { get; set; }
        public int TableId { get; set; }
        public string UdfCallArguments { get; set; }
        public string Alias { get; set; }

        public virtual Facet Facet { get; set; }
        public virtual Table Table { get; set; }
    }
}
