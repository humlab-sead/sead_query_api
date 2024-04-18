namespace SeadQueryCore
{
    public interface IRangeCategoryInfoSqlCompiler : ICategoryInfoSqlCompiler
    {
        string Compile(int interval, int min, int max, int interval_count);
    }
}
