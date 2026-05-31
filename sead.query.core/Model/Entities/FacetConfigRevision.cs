using System;

namespace SeadQueryCore;

public class FacetConfigRevision
{
    public int RevisionId { get; set; }

    public string ConfigRevision { get; set; }

    public string SourceCommit { get; set; }

    public string ContentHash { get; set; }

    public DateTime ImportedAt { get; set; }

    public string ImportedBy { get; set; }

    public bool IsActive { get; set; }
}
