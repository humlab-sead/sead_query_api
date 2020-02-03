using System;
using System.Collections.Generic;

namespace SeadQueryTest.Models
{
    public partial class Facet
    {
        public Facet()
        {
            FacetChildrenChildFacetCodeNavigation = new HashSet<FacetChildren>();
            FacetChildrenFacetCodeNavigation = new HashSet<FacetChildren>();
            FacetClause = new HashSet<FacetClause>();
            FacetDependencyDependencyFacet = new HashSet<FacetDependency>();
            FacetDependencyFacet = new HashSet<FacetDependency>();
            FacetTable = new HashSet<FacetTable>();
        }

        public int FacetId { get; set; }
        public string FacetCode { get; set; }
        public string DisplayTitle { get; set; }
        public string Description { get; set; }
        public int FacetGroupId { get; set; }
        public int FacetTypeId { get; set; }
        public string CategoryIdExpr { get; set; }
        public string CategoryNameExpr { get; set; }
        public string SortExpr { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsDefault { get; set; }
        public string AggregateType { get; set; }
        public string AggregateTitle { get; set; }
        public int AggregateFacetId { get; set; }

        public virtual FacetGroup FacetGroup { get; set; }
        public virtual FacetType FacetType { get; set; }
        public virtual ICollection<FacetChildren> FacetChildrenChildFacetCodeNavigation { get; set; }
        public virtual ICollection<FacetChildren> FacetChildrenFacetCodeNavigation { get; set; }
        public virtual ICollection<FacetClause> FacetClause { get; set; }
        public virtual ICollection<FacetDependency> FacetDependencyDependencyFacet { get; set; }
        public virtual ICollection<FacetDependency> FacetDependencyFacet { get; set; }
        public virtual ICollection<FacetTable> FacetTable { get; set; }
    }
}
