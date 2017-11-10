using QuerySeadDomain.QueryBuilder;
using System;
using System.Linq;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain
{
    public interface ICategoryBoundsService  {
        List<Key2Value<int, float>> Load();
    }

    public class RangeCategoryBoundsService : QueryServiceBase, ICategoryBoundsService {

        private IIndex<EFacetType, ICategoryBoundSqlQueryBuilder> Compilers;

        public RangeCategoryBoundsService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IIndex<EFacetType, ICategoryBoundSqlQueryBuilder> compilers) : base(config, context, builder)
        {
            Compilers = compilers;
        }

        public List<Key2Value<int, float>> Load()
        {
            List<string> sqls = new List<string>();
            foreach (FacetDefinition facet in Context.Facets.GetOfType(EFacetType.Range)) {
                QuerySetup query = QueryBuilder.Build(null, facet.FacetCode, ToList(facet.TargetTableName), ToList(facet.FacetCode));
                sqls.Add(Compilers[EFacetType.Range].Compile(query, facet, facet.FacetCode));
            }
            string sql = String.Join("\nUNION\n", sqls);
            List<Key2Value<int, float>> values = Context.QueryKeyValues2<int, float>(sql).ToList();
            return values;
        }
    }
}
