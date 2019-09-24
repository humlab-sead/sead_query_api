using System;
using System.Collections.Generic;

namespace sead.query.test.data.Entities
{
    public partial class FacetType
    {

        public int FacetTypeId { get; set; }
        public string FacetTypeName { get; set; }
        public bool ReloadAsTarget { get; set; }

    }
}
