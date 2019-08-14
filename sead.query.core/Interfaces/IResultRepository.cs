using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IResultRepository : IRepository<ResultAggregate, int>
    {
        IEnumerable<ResultField> GetAllFields();
        IEnumerable<ResultFieldType> GetAllFieldTypes();
        ResultAggregate GetByKey(string key);
        List<ResultAggregate> GetByKeys(List<string> keys);
        List<ResultAggregateField> GetFieldsByKeys(List<string> keys);
        ResultViewType GetViewType(string viewTypeId);
        List<ResultViewType> GetViewTypes();
        Dictionary<string, ResultAggregate> ToDictionary();
    }
}