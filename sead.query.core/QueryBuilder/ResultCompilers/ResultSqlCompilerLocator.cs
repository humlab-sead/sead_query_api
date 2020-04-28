using Autofac.Features.Indexed;

namespace SeadQueryCore
{
    public class ResultSqlCompilerLocator : IResultSqlCompilerLocator
    {
        public ResultSqlCompilerLocator(IIndex<string, IResultSqlCompiler> compilers)
        {
            Compilers = compilers;
        }

        public IIndex<string, IResultSqlCompiler> Compilers { get; }

        public virtual IResultSqlCompiler Locate(string viewTypeId)
        {
            return Compilers[viewTypeId];
        }

    }
}