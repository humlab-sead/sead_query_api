using Newtonsoft.Json;

namespace SeadQueryCore;

/// <summary>
/// Entity model for the facet_template table
/// </summary>
public class FacetTemplate
{
    public int TemplateId { get; set; }

    public int FacetId { get; set; }

    public string AnchorName { get; set; }

    public string SqlTemplate { get; set; }

    [JsonIgnore]
    public virtual Facet Facet { get; set; }
}
