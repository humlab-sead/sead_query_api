namespace SeadQueryCore;

// NOT USED.
// Note: Bogus picks are only removed for discrete facets
public class NullBugusPickService : IBogusPickService
{
    public FacetsConfig2 Update(FacetsConfig2 facetsConfig)
    {
        return facetsConfig;
    }
}
