namespace SeadQueryCore
{
    public class CountFieldCompiler(ResultFieldType fieldType) : SqlFieldCompiler(fieldType)
    {
        public override string Compile(string expr) { return $"COUNT({expr}) AS count_of_{expr}"; }
    }

    public class CountDistinctFieldCompiler(ResultFieldType fieldType) : SqlFieldCompiler(fieldType)
    {
        public override string Compile(string expr) { return $"COUNT(DISTINCT {expr}) AS count_of_{expr}"; }
    }

}
