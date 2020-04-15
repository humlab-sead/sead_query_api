
namespace SeadQueryCore
{
    public class SqlFieldCompiler : ISqlFieldCompiler
    {
        public ResultFieldType FieldType { get; private set; }
        public SqlFieldCompiler(ResultFieldType fieldType)
        {
            FieldType = fieldType;
        }
        public virtual string Compile(string expr) {
            return expr;
        }
    }
}