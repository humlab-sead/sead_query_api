using System;
using System.Collections.Generic;

namespace SQT.Infrastructure.Model
{
    public partial class TblDatasets
    {
        public TblDatasets()
        {
            InverseUpdatedDataset = new HashSet<TblDatasets>();
            TblAnalysisEntities = new HashSet<TblAnalysisEntities>();
            TblDatasetContacts = new HashSet<TblDatasetContacts>();
            TblDatasetMethods = new HashSet<TblDatasetMethods>();
            TblDatasetSubmissions = new HashSet<TblDatasetSubmissions>();
        }

        public int DatasetId { get; set; }
        public int? MasterSetId { get; set; }
        public int DataTypeId { get; set; }
        public int? MethodId { get; set; }
        public int? BiblioId { get; set; }
        public int? UpdatedDatasetId { get; set; }
        public int? ProjectId { get; set; }
        public string DatasetName { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual TblBiblio Biblio { get; set; }
        public virtual TblDataTypes DataType { get; set; }
        public virtual TblDatasetMasters MasterSet { get; set; }
        public virtual TblMethods Method { get; set; }
        public virtual TblProjects Project { get; set; }
        public virtual TblDatasets UpdatedDataset { get; set; }
        public virtual ICollection<TblDatasets> InverseUpdatedDataset { get; set; }
        public virtual ICollection<TblAnalysisEntities> TblAnalysisEntities { get; set; }
        public virtual ICollection<TblDatasetContacts> TblDatasetContacts { get; set; }
        public virtual ICollection<TblDatasetMethods> TblDatasetMethods { get; set; }
        public virtual ICollection<TblDatasetSubmissions> TblDatasetSubmissions { get; set; }
    }
}
