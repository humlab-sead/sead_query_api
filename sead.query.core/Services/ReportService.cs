using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services
{
    public class ReportService : QueryServiceBase
    {
        public string FacetCode { get; protected set; }

        // TODO Allow for transient, on-the-fly result facets
        public ReportService(IRepositoryRegistry context, IQuerySetupBuilder builder) : base(context, builder)
        {
            FacetCode = "distinct_expr";
        }

    }
}
