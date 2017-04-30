using Autofac.Features.Indexed;
using QueryFacetDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static QueryFacetDomain.Utility;

namespace QueryFacetDomain
{
    public interface ICategoryBoundsService  {
        IEnumerable<Key2Value<int, float>> Load();
    }

    public class RangeCategoryBoundsService : QueryServiceBase, ICategoryBoundsService {

        public RangeCategoryBoundsService(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public IEnumerable<Key2Value<int, float>> Load()
        {
            List<string> sqls = new List<string>();
            foreach (FacetDefinition facet in Context.Facets.GetOfType(EFacetType.Range)) {
                QueryBuilder.QuerySetup query = QueryBuilder.Build(null, facet.FacetCode, ToList(facet.TargetTableName), ToList(facet.FacetCode));
                sqls.Add(new RangeCategoryBoundSqlQueryBuilder().compile(query, facet, facet.FacetCode));
            }
            string sql = String.Join("\nUNION\n", sqls);
            IEnumerable<Key2Value<int, float>> values = Context.QueryKeyValues2<int, float>(sql);
            return values;
        }
    }
}
