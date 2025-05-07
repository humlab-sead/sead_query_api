using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class ProjectProperty
    {
        public int ProjectPropertyId { get; set; }
        public string PropertyName { get; set; }
        public int PropertyValue { get; set; }
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
