﻿using Autofac;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Plugin.Intersect;
using SeadQueryCore.Plugin.Range;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.Plugin.GeoPolygon;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System;

namespace SQT.Infrastructure
{
    public class DependencyService : Module, IDisposable
    {
        public ISetting Options { get; set; }

        public IFacetContext FacetContext { get; set; } = null;

        public DependencyService(IFacetContext facetContext)
        {
            FacetContext = facetContext;
        }
        public void Dispose()
        {
            //MockService.Dispose();
        }

        public virtual ISeadQueryCache GetCache(StoreSetting settings)
        {
            return new NullCache();
        }

        protected override void Load(ContainerBuilder builder)
        {
            Options ??= new SettingFactory().Create().Value;

            builder.RegisterInstance(Options).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(Options.Facet).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<StoreSetting>(Options.Store).SingleInstance().ExternallyOwned();

            if (FacetContext is null)
            {
                Debug.Print("Warning: Falling back to default online DB connection for test");

                // builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance().ExternallyOwned();

                builder.RegisterType<FacetContextFactory>()
                    .As<IFacetContextFactory>()
                    .InstancePerLifetimeScope();

                builder.Register(c => c.Resolve<IFacetContextFactory>().GetInstance())
                    .As<IFacetContext>()
                    .InstancePerLifetimeScope();
            }
            else
            {
                builder.RegisterInstance(FacetContext).SingleInstance().ExternallyOwned();
            }

            builder.Register(c => c.Resolve<IFacetContext>().TypedQueryProxy)
                .As<ITypedQueryProxy>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContext>().DynamicQueryProxy)
                .As<IDynamicQueryProxy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().SingleInstance().ExternallyOwned();
            builder.RegisterType<PathFinder>().As<IPathFinder>().UsingConstructor(typeof(IDefaultGraphFactory)).InstancePerLifetimeScope();
            builder.RegisterType<DefaultGraphFactory>().As<IDefaultGraphFactory>().InstancePerLifetimeScope();

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<BogusPickService>().As<IBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();
            builder.RegisterType<ResultConfigReconstituteService>().As<IResultConfigReconstituteService>();

            builder.RegisterType<UndefinedPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Unknown);

            DiscreteFacetPlugin.RegisterPlugin(builder);
            GeoPolygonFacetPlugin.RegisterPlugin(builder);
            RangeFacetPlugin.RegisterPlugin(builder);
            IntersectFacetPlugin.RegisterPlugin(builder);

            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();

            builder.RegisterType<PicksFilterCompiler>().As<IPicksFilterCompiler>();

            builder.RegisterType<CategoryCountService>().As<ICategoryCountService>();

            builder.RegisterType<ValidPicksSqCompiler>().As<IValidPicksSqlCompiler>();
            builder.RegisterType<JoinSqlCompiler>().As<IJoinSqlCompiler>();
            builder.RegisterType<JoinsClauseCompiler>().As<IJoinsClauseCompiler>();

            builder.RegisterType<FacetContentService>().As<IFacetContentService>();

            builder.RegisterType<ResultService>().As<IResultService>();

            builder.RegisterType<NullPayloadService>().Keyed<IResultPayloadService>("map");
            builder.RegisterType<NullPayloadService>().Keyed<IResultPayloadService>("tabular");
            builder.RegisterType<ResultPayloadServiceLocator>().As<IResultPayloadServiceLocator>();

            builder.RegisterType<TabularResultSqlCompiler>().Keyed<IResultSqlCompiler>("tabular");
            builder.RegisterType<MapResultSqlCompiler>().Keyed<IResultSqlCompiler>("map");
            builder.RegisterType<ResultSqlCompilerLocator>().As<IResultSqlCompilerLocator>();

            builder.Register(_ => GetCache(Options?.Store)).SingleInstance().ExternallyOwned();
            if (Options.Store.UseRedisCache)
            {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetWithCachingService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultWithCachingService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
            else
            {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
        }

        public static IContainer CreateContainer(IFacetContext facetContext, ISetting options)
        {
            var builder = new ContainerBuilder();
            //var fixture = new JsonFacetContextDataFixture(jsonFolder);
            var dependencyService = new DependencyService(facetContext) { Options = options };
            dependencyService.Load(builder);
            return builder.Build();
        }
    }
}
