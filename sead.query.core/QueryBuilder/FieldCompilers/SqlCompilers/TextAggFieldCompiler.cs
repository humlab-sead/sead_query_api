namespace SeadQueryCore
{
    public class TextAggFieldCompiler : SqlFieldCompiler
    {
        public TextAggFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr)
        {
            return $"array_to_string(array_agg(DISTINCT {expr}),',') AS text_agg_of_{expr}";
        }
    }
}