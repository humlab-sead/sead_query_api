using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;
using static SeadQueryCore.Utility;

namespace SeadQueryCore.Services
{
    public class ReportService : QueryServiceBase
    {
        public string FacetCode { get; protected set; }

        public ReportService(IRepositoryRegistry context, IQuerySetupBuilder builder) : base(context, builder)
        {
            FacetCode = "distinct_expr";
        }

    }
}
