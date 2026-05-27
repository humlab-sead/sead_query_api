using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using Xunit;

namespace SQT.UnitTests.QueryComposer.RouteCompiler;

/// <summary>
/// Comprehensive unit tests for RouteGraph and RouteGraphFactory classes.
/// Tests cover graph construction, route resolution, table name resolution,
/// bidirectional relationships, and error handling scenarios.
/// </summary>
public class RouteGraphTests
{
    #region Test Data Helpers

    /// <summary>
    /// Creates a test Table entity with the specified properties
    /// </summary>
    private static Table CreateTable(int id, string name, string primaryKey = "id")
    {
        return new Table
        {
            TableId = id,
            TableOrUdfName = name,
            PrimaryKeyName = primaryKey,
            IsUdf = false,
        };
    }

    /// <summary>
    /// Creates a test TableRelation entity between two tables
    /// </summary>
    private static TableRelation CreateRelation(
        int id,
        Table source,
        Table target,
        string sourceColumn = "id",
        string targetColumn = "fk_id",
        int weight = 1
    )
    {
        return new TableRelation
        {
            TableRelationId = id,
            SourceTable = source,
            TargetTable = target,
            SourceColumnName = sourceColumn,
            TargetColumnName = targetColumn,
            Weight = weight,
        };
    }

    /// <summary>
    /// Creates a basic test graph with sites -> samples -> measurements chain
    /// </summary>
    private static List<TableRelation> CreateBasicTestEdges()
    {
        var sites = CreateTable(1, "sites");
        var samples = CreateTable(2, "samples");
        var measurements = CreateTable(3, "measurements");

        return
        [
            CreateRelation(1, sites, samples, "site_id", "site_id"),
            CreateRelation(2, samples, measurements, "sample_id", "sample_id"),
        ];
    }

    /// <summary>
    /// Creates a complex test graph with multiple relationships and naming variations
    /// </summary>
    private static List<TableRelation> CreateComplexTestEdges()
    {
        var sites = CreateTable(1, "sites");
        var samples = CreateTable(2, "samples");
        var measurements = CreateTable(3, "measurements");
        var tblCountries = CreateTable(4, "tbl_countries");
        var tblDatingMethods = CreateTable(5, "tbl_dating_methods");

        return new List<TableRelation>
        {
            CreateRelation(1, sites, samples, "site_id", "site_id"),
            CreateRelation(2, samples, measurements, "sample_id", "sample_id"),
            CreateRelation(3, sites, tblCountries, "country_id", "country_id"),
            CreateRelation(4, samples, tblDatingMethods, "dating_method_id", "method_id"),
        };
    }

    #endregion

    #region RouteGraph Constructor Tests

    [Fact]
    public void Constructor_WithNullEdges_ThrowsArgumentNullException()
    {
        // Act & Assert
        Action act = () => new RouteResolver(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithEmptyEdges_CreatesEmptyGraph()
    {
        // Arrange
        var edges = new List<TableRelation>();

        // Act
        var graph = new RouteResolver(edges);

        // Assert
        graph.Relations.Should().BeEmpty();
        graph.Nodes.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithBidirectionalTrue_CreatesReversedRelationships()
    {
        // Arrange
        var edges = CreateBasicTestEdges();

        // Act
        var graph = new RouteResolver(edges, bidirectional: true);

        // Assert
        graph.Relations.Should().HaveCount(4); // 2 original + 2 reversed
        graph.Relations.Should().ContainKey(("sites", "samples"));
        graph.Relations.Should().ContainKey(("samples", "sites")); // Reversed
        graph.Relations.Should().ContainKey(("samples", "measurements"));
        graph.Relations.Should().ContainKey(("measurements", "samples")); // Reversed
    }

    [Fact]
    public void Constructor_WithBidirectionalFalse_DoesNotCreateReversedRelationships()
    {
        // Arrange
        var edges = CreateBasicTestEdges();

        // Act
        var graph = new RouteResolver(edges, bidirectional: false);

        // Assert
        graph.Relations.Should().HaveCount(2); // Only original edges
        graph.Relations.Should().ContainKey(("sites", "samples"));
        graph.Relations.Should().ContainKey(("samples", "measurements"));
        graph.Relations.Should().NotContainKey(("samples", "sites"));
        graph.Relations.Should().NotContainKey(("measurements", "samples"));
    }

    [Fact]
    public void Constructor_PopulatesNodesCorrectly()
    {
        // Arrange
        var edges = CreateBasicTestEdges();

        // Act
        var graph = new RouteResolver(edges);

        // Assert
        graph.Nodes.Should().HaveCount(3);
        graph.Nodes.Should().ContainKey("sites");
        graph.Nodes.Should().ContainKey("samples");
        graph.Nodes.Should().ContainKey("measurements");
        graph.Nodes["sites"].TableId.Should().Be(1);
        graph.Nodes["samples"].TableId.Should().Be(2);
        graph.Nodes["measurements"].TableId.Should().Be(3);
    }

    #endregion

    #region GetRoute Method Tests

    [Fact]
    public void GetRoute_WithSingleTable_ReturnsEmptyRoute()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(new[] { "sites" });

        // Assert
        route.Should().BeEmpty();
    }

    [Fact]
    public void GetRoute_WithTwoTables_ReturnsSingleRelation()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(new[] { "sites", "samples" });

        // Assert
        route.Should().HaveCount(1);
        route[0].SourceTable.TableOrUdfName.Should().Be("sites");
        route[0].TargetTable.TableOrUdfName.Should().Be("samples");
    }

    [Fact]
    public void GetRoute_WithThreeTables_ReturnsTwoRelations()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(new[] { "sites", "samples", "measurements" });

        // Assert
        route.Should().HaveCount(2);
        route[0].SourceTable.TableOrUdfName.Should().Be("sites");
        route[0].TargetTable.TableOrUdfName.Should().Be("samples");
        route[1].SourceTable.TableOrUdfName.Should().Be("samples");
        route[1].TargetTable.TableOrUdfName.Should().Be("measurements");
    }

    [Fact]
    public void GetRoute_WithReversedDirection_WorksWithBidirectionalGraph()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges, bidirectional: true);

        // Act
        var route = graph.Resolve(new[] { "measurements", "samples", "sites" });

        // Assert
        route.Should().HaveCount(2);
        route[0].SourceTable.TableOrUdfName.Should().Be("measurements");
        route[0].TargetTable.TableOrUdfName.Should().Be("samples");
        route[1].SourceTable.TableOrUdfName.Should().Be("samples");
        route[1].TargetTable.TableOrUdfName.Should().Be("sites");
    }

    [Fact]
    public void GetRoute_WithNonExistentRelation_ThrowsInvalidOperationException()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act & Assert
        Action act = () => graph.Resolve(new[] { "sites", "measurements" }); // No direct relation
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*No relation found between 'sites' (sites) and 'measurements' (measurements)*");
    }

    [Fact]
    public void GetRoute_WithUnknownTable_ThrowsKeyNotFoundException()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act & Assert
        Action act = () => graph.Resolve(new[] { "unknown_table", "sites" });
        act.Should().Throw<KeyNotFoundException>().WithMessage("*Table 'unknown_table' not found in graph*");
    }

    #endregion

    #region Table Name Resolution Tests

    [Fact]
    public void GetRoute_WithExactTableName_ResolvesCorrectly()
    {
        // Arrange
        var edges = CreateComplexTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(new[] { "sites", "tbl_countries" });

        // Assert
        route.Should().HaveCount(1);
        route[0].TargetTable.TableOrUdfName.Should().Be("tbl_countries");
    }

    [Fact]
    public void GetRoute_WithTableNameVariations_ResolvesFlexibly()
    {
        // Arrange - Create table with "tbl_" prefix
        var tblSites = CreateTable(1, "tbl_sites");
        var samples = CreateTable(2, "samples");
        var edges = new List<TableRelation> { CreateRelation(1, tblSites, samples, "site_id", "site_id") };
        var graph = new RouteResolver(edges);

        // Act - Use name without "tbl_" prefix
        var route = graph.Resolve(new[] { "sites", "samples" });

        // Assert
        route.Should().HaveCount(1);
        route[0].SourceTable.TableOrUdfName.Should().Be("tbl_sites");
    }

    [Fact]
    public void GetRoute_WithPluralizedTableName_ResolvesCorrectly()
    {
        // Arrange - Create table with "tbl_" prefix and singular form
        var tblSite = CreateTable(1, "tbl_sites"); // Note: plural in actual table name
        var samples = CreateTable(2, "samples");
        var edges = new List<TableRelation> { CreateRelation(1, tblSite, samples, "site_id", "site_id") };
        var graph = new RouteResolver(edges);

        // Act - Use singular form without prefix
        var route = graph.Resolve(new[] { "site", "samples" }); // "site" should resolve to "tbl_sites"

        // Assert
        route.Should().HaveCount(1);
        route[0].SourceTable.TableOrUdfName.Should().Be("tbl_sites");
    }

    [Fact]
    public void GetRoute_WithComplexNamingVariations_ResolvesAllCorrectly()
    {
        // Arrange
        var sites = CreateTable(1, "sites"); // Exact match
        var tblCountries = CreateTable(2, "tbl_countries"); // Requires prefix resolution
        var tblSamples = CreateTable(3, "tbl_samples"); // Requires prefix + plural resolution

        var edges = new List<TableRelation>
        {
            CreateRelation(1, sites, tblCountries, "country_id", "country_id"),
            CreateRelation(2, tblCountries, tblSamples, "id", "country_id"),
        };
        var graph = new RouteResolver(edges);

        // Act - Use various naming conventions
        var route = graph.Resolve(new[] { "sites", "countries", "sample" });

        // Assert
        route.Should().HaveCount(2);
        route[0].SourceTable.TableOrUdfName.Should().Be("sites");
        route[0].TargetTable.TableOrUdfName.Should().Be("tbl_countries");
        route[1].SourceTable.TableOrUdfName.Should().Be("tbl_countries");
        route[1].TargetTable.TableOrUdfName.Should().Be("tbl_samples");
    }

    #endregion

    #region Edge Cases and Error Scenarios

    [Fact]
    public void GetRoute_WithEmptyTableSequence_ReturnsEmptyRoute()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(Array.Empty<string>());

        // Assert
        route.Should().BeEmpty();
    }

    [Fact]
    public void GetRoute_WithTableSequenceFromLinq_WorksCorrectly()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var tableNames = new[] { "sites", "samples", "measurements" };
        var route = graph.Resolve(tableNames.Where(t => t.Contains("s")));

        // Assert
        route.Should().HaveCount(2);
    }

    [Fact]
    public void GetRoute_WithDuplicateTablesInSequence_HandlesCorrectly()
    {
        // Arrange
        var edges = CreateBasicTestEdges();
        var graph = new RouteResolver(edges);

        // Act
        var route = graph.Resolve(new[] { "sites", "samples", "samples", "measurements" });

        // Assert
        route.Should().HaveCount(3);
        route[0].SourceTable.TableOrUdfName.Should().Be("sites");
        route[0].TargetTable.TableOrUdfName.Should().Be("samples");
        route[1].SourceTable.TableOrUdfName.Should().Be("samples");
        route[1].TargetTable.TableOrUdfName.Should().Be("samples"); // Self-relation
        route[2].SourceTable.TableOrUdfName.Should().Be("samples");
        route[2].TargetTable.TableOrUdfName.Should().Be("measurements");
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void GetRoute_ComplexScenario_DatabaseStyleRelationships()
    {
        // Arrange - Realistic archaeological database scenario
        var sites = CreateTable(1, "tbl_sites");
        var samples = CreateTable(2, "tbl_physical_samples");
        var sampleGroups = CreateTable(3, "tbl_sample_groups");
        var analysisEntities = CreateTable(4, "tbl_analysis_entities");
        var measurements = CreateTable(5, "tbl_measured_values");

        var edges = new List<TableRelation>
        {
            CreateRelation(1, sites, samples, "site_id", "site_id"),
            CreateRelation(2, samples, sampleGroups, "sample_id", "sample_id"),
            CreateRelation(3, sampleGroups, analysisEntities, "sample_group_id", "sample_group_id"),
            CreateRelation(4, analysisEntities, measurements, "analysis_entity_id", "analysis_entity_id"),
        };

        var graph = new RouteResolver(edges);

        // Act - Use simplified table names
        var route = graph.Resolve(new[] { "sites", "physical_samples", "sample_groups", "analysis_entities", "measured_values" });

        // Assert
        route.Should().HaveCount(4);
        route[0].SourceTable.TableOrUdfName.Should().Be("tbl_sites");
        route[0].TargetTable.TableOrUdfName.Should().Be("tbl_physical_samples");
        route[3].SourceTable.TableOrUdfName.Should().Be("tbl_analysis_entities");
        route[3].TargetTable.TableOrUdfName.Should().Be("tbl_measured_values");
    }

    #endregion
}

/// <summary>
/// Unit tests for RouteGraphFactory
/// </summary>
public class RouteGraphFactoryTests
{
    private readonly Mock<IRepositoryRegistry> _mockRegistry;
    private readonly Mock<IEdgeRepository> _mockRelationRepository;
    private readonly RouteGraphFactory _factory;

    public RouteGraphFactoryTests()
    {
        _mockRegistry = new Mock<IRepositoryRegistry>();
        _mockRelationRepository = new Mock<IEdgeRepository>();
        _mockRegistry.Setup(x => x.Relations).Returns(_mockRelationRepository.Object);
        _factory = new RouteGraphFactory(_mockRegistry.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidRegistry_CreatesInstance()
    {
        // Act
        var factory = new RouteGraphFactory(_mockRegistry.Object);

        // Assert
        factory.Should().NotBeNull();
    }

    #endregion

    #region CreateGraph Tests

    [Fact]
    public void CreateGraph_CallsRepositoryGetEdges()
    {
        // Arrange
        var testEdges = new List<TableRelation>();
        _mockRelationRepository.Setup(x => x.GetEdges(It.IsAny<bool>())).Returns(testEdges);

        // Act
        var graph = _factory.CreateGraph();

        // Assert
        _mockRelationRepository.Verify(x => x.GetEdges(It.IsAny<bool>()), Times.Once);
        graph.Should().NotBeNull();
        graph.Should().BeOfType<RouteResolver>();
    }

    [Fact]
    public void CreateGraph_WithEdgesFromRepository_CreatesPopulatedGraph()
    {
        // Arrange
        var sites = new Table { TableId = 1, TableOrUdfName = "sites" };
        var samples = new Table { TableId = 2, TableOrUdfName = "samples" };
        var testEdges = new List<TableRelation>
        {
            new TableRelation
            {
                TableRelationId = 1,
                SourceTable = sites,
                TargetTable = samples,
                SourceColumnName = "site_id",
                TargetColumnName = "site_id",
            },
        };
        _mockRelationRepository.Setup(x => x.GetEdges(It.IsAny<bool>())).Returns(testEdges);

        // Act
        var graph = _factory.CreateGraph();

        // Assert
        graph.Relations.Should().HaveCount(2); // Original + reversed due to bidirectional=true
        graph.Nodes.Should().HaveCount(2);
        graph.Relations.Should().ContainKey(("sites", "samples"));
        graph.Relations.Should().ContainKey(("samples", "sites"));
    }

    [Fact]
    public void CreateGraph_WithEmptyRepository_CreatesEmptyGraph()
    {
        // Arrange
        _mockRelationRepository.Setup(x => x.GetEdges(It.IsAny<bool>())).Returns(new List<TableRelation>());

        // Act
        var graph = _factory.CreateGraph();

        // Assert
        graph.Relations.Should().BeEmpty();
        graph.Nodes.Should().BeEmpty();
    }

    #endregion
}
