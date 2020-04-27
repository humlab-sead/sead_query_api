namespace SeadQueryCore
{
    public interface IJoinSqlCompiler : ISqlCompiler
    {
        string Compile(TableRelation edge, FacetTable facetTable, bool innerJoin = false);
    }
}