using Autofac.Features.Indexed;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeadQueryCore.QueryBuilder
{
    public interface IPickFilterCompilerLocator
    {
        IPickFilterCompiler Locate(EFacetType facetType);
    }

    public class PickFilterCompilerLocator : IPickFilterCompilerLocator
    {
        public PickFilterCompilerLocator(IIndex<int, IPickFilterCompiler> pickCompilers)
        {
            PickCompilers = pickCompilers;
        }

        public IIndex<int, IPickFilterCompiler> PickCompilers { get; }

        public IPickFilterCompiler Locate(EFacetType facetType)
        {
            return PickCompilers[(int)facetType];
        }

    }
}
