using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeadQueryCore;

/// <summary>
/// A relational table associated to a facet
/// </summary>
public class Route
{
    public int RouteId { get; set; }
    public string Name { get; set; }

    public int SourceTableId { get; set; }
    public int TargetTableId { get; set; }
    public string Specification { get; set; }
    public virtual string Alias { get; set; }

    [JsonIgnore]
    public virtual Table SourceTable { get; set; }

    [JsonIgnore]
    public virtual Table TargetTable { get; set; }

    [JsonIgnore]
    public virtual List<RouteStep> Steps { get; set; } = null;
}

public class RouteStep
{
    public int RouteStepId { get; set; }

    /// <summary>
    /// Parent route identifier
    /// </summary>
    public int RouteId { get; set; }

    /// <summary>
    /// Step sequence number
    /// </summary>
    public int SequenceId { get; set; }

    /// <summary>
    /// Table identifier
    /// </summary>
    public int TableId { get; set; }

    [JsonIgnore]
    public virtual Route Route { get; set; }

    [JsonIgnore]
    public virtual Table Table { get; set; }
}
