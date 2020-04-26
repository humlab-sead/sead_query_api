using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblAnalysisEntities
    {
        public TblAnalysisEntities()
        {
            TblAbundances = new HashSet<TblAbundances>();
            TblAggregateSamples = new HashSet<TblAggregateSamples>();
            TblAnalysisEntityAges = new HashSet<TblAnalysisEntityAges>();
            TblAnalysisEntityDimensions = new HashSet<TblAnalysisEntityDimensions>();
            TblAnalysisEntityPrepMethods = new HashSet<TblAnalysisEntityPrepMethods>();
            TblCeramics = new HashSet<TblCeramics>();
            TblDendro = new HashSet<TblDendro>();
            TblDendroDates = new HashSet<TblDendroDates>();
            TblGeochronology = new HashSet<TblGeochronology>();
            TblIsotopes = new HashSet<TblIsotopes>();
            TblMeasuredValues = new HashSet<TblMeasuredValues>();
            TblRelativeDates = new HashSet<TblRelativeDates>();
            TblTephraDates = new HashSet<TblTephraDates>();
        }

        public int AnalysisEntityId { get; set; }
        public int? PhysicalSampleId { get; set; }
        public int? DatasetId { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblDatasets Dataset { get; set; }
        public virtual TblPhysicalSamples PhysicalSample { get; set; }
        public virtual ICollection<TblAbundances> TblAbundances { get; set; }
        public virtual ICollection<TblAggregateSamples> TblAggregateSamples { get; set; }
        public virtual ICollection<TblAnalysisEntityAges> TblAnalysisEntityAges { get; set; }
        public virtual ICollection<TblAnalysisEntityDimensions> TblAnalysisEntityDimensions { get; set; }
        public virtual ICollection<TblAnalysisEntityPrepMethods> TblAnalysisEntityPrepMethods { get; set; }
        public virtual ICollection<TblCeramics> TblCeramics { get; set; }
        public virtual ICollection<TblDendro> TblDendro { get; set; }
        public virtual ICollection<TblDendroDates> TblDendroDates { get; set; }
        public virtual ICollection<TblGeochronology> TblGeochronology { get; set; }
        public virtual ICollection<TblIsotopes> TblIsotopes { get; set; }
        public virtual ICollection<TblMeasuredValues> TblMeasuredValues { get; set; }
        public virtual ICollection<TblRelativeDates> TblRelativeDates { get; set; }
        public virtual ICollection<TblTephraDates> TblTephraDates { get; set; }
    }
}
