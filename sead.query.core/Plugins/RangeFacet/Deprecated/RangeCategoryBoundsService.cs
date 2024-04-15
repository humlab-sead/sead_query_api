//using SeadQueryCore.QueryBuilder;
//using System;
//using System.Linq;
//using System.Collections.Generic;
//using Autofac.Features.Indexed;
//using static SeadQueryCore.Utility;

//namespace SeadQueryCore
//{

//    public class RangeCategoryBoundsService : QueryServiceBase, ICategoryBoundsService {

//        private IIndex<EFacetType, ICategoryBoundSqlCompiler> Compilers;

//        public RangeCategoryBoundsService(
//            IRepositoryRegistry context,
//            IQuerySetupBuilder builder,
//            IIndex<EFacetType, ICategoryBoundSqlCompiler> compilers,
//            ITypedQueryProxy queryProxy) : base(context, builder)
//        {
//            Compilers = compilers;
//            QueryProxy = queryProxy;
//        }

//        public ITypedQueryProxy QueryProxy { get; }

//        public List<Key2Value<int, float>> Load()
//        {
//            List<string> sqls = new List<string>();
//            foreach (Facet facet in Registry.Facets.GetOfType(EFacetType.Range)) {
//                QuerySetup query = QuerySetupBuilder.Build(null, facet, ToList(facet.TargetTable.TableOrUdfName), ToList(facet.FacetCode));
//                sqls.Add(Compilers[EFacetType.Range].Compile(query, facet, facet.FacetCode));
//            }
//            string sql = String.Join("\nUNION\n", sqls);
//            List<Key2Value<int, float>> values = QueryProxy.QueryKeyValues2<int, float>(sql).ToList();
//            return values;
//        }
//    }
//}
