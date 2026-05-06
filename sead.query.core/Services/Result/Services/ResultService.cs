
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore.Services.Result
{
    public class ResultService : QueryServiceBase, IResultService
    {
        private const string SiteLevelSpecificationKey = "site_level";
        private const string TabularViewTypeId = "tabular";
        private const string CountryResultFieldKey = "country";

        public IResultSqlCompilerLocator SqlCompilerLocator { get; }
        public IResultPayloadServiceLocator PayloadServiceLocator { get; }
        public IDynamicQueryProxy QueryProxy { get; }

        public ResultService(
            IRepositoryRegistry repositoryRegistry,
            IDynamicQueryProxy queryProxy,
            IQuerySetupBuilder builder,
            IResultPayloadServiceLocator payloadServiceLocator,
            IResultSqlCompilerLocator sqlCompilerLocator
        ) : base(repositoryRegistry, builder)
        {
            PayloadServiceLocator = payloadServiceLocator;
            SqlCompilerLocator = sqlCompilerLocator;
            QueryProxy = queryProxy;
        }

        public virtual ResultContentSet Load(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
        {
            var queryFields = GetQueryFields(resultConfig);
            var pathFinder = new PathFinder(Registry.Relations.GetEdges());
            var querySetup = QuerySetupBuilder
                .Build(facetsConfig, resultConfig.Facet, queryFields);

            var compiler = SqlCompilerLocator.Locate(resultConfig.ViewTypeId);
            var sqlQuery = compiler.Compile(querySetup, resultConfig.Facet, queryFields);

            return new TabularResultContentSet(
                resultConfig: resultConfig,
                resultFields: queryFields.GetResultValueFields().ToList(),
                reader: QueryProxy.Query(sqlQuery) /* This is (for now) only call to generic QueryProxy.Query */
            )
            {
                Payload = GetPayload(facetsConfig, resultConfig),
                Query = sqlQuery
            };
        }

        private dynamic GetPayload(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
            => PayloadServiceLocator.Locate(resultConfig.ViewTypeId)
                .GetExtraPayload(facetsConfig, resultConfig.Facet.FacetCode);

        private static List<ResultSpecificationField> GetQueryFields(ResultConfig resultConfig)
        {
            var queryFields = resultConfig.GetSortedFields();
            if (!ShouldIncludeCountryField(resultConfig, queryFields))
                return queryFields;

            queryFields.Add(CreateCountryResultSpecificationField(queryFields));
            return queryFields;
        }

        private static bool ShouldIncludeCountryField(ResultConfig resultConfig, List<ResultSpecificationField> queryFields)
        {
            if (resultConfig?.ViewTypeId != TabularViewTypeId)
                return false;

            if (!(resultConfig.SpecificationKeys?.Contains(SiteLevelSpecificationKey) ?? false))
                return false;

            if (!(queryFields?.Any() ?? false))
                return false;

            if (!queryFields.Any(z => z.ResultField?.TableName == "tbl_sites"))
                return false;

            return !queryFields.Any(z => z.ResultField?.ResultFieldKey == CountryResultFieldKey);
        }

        private static ResultSpecificationField CreateCountryResultSpecificationField(List<ResultSpecificationField> queryFields)
        {
            var singleItemFieldType = queryFields
                .Select(z => z.FieldType)
                .FirstOrDefault(z => z?.FieldTypeId == "single_item")
                ?? throw new System.InvalidOperationException("Unable to locate a single_item result field type.");

            return new ResultSpecificationField
            {
                SpecificationFieldId = 0,
                SpecificationId = queryFields.First().SpecificationId,
                FieldTypeId = singleItemFieldType.FieldTypeId,
                SequenceId = queryFields.Max(z => z.SequenceId) + 1,
                FieldType = singleItemFieldType,
                ResultField = new ResultField
                {
                    ResultFieldId = 0,
                    ResultFieldKey = CountryResultFieldKey,
                    TableName = null,
                    ColumnName = "site_country.country_name",
                    DisplayText = "Country",
                    FieldTypeId = singleItemFieldType.FieldTypeId,
                    Activated = true,
                    DataType = "text",
                    FieldType = singleItemFieldType
                }
            };
        }
    }
}
