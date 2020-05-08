using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryInfra
{

    public class ResultRepository : Repository<ResultComposite, int>, IResultRepository
    {
        public ResultRepository(IFacetContext context) : base(context)
        {
        }

        public Dictionary<string, ResultComposite> ToDictionary()
        {
            return GetAll().ToDictionary(x => x.CompositeKey);
        }

        public override IEnumerable<ResultComposite> GetAll()
        {
            return Context.Set<ResultComposite>().BuildEntity().ToList();
        }

        public ResultComposite GetByKey(string key)
        {
            return GetAll().FirstOrDefault(x => x.CompositeKey.Equals(key));
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

        public List<ResultComposite> GetByKeys(List<string> keys)
        {
            return (keys ?? new List<string>()).Select(key => GetByKey(key)).ToList();
        }

        public List<ResultCompositeField> GetFieldsByKeys(List<string> keys)
        {
            return GetByKeys(keys).SelectMany(x => x.GetSortedFields()).ToList();
        }
        public List<ResultCompositeField> GetFieldsByKey(string key)
        {
            return GetByKey(key).GetSortedFields().ToList();
        }
    }

    public static class ResultDefinitionRepositoryEagerBuilder {
        public static IQueryable<ResultComposite> BuildEntity(this IQueryable<ResultComposite> query)
        {
            return query.Include(x => x.Fields)
                        .Include("Fields.FieldType")
                        .Include("Fields.ResultField");
        }
    }
}
