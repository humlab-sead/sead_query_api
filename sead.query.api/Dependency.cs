﻿using Autofac;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System;

namespace SeadQueryAPI
{
    public class DependencyService : Module
    {
        public ISetting Options { get; set; }

        public virtual ISeadQueryCache GetCache(StoreSetting settings)
        {
            try
            {
                if (settings?.UseRedisCache == true)
                    return new RedisCacheFactory().Create(settings.CacheHost, settings.CachePort);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Failed to connect to Redis!");
            }
            Console.WriteLine("Warning: Using in memory cache provider!");
            return new MemoryCacheFactory().Create();
        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterInstance<ISetting>(Options).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(Options.Facet).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<StoreSetting>(Options.Store).SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetContextFactory>()
                .As<IFacetContextFactory>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContextFactory>().GetInstance())
                .As<IFacetContext>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContext>().TypedQueryProxy)
                .As<ITypedQueryProxy>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContext>().DynamicQueryProxy)
                .As<IDynamicQueryProxy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().InstancePerLifetimeScope();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build()).InstancePerLifetimeScope();

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<BogusPickService>().As<IBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();
            builder.RegisterType<ResultConfigReconstituteService>().As<IResultConfigReconstituteService>();

            builder.RegisterType<RangeOuterBoundExtentService>().As<IRangeOuterBoundExtentService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(3);
            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();
            builder.RegisterType<PicksFilterCompiler>().As<IPicksFilterCompiler>();

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);
            builder.RegisterType<DiscreteCategoryCountService>().As<IDiscreteCategoryCountService>();
            builder.RegisterType<CategoryCountServiceLocator>().As<ICategoryCountServiceLocator>();

            builder.RegisterType<ValidPicksSqCompiler>().As<IValidPicksSqlCompiler>();
            builder.RegisterType<JoinSqlCompiler>().As<IJoinSqlCompiler>();
            builder.RegisterType<JoinsClauseCompiler>().As<IJoinsClauseCompiler>();

            builder.RegisterType<DiscreteContentSqlCompiler>().As<IDiscreteContentSqlCompiler>();
            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().As<IDiscreteCategoryCountQueryCompiler>();
            builder.RegisterType<RangeCategoryCountSqlCompiler>().As<IRangeCategoryCountSqlCompiler>();
            builder.RegisterType<RangeIntervalSqlCompiler>().As<IRangeIntervalSqlCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);
            builder.RegisterType<FacetContentServiceLocator>().As<IFacetContentServiceLocator>();

            builder.RegisterType<RangeCategoryBoundSqlCompiler>().Keyed<ICategoryBoundSqlCompiler>(EFacetType.Range);

            builder.RegisterType<ResultService>().As<IResultService>();

            builder.RegisterType<CategoryCountPayloadService>().Keyed<IResultPayloadService>("map");
            builder.RegisterType<NullPayloadService>().Keyed<IResultPayloadService>("tabular");
            builder.RegisterType<ResultPayloadServiceLocator>().As<IResultPayloadServiceLocator>();

            builder.RegisterType<TabularResultSqlCompiler>().Keyed<IResultSqlCompiler>("tabular");
            builder.RegisterType<MapResultSqlCompiler>().Keyed<IResultSqlCompiler>("map");
            builder.RegisterType<ResultSqlCompilerLocator>().As<IResultSqlCompilerLocator>();

            builder.Register(_ => GetCache(Options?.Store)).SingleInstance().ExternallyOwned();
            if (Options.Store.UseRedisCache)
            {
                builder.RegisterType<Services.CachedLoadFacetService>().As<Services.IFacetContentReconstituteService>();
                builder.RegisterType<Services.CachedLoadResultService>().As<Services.ILoadResultService>();
            }
            else
            {
                builder.RegisterType<Services.LoadFacetService>().As<Services.IFacetContentReconstituteService>();
                builder.RegisterType<Services.LoadResultService>().As<Services.ILoadResultService>();
            }
        }
    }
}
