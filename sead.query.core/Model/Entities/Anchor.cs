using Newtonsoft.Json;

namespace SeadQueryCore;

/// <summary>
/// A relational table associated to a facet
/// </summary>
public class Anchor
{
    public int AnchorId { get; set; }
    public int TableId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    public virtual Table Table { get; set; }
}
