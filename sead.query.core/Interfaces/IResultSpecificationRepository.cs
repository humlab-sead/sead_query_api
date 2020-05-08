using System.Collections.Generic;

namespace SeadQueryCore
{
    public interface IResultSpecificationRepository : IRepository<ResultSpecification, int>
    {
        IEnumerable<ResultField> GetAllFields();
        IEnumerable<ResultFieldType> GetAllFieldTypes();
        ResultSpecification GetByKey(string key);
        List<ResultSpecification> GetByKeys(List<string> keys);
        List<ResultSpecificationField> GetFieldsByKeys(List<string> keys);
        List<ResultSpecificationField> GetFieldsByKey(string key);
        ResultViewType GetViewType(string viewTypeId);
        List<ResultViewType> GetViewTypes();
        Dictionary<string, ResultSpecification> ToDictionary();
    }
}