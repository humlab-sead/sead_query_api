
namespace SeadQueryCore
{
    public class DefaultFieldCompiler : SqlFieldCompiler
    {
        public DefaultFieldCompiler(ResultFieldType fieldType) : base(fieldType) { }
        public override string Compile(string expr) { return expr; }
    }
}