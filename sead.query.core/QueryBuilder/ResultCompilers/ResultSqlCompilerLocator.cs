using Autofac.Features.Indexed;

namespace SeadQueryCore
{
    public class ResultSqlCompilerLocator : IResultSqlCompilerLocator
    {
        public ResultSqlCompilerLocator(IIndex<int, IResultSqlCompiler> compilers)
        {
            Compilers = Compilers;
        }

        public IIndex<string, IResultSqlCompiler> Compilers { get; }

        public virtual IResultSqlCompiler Locate(string viewTypeId)
        {
            return Compilers[viewTypeId];
        }

    }
}