using Newtonsoft.Json;

namespace SeadQueryCore;

/// <summary>
/// A relational table associated to a facet
/// </summary>
public class FacetAnchor
{
    public int FacetAnchorId { get; set; }
    public int FacetId { get; set; }
    public int AnchorId { get; set; }
    public int RouteId { get; set; }

    [JsonIgnore]
    public virtual Facet Facet { get; set; } = null!;

    [JsonIgnore]
    public virtual Route Route { get; set; } = null!;

    [JsonIgnore]
    public virtual Anchor Anchor { get; set; } = null!;
}
