using System.Collections.Generic;

namespace SeadQueryComposer.QueryComposer.RouteCompiler;

public class AnchorTemplate
{
    /// <summary>
    /// Direct SQL override - takes precedence over route
    /// </summary>
    public string? ExplicitSql { get; set; }

    /// <summary>
    /// Unified route specification supporting:
    /// - Table names: "tbl_sites"
    /// - Global route names: "site_to_dataset"
    /// - Encoded chains: "tbl_sites -> tbl_sample_groups -> tbl_physical_samples"
    /// - Mixed sequences: ["tbl_sites", "site_to_sample", "tbl_analysis_entities"]
    /// </summary>
    public List<string> Route { get; set; } = new();

    public bool RequiresDistinct { get; set; } = false;
    public string? Description { get; set; }
}
