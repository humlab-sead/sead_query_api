namespace SeadQueryCore
{
    public class SumFieldCompiler : SqlFieldCompiler
    {
        public SumFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr)
        {
            return $"SUM({expr}.double precision) AS sum_of_{expr}";
        }
    }
}