using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblMethods
    {
        public TblMethods()
        {
            TblAnalysisEntityPrepMethods = new HashSet<TblAnalysisEntityPrepMethods>();
            TblCeramicsLookup = new HashSet<TblCeramicsLookup>();
            TblCeramicsMeasurements = new HashSet<TblCeramicsMeasurements>();
            TblColours = new HashSet<TblColours>();
            TblCoordinateMethodDimensions = new HashSet<TblCoordinateMethodDimensions>();
            TblDatasetMethods = new HashSet<TblDatasetMethods>();
            TblDatasets = new HashSet<TblDatasets>();
            TblDendroLookup = new HashSet<TblDendroLookup>();
            TblDendroMeasurements = new HashSet<TblDendroMeasurements>();
            TblHorizons = new HashSet<TblHorizons>();
            TblIsotopeMeasurements = new HashSet<TblIsotopeMeasurements>();
            TblRelativeDates = new HashSet<TblRelativeDates>();
            TblSampleDimensions = new HashSet<TblSampleDimensions>();
            TblSampleGroups = new HashSet<TblSampleGroups>();
            TblSiteNatgridrefs = new HashSet<TblSiteNatgridrefs>();
        }

        public int MethodId { get; set; }
        public int? BiblioId { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Description { get; set; }
        public string MethodAbbrevOrAltName { get; set; }
        public int MethodGroupId { get; set; }
        public string MethodName { get; set; }
        public int? RecordTypeId { get; set; }
        public int? UnitId { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblMethodGroups MethodGroup { get; set; }
        public virtual TblRecordTypes RecordType { get; set; }
        public virtual TblUnits Unit { get; set; }
        public virtual ICollection<TblAnalysisEntityPrepMethods> TblAnalysisEntityPrepMethods { get; set; }
        public virtual ICollection<TblCeramicsLookup> TblCeramicsLookup { get; set; }
        public virtual ICollection<TblCeramicsMeasurements> TblCeramicsMeasurements { get; set; }
        public virtual ICollection<TblColours> TblColours { get; set; }
        public virtual ICollection<TblCoordinateMethodDimensions> TblCoordinateMethodDimensions { get; set; }
        public virtual ICollection<TblDatasetMethods> TblDatasetMethods { get; set; }
        public virtual ICollection<TblDatasets> TblDatasets { get; set; }
        public virtual ICollection<TblDendroLookup> TblDendroLookup { get; set; }
        public virtual ICollection<TblDendroMeasurements> TblDendroMeasurements { get; set; }
        public virtual ICollection<TblHorizons> TblHorizons { get; set; }
        public virtual ICollection<TblIsotopeMeasurements> TblIsotopeMeasurements { get; set; }
        public virtual ICollection<TblRelativeDates> TblRelativeDates { get; set; }
        public virtual ICollection<TblSampleDimensions> TblSampleDimensions { get; set; }
        public virtual ICollection<TblSampleGroups> TblSampleGroups { get; set; }
        public virtual ICollection<TblSiteNatgridrefs> TblSiteNatgridrefs { get; set; }
    }
}
