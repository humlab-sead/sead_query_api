using System.Collections.Generic;

namespace SeadQueryComposer.Plugins.DiscretePlugin;

public class DiscreteFacetUserInput
{
    public List<object> Picks { get; set; } = new();
    public string Operator { get; set; } = "in";
    public bool HasPicks => Picks.Any();
}