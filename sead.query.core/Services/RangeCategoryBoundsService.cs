using SeadQueryCore.QueryBuilder;
using System;
using System.Linq;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{

    public class RangeCategoryBoundsService : QueryServiceBase, ICategoryBoundsService {

        private IIndex<EFacetType, ICategoryBoundSqlQueryCompiler> Compilers;

        public RangeCategoryBoundsService(
            IRepositoryRegistry context,
            IQuerySetupCompiler builder,
            IIndex<EFacetType, ICategoryBoundSqlQueryCompiler> compilers) : base(context, builder)
        {
            Compilers = compilers;
        }

        public List<Key2Value<int, float>> Load()
        {
            List<string> sqls = new List<string>();
            foreach (Facet facet in Context.Facets.GetOfType(EFacetType.Range)) {
                QuerySetup query = QuerySetupBuilder.Build(null, facet, ToList(facet.TargetTable.ObjectName), ToList(facet.FacetCode));
                sqls.Add(Compilers[EFacetType.Range].Compile(query, facet, facet.FacetCode));
            }
            string sql = String.Join("\nUNION\n", sqls);
            List<Key2Value<int, float>> values = Context.QueryKeyValues2<int, float>(sql).ToList();
            return values;
        }
    }
}
