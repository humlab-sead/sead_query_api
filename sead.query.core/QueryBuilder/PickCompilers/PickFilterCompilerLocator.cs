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
        public PickFilterCompilerLocator(IIndex<EFacetType, IPickFilterCompiler> pickCompilers)
        {
            PickCompilers = pickCompilers;
        }

        public IIndex<EFacetType, IPickFilterCompiler> PickCompilers { get; }

        public virtual IPickFilterCompiler Locate(EFacetType facetType)
        {
            return PickCompilers[facetType];
        }

    }
}
