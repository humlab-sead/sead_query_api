using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Fixtures;
using SeadQueryTest.Infrastructure;
using System.IO;
using System.Linq;

namespace SeadQueryTest.Mocks
{
    public interface IFakeFacetContextSeeder
    {
        IFacetContext Seed(FacetContext context);
    }

     public class FakeFacetContextJsonSeeder : IFakeFacetContextSeeder
     {
        public string Folder { get; }

        public FakeFacetContextJsonSeeder()
        {
            Folder = JsonService.DataFolder();
        }

        public IFacetContext Seed(FacetContext context)
        {

            var reader = new JsonReaderService();

            context.AttachRange(reader.Deserialize<Table>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<TableRelation>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<FacetGroup>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<FacetType>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<Facet>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<FacetClause>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<FacetTable>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ViewState>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ResultViewType>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ResultFieldType>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ResultField>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ResultAggregate>(Folder).ToArray());
            context.AttachRange(reader.Deserialize<ResultAggregateField>(Folder).ToArray());

            return context;
        }
    }


}
