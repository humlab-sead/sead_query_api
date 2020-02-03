using Newtonsoft.Json;

namespace SeadQueryCore
{
    public partial class FacetChild
    {
        public string FacetCode { get; set; }
        public string ChildFacetCode { get; set; }
        public int Position { get; set; }

        public virtual Facet Facet { get; set; }
        public virtual Facet Child { get; set; }
    }
}
