using QuerySeadDomain.QueryBuilder;
using System;
using System.Linq;
using System.Collections.Generic;
using static QuerySeadDomain.Utility;

namespace QuerySeadDomain
{
    public interface ICategoryBoundsService  {
        List<Key2Value<int, float>> Load();
    }

    public class RangeCategoryBoundsService : QueryServiceBase, ICategoryBoundsService {

        public RangeCategoryBoundsService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public List<Key2Value<int, float>> Load()
        {
            List<string> sqls = new List<string>();
            foreach (FacetDefinition facet in Context.Facets.GetOfType(EFacetType.Range)) {
                QueryBuilder.QuerySetup query = QueryBuilder.Build(null, facet.FacetCode, ToList(facet.TargetTableName), ToList(facet.FacetCode));
                sqls.Add(new RangeCategoryBoundSqlQueryBuilder().Compile(query, facet, facet.FacetCode));
            }
            string sql = String.Join("\nUNION\n", sqls);
            List<Key2Value<int, float>> values = Context.QueryKeyValues2<int, float>(sql).ToList();
            return values;
        }
    }
}
