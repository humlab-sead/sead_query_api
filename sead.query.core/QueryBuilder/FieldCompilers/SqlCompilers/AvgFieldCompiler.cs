namespace SeadQueryCore
{
    public class AvgFieldCompiler : SqlFieldCompiler
    {
        public AvgFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"AVG({expr}) AS avg_of_{expr}"; }
    }
}