using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblSampleGroups
    {
        public TblSampleGroups()
        {
            TblChronologies = new HashSet<TblChronologies>();
            TblLithology = new HashSet<TblLithology>();
            TblPhysicalSamples = new HashSet<TblPhysicalSamples>();
            TblSampleGroupCoordinates = new HashSet<TblSampleGroupCoordinates>();
            TblSampleGroupDescriptions = new HashSet<TblSampleGroupDescriptions>();
            TblSampleGroupDimensions = new HashSet<TblSampleGroupDimensions>();
            TblSampleGroupImages = new HashSet<TblSampleGroupImages>();
            TblSampleGroupNotes = new HashSet<TblSampleGroupNotes>();
            TblSampleGroupReferences = new HashSet<TblSampleGroupReferences>();
        }

        public int SampleGroupId { get; set; }
        public int? SiteId { get; set; }
        public int? SamplingContextId { get; set; }
        public int MethodId { get; set; }
        public string SampleGroupName { get; set; }
        public string SampleGroupDescription { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblMethods Method { get; set; }
        public virtual TblSampleGroupSamplingContexts SamplingContext { get; set; }
        public virtual TblSites Site { get; set; }
        public virtual ICollection<TblChronologies> TblChronologies { get; set; }
        public virtual ICollection<TblLithology> TblLithology { get; set; }
        public virtual ICollection<TblPhysicalSamples> TblPhysicalSamples { get; set; }
        public virtual ICollection<TblSampleGroupCoordinates> TblSampleGroupCoordinates { get; set; }
        public virtual ICollection<TblSampleGroupDescriptions> TblSampleGroupDescriptions { get; set; }
        public virtual ICollection<TblSampleGroupDimensions> TblSampleGroupDimensions { get; set; }
        public virtual ICollection<TblSampleGroupImages> TblSampleGroupImages { get; set; }
        public virtual ICollection<TblSampleGroupNotes> TblSampleGroupNotes { get; set; }
        public virtual ICollection<TblSampleGroupReferences> TblSampleGroupReferences { get; set; }
    }
}
