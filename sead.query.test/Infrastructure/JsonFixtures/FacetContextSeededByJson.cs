using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using SeadQueryTest.Infrastructure.Scaffolding;
using System.Collections.Generic;

namespace SeadQueryTest.Infrastructure
{
    public class FacetContextSeededByJson : FacetContextFixture
    {
        public FacetContextSeededByJson(string folder) : base(folder)
        {
        }

        protected override void Seed()
        {
            var path = DataFolder;
            var reader = new JsonReaderService();

            List<FacetGroup> facetGroups = new List<FacetGroup>(reader.Deserialize<FacetGroup>(path));
            FacetContext.AddRange(facetGroups);
            FacetContext.AddRange(reader.Deserialize<FacetType>(path));
            FacetContext.AddRange(reader.Deserialize<FacetClause>(path));
            FacetContext.AddRange(reader.Deserialize<FacetTable>(path));
            FacetContext.AddRange(reader.Deserialize<Facet>(path));
            FacetContext.AddRange(reader.Deserialize<ViewState>(path));
            FacetContext.AddRange(reader.Deserialize<Table>(path));
            FacetContext.AddRange(reader.Deserialize<TableRelation>(path));
            FacetContext.AddRange(reader.Deserialize<ResultViewType>(path));
            FacetContext.AddRange(reader.Deserialize<ResultFieldType>(path));
            FacetContext.AddRange(reader.Deserialize<ResultAggregateField>(path));
            FacetContext.AddRange(reader.Deserialize<ResultField>(path));
            FacetContext.AddRange(reader.Deserialize<ResultAggregate>(path));

            FacetContext.SaveChanges();

        }

    }
}
