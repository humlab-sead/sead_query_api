using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDendroDates
    {
        public TblDendroDates()
        {
            TblDendroDateNotes = new HashSet<TblDendroDateNotes>();
        }

        public int DendroDateId { get; set; }
        public int AnalysisEntityId { get; set; }
        public int? AgeOlder { get; set; }
        public int? AgeYounger { get; set; }
        public int? DatingUncertaintyId { get; set; }
        public int? SeasonOrQualifierId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? ErrorPlus { get; set; }
        public int? ErrorMinus { get; set; }
        public int DendroLookupId { get; set; }
        public int? ErrorUncertaintyId { get; set; }
        public int AgeTypeId { get; set; }

        public virtual TblAgeTypes AgeType { get; set; }
        public virtual TblAnalysisEntities AnalysisEntity { get; set; }
        public virtual TblDatingUncertainty DatingUncertainty { get; set; }
        public virtual TblDendroLookup DendroLookup { get; set; }
        public virtual TblErrorUncertainties ErrorUncertainty { get; set; }
        public virtual ICollection<TblDendroDateNotes> TblDendroDateNotes { get; set; }
    }
}
