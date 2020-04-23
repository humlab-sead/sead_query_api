namespace SeadQueryCore
{
    public class CountFieldCompiler : SqlFieldCompiler
    {
        public CountFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return $"COUNT({expr}) AS count_of_{expr}"; }
    }
}