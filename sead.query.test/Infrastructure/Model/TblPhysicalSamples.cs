using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblPhysicalSamples
    {
        public TblPhysicalSamples()
        {
            TblAnalysisEntities = new HashSet<TblAnalysisEntities>();
            TblPhysicalSampleFeatures = new HashSet<TblPhysicalSampleFeatures>();
            TblSampleAltRefs = new HashSet<TblSampleAltRefs>();
            TblSampleColours = new HashSet<TblSampleColours>();
            TblSampleCoordinates = new HashSet<TblSampleCoordinates>();
            TblSampleDescriptions = new HashSet<TblSampleDescriptions>();
            TblSampleDimensions = new HashSet<TblSampleDimensions>();
            TblSampleHorizons = new HashSet<TblSampleHorizons>();
            TblSampleImages = new HashSet<TblSampleImages>();
            TblSampleLocations = new HashSet<TblSampleLocations>();
            TblSampleNotes = new HashSet<TblSampleNotes>();
        }

        public int PhysicalSampleId { get; set; }
        public int SampleGroupId { get; set; }
        public int? AltRefTypeId { get; set; }
        public int SampleTypeId { get; set; }
        public string SampleName { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string DateSampled { get; set; }

        public virtual TblAltRefTypes AltRefType { get; set; }
        public virtual TblSampleGroups SampleGroup { get; set; }
        public virtual TblSampleTypes SampleType { get; set; }
        public virtual ICollection<TblAnalysisEntities> TblAnalysisEntities { get; set; }
        public virtual ICollection<TblPhysicalSampleFeatures> TblPhysicalSampleFeatures { get; set; }
        public virtual ICollection<TblSampleAltRefs> TblSampleAltRefs { get; set; }
        public virtual ICollection<TblSampleColours> TblSampleColours { get; set; }
        public virtual ICollection<TblSampleCoordinates> TblSampleCoordinates { get; set; }
        public virtual ICollection<TblSampleDescriptions> TblSampleDescriptions { get; set; }
        public virtual ICollection<TblSampleDimensions> TblSampleDimensions { get; set; }
        public virtual ICollection<TblSampleHorizons> TblSampleHorizons { get; set; }
        public virtual ICollection<TblSampleImages> TblSampleImages { get; set; }
        public virtual ICollection<TblSampleLocations> TblSampleLocations { get; set; }
        public virtual ICollection<TblSampleNotes> TblSampleNotes { get; set; }
    }
}
