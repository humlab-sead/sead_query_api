using System.Collections.Generic;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Model;

public class FacetTemplate
{
    public string FacetCode { get; set; } = string.Empty;
    public string TargetTable { get; set; } = string.Empty;
    public string TargetId { get; set; } = string.Empty;
    public EFacetType Type { get; set; }
    public Dictionary<string, AnchorTemplate> Anchors { get; set; } = new();
}
