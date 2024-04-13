using System.Collections.Generic;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface ISqlCompiler
    {
    }

    public class CompilePayload // : Dictionary<string, object>
    {
        /// <summary>
        /// TargetFacet is the target filter facet
        /// </summary>
        public Facet TargetFacet { get; set; }
        /// <summary>
        /// ResultFacet specifies the result (fields)
        /// </summary>
        public Facet ResultFacet { get; set; }
        /// <summary>
        /// AggregateFacet specifies how the result should be aggregated
        /// </summary>
        public Facet AggregateFacet { get; set; }
        public string IntervalQuery { get; set; }
        public string CountColumn { get; set; }
        public string AggregateType { get; set; }

    }
    public interface ICategoryCountSqlCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, Facet facet, CompilePayload payload);
    }

}
