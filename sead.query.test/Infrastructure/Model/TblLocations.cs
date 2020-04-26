using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblLocations
    {
        public TblLocations()
        {
            TblRdb = new HashSet<TblRdb>();
            TblRdbSystems = new HashSet<TblRdbSystems>();
            TblRelativeAges = new HashSet<TblRelativeAges>();
            TblSiteLocations = new HashSet<TblSiteLocations>();
            TblTaxaSeasonality = new HashSet<TblTaxaSeasonality>();
        }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int LocationTypeId { get; set; }
        public decimal? DefaultLatDd { get; set; }
        public decimal? DefaultLongDd { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblLocationTypes LocationType { get; set; }
        public virtual ICollection<TblRdb> TblRdb { get; set; }
        public virtual ICollection<TblRdbSystems> TblRdbSystems { get; set; }
        public virtual ICollection<TblRelativeAges> TblRelativeAges { get; set; }
        public virtual ICollection<TblSiteLocations> TblSiteLocations { get; set; }
        public virtual ICollection<TblTaxaSeasonality> TblTaxaSeasonality { get; set; }
    }
}
