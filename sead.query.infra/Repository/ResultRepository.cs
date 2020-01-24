using SeadQueryInfra.DataAccessProvider;
using SeadQueryCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SeadQueryInfra {

    public class ResultRepository : Repository<ResultAggregate, int>, IResultRepository
    {
        public ResultRepository(IFacetContext context) : base(context)
        {
        }

        public Dictionary<string, ResultAggregate> ToDictionary()
        {
            return GetAll().ToDictionary(x => x.AggregateKey);
        }

        public override IEnumerable<ResultAggregate> GetAll()
        {
            return Context.Set<ResultAggregate>().BuildEntity().ToList();
        }

        public ResultAggregate GetByKey(string key)
        {
            return GetAll().FirstOrDefault(x => x.AggregateKey.Equals(key));
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

        public List<ResultAggregate> GetByKeys(List<string> keys)
        {
            return (keys ?? new List<string>()).Select(key => GetByKey(key)).ToList();
        }

        public List<ResultAggregateField> GetFieldsByKeys(List<string> keys)
        {
            return GetByKeys(keys).SelectMany(x => x.GetFields()).ToList();
        }
    }

    public static class ResultDefinitionRepositoryEagerBuilder {
        public static IQueryable<ResultAggregate> BuildEntity(this IQueryable<ResultAggregate> query)
        {
            return query.Include(x => x.Fields)
                        .Include("Fields.FieldType")
                        .Include("Fields.ResultField");
        }
    }
}
