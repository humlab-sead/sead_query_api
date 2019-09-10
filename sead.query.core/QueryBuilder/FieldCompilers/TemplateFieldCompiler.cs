namespace SeadQueryCore
{
    public class TemplateFieldCompiler : SqlFieldCompiler
    {
        public TemplateFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }

        public override string Compile(string expr)
        {
            return string.Format(FieldType.SqlTemplate, expr);
        }
    }
}