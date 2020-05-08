using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IResultRepository : IRepository<ResultComposite, int>
    {
        IEnumerable<ResultField> GetAllFields();
        IEnumerable<ResultFieldType> GetAllFieldTypes();
        ResultComposite GetByKey(string key);
        List<ResultComposite> GetByKeys(List<string> keys);
        List<ResultCompositeField> GetFieldsByKeys(List<string> keys);
        List<ResultCompositeField> GetFieldsByKey(string key);
        ResultViewType GetViewType(string viewTypeId);
        List<ResultViewType> GetViewTypes();
        Dictionary<string, ResultComposite> ToDictionary();
    }
}