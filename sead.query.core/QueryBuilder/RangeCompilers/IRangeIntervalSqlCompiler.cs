namespace SeadQueryCore
{
    public interface IRangeIntervalSqlCompiler
    {
        string Compile(int interval, int min, int max, int interval_count);
    }
}