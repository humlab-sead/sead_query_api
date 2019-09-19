using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAccessPostgreSqlProvider;
using Microsoft.Extensions.DependencyInjection;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;

namespace SeadQueryAPI
{
    //public interface IControllerServiceAggregate
    //{
    //    IQueryBuilderSetting Setting { get; set; }
    //    ICacheContainer QueryCache { get; set; }
    //    IRepositoryRegistry UnitOfWork { get; set; }
    //}

    public class DependencyService
    {
        public virtual ISeadQueryCache GetCache(StoreSetting settings)
        {
            try {
                if (settings?.UseRedisCache == true)
                    return new RedisCacheProvider();
            } catch (InvalidOperationException) {
                Console.WriteLine("Failed to connect to Redis!");
            }
            Console.WriteLine("Warning: Using in memory cache provider!");
            return new SimpleMemoryCacheProvider();
        }

        public IFacetsGraph DefaultFacetsGraph(IFacetGraphFactory factory, IRepositoryRegistry registry)
        {
            List<GraphNode> nodes = registry.Nodes.GetAll().ToList();
            List<GraphEdge> edges = registry.Edges.GetAll().ToList();
            List<Facet> facets = registry.Facets.FindThoseWithAlias().ToList();
            var graph = factory.Build(nodes, edges, facets);
            return graph;
        }

        public virtual IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(options.Facet).SingleInstance().ExternallyOwned();

            builder.Register(_ => GetCache(options?.Store)).SingleInstance().ExternallyOwned();
            builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().InstancePerLifetimeScope();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => DefaultFacetsGraph(c.Resolve<IFacetGraphFactory>(), c.Resolve<IRepositoryRegistry>()));

            builder.RegisterType<QuerySetupCompiler>().As<IQuerySetupCompiler>();
            builder.RegisterType<DiscreteBogusPickService>().As<IDiscreteBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(3);

            #region __Count Services__
            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            //builder.RegisterAggregateService<ICategoryCountServiceAggregate>();
            //builder.RegisterType<RangeCategoryCountService>();
            //builder.RegisterType<DiscreteCategoryCountService>();
            #endregion

            builder.RegisterType<ValidPicksSqlQueryCompiler>().As<IValidPicksSqlQueryCompiler>();
            builder.RegisterType<EdgeSqlCompiler>().As<IEdgeSqlCompiler>();
            builder.RegisterType<DiscreteContentSqlQueryBuilder>().As<IDiscreteContentSqlQueryCompiler>();
            builder.RegisterType<DiscreteCategoryCountSqlQueryCompiler>().As<IDiscreteCategoryCountSqlQueryCompiler>();
            builder.RegisterType<RangeCategoryCountSqlQueryCompiler>().As<IRangeCategoryCountSqlQueryCompiler>();
            builder.RegisterType<RangeIntervalSqlQueryCompiler>().As<IRangeIntervalSqlQueryCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();
            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterType<ResultCompiler>().As<IResultCompiler>();

            //builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterType<RangeCategoryBoundSqlQueryCompiler>().Keyed<ICategoryBoundSqlQueryCompiler>(EFacetType.Range);

            #region __Result Services__
            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlQueryCompiler>().Keyed<IResultSqlQueryCompiler>("tabular");
            builder.RegisterType<MapResultSqlQueryCompiler>().Keyed<IResultSqlQueryCompiler>("map");

            #endregion

            /* App Services */

            if (options.Store.UseRedisCache) {
                builder.RegisterType<Services.CachedLoadFacetService>().As<Services.IFacetReconstituteService>();
                builder.RegisterType<Services.CachedLoadResultService>().As<Services.ILoadResultService>();
            } else {
                builder.RegisterType<Services.LoadFacetService>().As<Services.IFacetReconstituteService>();
                builder.RegisterType<Services.LoadResultService>().As<Services.ILoadResultService>();
            }
            if (services != null)
                builder.Populate(services);

            var container = builder.Build();

            return container;
        }
    }
}
