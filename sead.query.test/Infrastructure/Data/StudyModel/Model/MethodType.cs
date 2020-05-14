using System;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure.Data.StudyModel.Model
{
    public partial class MethodType
    {
        public MethodType()
        {
            Method = new HashSet<Method>();
        }

        public int MethodTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Method> Method { get; set; }
    }
}
