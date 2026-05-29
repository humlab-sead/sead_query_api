using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using Xunit;

namespace SQT.UnitTests.QueryComposer.RouteCompiler;

public class RouteSqlCompilerTests
{
    [Fact]
    public void Compile_WithNullTables_ThrowsArgumentNullException()
    {
        var repository = new Mock<IRouteRepository>();
        var routeResolver = new Mock<IRouteResolver>();
        var compiler = new RouteSqlCompiler(repository.Object, routeResolver.Object);

        Action act = () => compiler.Compile(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("tables");
    }

    [Fact]
    public void Compile_WithMultiHopRoute_SelectsTargetKeyFromFinalAlias()
    {
        var repository = new Mock<IRouteRepository>();
        var routeResolver = new Mock<IRouteResolver>();
        var compiler = new RouteSqlCompiler(repository.Object, routeResolver.Object);

        var sites = CreateTable(1, "tbl_sites", "site_id");
        var sampleGroups = CreateTable(2, "tbl_sample_groups", "sample_group_id");
        var samples = CreateTable(3, "tbl_physical_samples", "physical_sample_id");
        var analysisEntities = CreateTable(4, "tbl_analysis_entities", "analysis_entity_id");

        routeResolver
            .Setup(x => x.Resolve(It.IsAny<IEnumerable<string>>()))
            .Returns(
                [
                    CreateRelation(1, sites, sampleGroups, "site_id", "site_id"),
                    CreateRelation(2, sampleGroups, samples, "sample_group_id", "sample_group_id"),
                    CreateRelation(3, samples, analysisEntities, "physical_sample_id", "physical_sample_id"),
                ]
            );

        var sql = compiler.Compile(["tbl_sites", "tbl_sample_groups", "tbl_physical_samples", "tbl_analysis_entities"]);

        sql.Should().Contain("select distinct X_0.site_id as source_id, X_3.analysis_entity_id as target_id");
        sql.Should().Contain("join tbl_analysis_entities as X_3 on X_3.physical_sample_id = X_2.physical_sample_id");
        sql.Should().NotContain("X_2.analysis_entity_id");
    }

    [Fact]
    public void Compile_WithTargetKeyOverride_SelectsOverriddenTargetColumn()
    {
        var repository = new Mock<IRouteRepository>();
        var routeResolver = new Mock<IRouteResolver>();
        var compiler = new RouteSqlCompiler(repository.Object, routeResolver.Object);

        var sites = CreateTable(1, "tbl_sites", "site_id");
        var countryShortcut = CreateTable(2, "facet.site_location_shortcut", "xxxx");

        routeResolver
            .Setup(x => x.Resolve(It.IsAny<IEnumerable<string>>()))
            .Returns([CreateRelation(1, sites, countryShortcut, "site_id", "site_id")]);

        var sql = compiler.Compile(["tbl_sites", "facet.site_location_shortcut"], "location_id");

        sql.Should().Contain("select distinct X_0.site_id as source_id, X_1.location_id as target_id");
        sql.Should().NotContain("X_1.xxxx as target_id");
    }

    [Fact]
    public void Compile_WithSourceAndTargetKeyOverrides_SelectsOverriddenBoundaryColumns()
    {
        var repository = new Mock<IRouteRepository>();
        var routeResolver = new Mock<IRouteResolver>();
        var compiler = new RouteSqlCompiler(repository.Object, routeResolver.Object);

        var countryShortcut = CreateTable(1, "facet.site_location_shortcut", "xxxx");
        var sites = CreateTable(2, "tbl_sites", "site_id");

        routeResolver
            .Setup(x => x.Resolve(It.IsAny<IEnumerable<string>>()))
            .Returns([CreateRelation(1, countryShortcut, sites, "site_id", "site_id")]);

        var sql = compiler.Compile(["facet.site_location_shortcut", "tbl_sites"], "location_id", "site_id");

        sql.Should().Contain("select distinct X_0.location_id as source_id, X_1.site_id as target_id");
        sql.Should().NotContain("X_0.xxxx as source_id");
    }

    private static Table CreateTable(int id, string name, string primaryKey)
    {
        return new Table
        {
            TableId = id,
            TableOrUdfName = name,
            PrimaryKeyName = primaryKey,
            IsUdf = false,
        };
    }

    private static TableRelation CreateRelation(int id, Table source, Table target, string sourceColumn, string targetColumn)
    {
        return new TableRelation
        {
            TableRelationId = id,
            SourceTable = source,
            TargetTable = target,
            SourceColumnName = sourceColumn,
            TargetColumnName = targetColumn,
        };
    }
}
