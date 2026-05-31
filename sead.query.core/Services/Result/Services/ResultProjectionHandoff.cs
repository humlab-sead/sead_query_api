using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Services.Result
{
    public sealed class ResultProjectionHandoff
    {
        public ResultProjectionHandoff(QuerySetup querySetup, IEnumerable<ResultSpecificationField> resultFields)
        {
            ArgumentNullException.ThrowIfNull(querySetup);
            ArgumentNullException.ThrowIfNull(resultFields);

            QuerySetup = querySetup;
            ResultFields = resultFields.ToList();
        }

        public QuerySetup QuerySetup { get; }
        public IReadOnlyList<ResultSpecificationField> ResultFields { get; }
    }
}
