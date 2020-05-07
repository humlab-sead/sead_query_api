using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SeadQueryCore;

namespace SeadQueryAPI.Serializers
{
    public class ReconstituteService
    {
        protected readonly IRepositoryRegistry Registry;
        protected readonly JsonSerializerSettings SerializerSettings;

        public ReconstituteService(IRepositoryRegistry registry)
        {
            Registry = registry;
            SerializerSettings = new JsonSerializerSettings {
                Error = (sender, errorArgs) =>
                {
                    var currentError = errorArgs.ErrorContext.Error.Message;
                    errorArgs.ErrorContext.Handled = true;
                }
            };
        }

        protected Facet GetFacetByCode(string facetCode)
        {
            if (String.IsNullOrEmpty(facetCode))
                return null;

            return Registry.Facets.GetByCode(facetCode);
        }

    }
}
