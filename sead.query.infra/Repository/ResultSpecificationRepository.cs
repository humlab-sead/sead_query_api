using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryInfra
{

    public class ResultSpecificationRepository : Repository<ResultSpecification, int>, IResultSpecificationRepository
    {
        public ResultSpecificationRepository(IFacetContext context) : base(context)
        {
        }

        public Dictionary<string, ResultSpecification> ToDictionary()
        {
            return GetAll().ToDictionary(x => x.SpecificationKey);
        }

        public override IEnumerable<ResultSpecification> GetAll()
        {
            return Context.Set<ResultSpecification>().BuildEntity().ToList();
        }

        public ResultSpecification GetByKey(string key)
        {
            return GetAll().FirstOrDefault(x => x.SpecificationKey.Equals(key));
        }

        public IEnumerable<ResultField> GetAllFields()
        {
            return Context.Set<ResultField>().ToList();
        }

        public List<ResultViewType> GetViewTypes()
        {
            return Context.Set<ResultViewType>().ToList();
        }

        public ResultViewType GetViewType(string viewTypeId)
        {
            return GetViewTypes().FirstOrDefault(z => z.ViewTypeId == viewTypeId);
        }

        public IEnumerable<ResultFieldType> GetAllFieldTypes()
        {
            return Context.Set<ResultFieldType>().ToList();
        }

        public List<ResultSpecification> GetByKeys(List<string> keys)
        {
            return (keys ?? new List<string>()).Select(key => GetByKey(key)).ToList();
        }

        public List<ResultSpecificationField> GetFieldsByKeys(List<string> keys)
        {
            return GetByKeys(keys).SelectMany(x => x.GetSortedFields()).ToList();
        }
        public List<ResultSpecificationField> GetFieldsByKey(string key)
        {
            return GetByKey(key).GetSortedFields().ToList();
        }
    }

    public static class ResultDefinitionRepositoryEagerBuilder {
        public static IQueryable<ResultSpecification> BuildEntity(this IQueryable<ResultSpecification> query)
        {
            return query.Include(x => x.Fields)
                        .Include("Fields.FieldType")
                        .Include("Fields.ResultField");
        }
    }
}
