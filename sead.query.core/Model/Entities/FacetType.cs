using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeadQueryCore
{
    /// <summary>
    /// Facet definition ID and name
    /// </summary>
    public class FacetType
    {
        /// <summary>
        /// Facet type id
        /// </summary>
        public EFacetType FacetTypeId { get; set; }

        /// <summary>
        /// Type of facet in plain text
        /// </summary>
        public string FacetTypeName { get; set; }

        /// <summary>
        /// Specifies if facets of type should be reloaded when is target facet
        /// </summary>
        public virtual bool ReloadAsTarget { get; set; }

    }
}
