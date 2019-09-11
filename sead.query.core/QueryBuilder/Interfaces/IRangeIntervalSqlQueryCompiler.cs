namespace SeadQueryCore
{
    public interface IRangeIntervalSqlQueryCompiler
    {
        string Compile(int interval, int min, int max, int interval_count);
    }
}