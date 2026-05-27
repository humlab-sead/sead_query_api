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
/// Comprehensive unit tests for ArrowRouteParser, covering all scenarios:
/// - Simple route parsing
/// - Macro expansion
/// - Cycle detection
/// - Arrow notation variants
/// - Error handling
/// - Edge cases
/// </summary>
public class ArrowRouteParserTests
{
    private readonly Mock<IRouteRepository> _repository;
    private readonly ArrowRouteParser _parser;

    public ArrowRouteParserTests()
    {
        _repository = new Mock<IRouteRepository>();
        _parser = new ArrowRouteParser(_repository.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithNullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Action act = () => new ArrowRouteParser(null);
        act.Should().Throw<ArgumentNullException>().WithParameterName("repository");
    }

    [Fact]
    public void Constructor_WithValidRepository_CreatesInstance()
    {
        var parser = new ArrowRouteParser(_repository.Object);
        parser.Should().NotBeNull();
    }

    #endregion

    #region ResolveRoute - Basic Functionality Tests

    [Fact]
    public void ResolveRoute_WithNullRoute_ThrowsArgumentException()
    {
        Action act = () => _parser.ResolveRoute(null);
        act.Should().Throw<ArgumentException>().WithParameterName("route").WithMessage("*Route is null or empty*");
    }

    [Fact]
    public void ResolveRoute_WithEmptyRoute_ThrowsArgumentException()
    {
        Action act = () => _parser.ResolveRoute("");
        act.Should().Throw<ArgumentException>().WithParameterName("route").WithMessage("*Route is null or empty*");
    }

    [Fact]
    public void ResolveRoute_WithWhitespaceOnlyRoute_ThrowsArgumentException()
    {
        Action act = () => _parser.ResolveRoute("   ");
        act.Should().Throw<ArgumentException>().WithParameterName("route").WithMessage("*Route is null or empty*");
    }

    [Fact]
    public void ResolveRoute_WithSingleTableName_ReturnsSingleTable()
    {
        // Arrange
        _repository.Setup(x => x.HasRoute("a")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a");

        // Assert
        result.Should().ContainSingle().Which.Should().Be("a");
    }

    [Fact]
    public void ResolveRoute_WithSimpleArrowRoute_ReturnsAllTables()
    {
        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("b")).Returns(false);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        var result = _parser.ResolveRoute("a -> b -> c");

        result.Should().HaveCount(3);
        result.Should().ContainInOrder("a", "b", "c");
    }

    #endregion

    #region Arrow Notation Variants Tests

    [Theory]
    [InlineData("a -> b -> c")]
    [InlineData("a → b → c")]
    [InlineData("a ⇒ b ⇒ c")]
    [InlineData("a ⟶ b ⟶ c")]
    [InlineData("a => b => c")]
    public void ResolveRoute_WithDifferentArrowNotations_ProducesSameResult(string route)
    {
        // Arrange
        _repository.Setup(x => x.HasRoute(It.IsAny<string>())).Returns(false);

        // Act
        var result = _parser.ResolveRoute(route);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainInOrder("a", "b", "c");
    }

    [Theory]
    [InlineData("a->b->c")]
    [InlineData("a  ->  b  ->  c")]
    [InlineData("  a   ->   b   ->   c  ")]
    [InlineData("a\t->\tb\t->\tc")]
    public void ResolveRoute_WithVariableWhitespace_ProducesSameResult(string route)
    {
        // Arrange
        _repository.Setup(x => x.HasRoute(It.IsAny<string>())).Returns(false);

        // Act
        var result = _parser.ResolveRoute(route);

        // Assert
        result.Should().HaveCount(3);
        result.Should().ContainInOrder("a", "b", "c");
    }

    #endregion

    #region Macro Expansion Tests

    [Fact]
    public void ResolveRoute_WithSimpleMacro_ExpandsMacroCorrectly()
    {
        // Arrange
        var sampleChainRoute = new Route { Name = "{m}", Specification = "b -> d" };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("{m}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m}")).Returns(sampleChainRoute);
        _repository.Setup(x => x.HasRoute("b")).Returns(false);
        _repository.Setup(x => x.HasRoute("d")).Returns(false);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a -> {m} -> c");

        // Assert
        result.Should().HaveCount(4);
        result.Should().ContainInOrder("a", "b", "d", "c");
    }

    [Fact]
    public void ResolveRoute_WithNestedMacros_ExpandsAllMacrosCorrectly()
    {
        // Arrange
        var innerMacro = new Route { Name = "{inner}", Specification = "table2 -> table3" };
        var outerMacro = new Route { Name = "{outer}", Specification = "table1 -> {inner} -> table4" };

        _repository.Setup(x => x.HasRoute("{outer}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{outer}")).Returns(outerMacro);
        _repository.Setup(x => x.HasRoute("{inner}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{inner}")).Returns(innerMacro);

        // All concrete tables
        foreach (var table in new[] { "table1", "table2", "table3", "table4" })
        {
            _repository.Setup(x => x.HasRoute(table)).Returns(false);
        }

        // Act
        var result = _parser.ResolveRoute("{outer}");

        // Assert
        result.Should().HaveCount(4);
        result.Should().ContainInOrder("table1", "table2", "table3", "table4");
    }

    [Fact]
    public void ResolveRoute_WithEmptyMacroDefinition_SkipsMacro()
    {
        // Arrange
        var emptyMacro = new Route { Name = "{empty}", Specification = "" };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("{empty}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{empty}")).Returns(emptyMacro);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a -> {empty} -> c");

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainInOrder("a", "c");
    }

    [Fact]
    public void ResolveRoute_WithNullMacroDefinition_SkipsMacro()
    {
        // Arrange
        var nullMacro = new Route { Name = "NULL", Specification = null };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("NULL")).Returns(true);
        _repository.Setup(x => x.GetRoute("NULL")).Returns(nullMacro);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a -> NULL -> c");

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainInOrder("a", "c");
    }

    #endregion

    #region Cycle Detection Tests

    [Fact]
    public void ResolveRoute_WithDirectCycle_ThrowsInvalidOperationException()
    {
        // Arrange - A route that references itself
        var cyclicRoute = new Route { Name = "{cyclic}", Specification = "{cyclic}" };

        _repository.Setup(x => x.HasRoute("{cyclic}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{cyclic}")).Returns(cyclicRoute);

        // Act & Assert
        Action act = () => _parser.ResolveRoute("{cyclic}");
        act.Should().Throw<InvalidOperationException>().WithMessage("*Route expansion cycle detected at '{cyclic}'*");
    }

    [Fact]
    public void ResolveRoute_WithIndirectCycle_ThrowsInvalidOperationException()
    {
        // Arrange - A -> B -> A cycle
        var routeA = new Route { Name = "A", Specification = "B" };
        var routeB = new Route { Name = "B", Specification = "A" };

        _repository.Setup(x => x.HasRoute("A")).Returns(true);
        _repository.Setup(x => x.GetRoute("A")).Returns(routeA);
        _repository.Setup(x => x.HasRoute("B")).Returns(true);
        _repository.Setup(x => x.GetRoute("B")).Returns(routeB);

        // Act & Assert
        Action act = () => _parser.ResolveRoute("A");
        act.Should().Throw<InvalidOperationException>().WithMessage("*Route expansion cycle detected*");
    }

    [Fact]
    public void ResolveRoute_WithLongerCycle_ThrowsInvalidOperationException()
    {
        // Arrange - A -> B -> C -> A cycle
        var routeA = new Route { Name = "A", Specification = "B" };
        var routeB = new Route { Name = "B", Specification = "C" };
        var routeC = new Route { Name = "C", Specification = "A" };

        _repository.Setup(x => x.HasRoute("A")).Returns(true);
        _repository.Setup(x => x.GetRoute("A")).Returns(routeA);
        _repository.Setup(x => x.HasRoute("B")).Returns(true);
        _repository.Setup(x => x.GetRoute("B")).Returns(routeB);
        _repository.Setup(x => x.HasRoute("C")).Returns(true);
        _repository.Setup(x => x.GetRoute("C")).Returns(routeC);

        // Act & Assert
        Action act = () => _parser.ResolveRoute("A");
        act.Should().Throw<InvalidOperationException>().WithMessage("*Route expansion cycle detected*");
    }

    #endregion

    #region Complex Scenario Tests

    [Fact]
    public void ResolveRoute_WithMixedConcreteAndMacroTables_ReturnsCorrectSequence()
    {
        // Arrange
        var datingChain = new Route { Name = "{m2}", Specification = "e -> f" };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("b")).Returns(false);
        _repository.Setup(x => x.HasRoute("{m2}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m2}")).Returns(datingChain);
        _repository.Setup(x => x.HasRoute("e")).Returns(false);
        _repository.Setup(x => x.HasRoute("f")).Returns(false);
        _repository.Setup(x => x.HasRoute("results")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a -> b -> {m2} -> results");

        // Assert
        result.Should().HaveCount(5);
        result.Should().ContainInOrder("a", "b", "e", "f", "results");
    }

    [Fact]
    public void ResolveRoute_WithMultipleMacrosInSequence_ExpandsAllCorrectly()
    {
        // Arrange
        var sampleChain = new Route { Name = "{m}", Specification = "b -> d" };
        var datingChain = new Route { Name = "{m2}", Specification = "e -> f" };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("{m}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m}")).Returns(sampleChain);
        _repository.Setup(x => x.HasRoute("{m2}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m2}")).Returns(datingChain);

        // All concrete tables
        foreach (var table in new[] { "b", "d", "e", "f", "results" })
        {
            _repository.Setup(x => x.HasRoute(table)).Returns(false);
        }

        // Act
        var result = _parser.ResolveRoute("a -> {m} -> {m2} -> results");

        // Assert
        result.Should().HaveCount(6);
        result.Should().ContainInOrder("a", "b", "d", "e", "f", "results");
    }

    #endregion

    #region Edge Cases and Error Scenarios

    [Fact]
    public void ResolveRoute_WithRepositoryReturningNull_SkipsMacro()
    {
        // Arrange
        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("NULL_MACRO")).Returns(true);
        _repository.Setup(x => x.GetRoute("NULL_MACRO")).Returns((Route)null);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        // Act
        var result = _parser.ResolveRoute("a -> NULL_MACRO -> c");

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainInOrder("a", "c");
    }

    [Fact]
    public void ResolveRoute_WithEmptyTokensInRoute_IgnoresEmptyTokens()
    {
        // Arrange
        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        // Act - Double arrows create empty tokens
        var result = _parser.ResolveRoute("a -> -> c");

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainInOrder("a", "c");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ResolveRoute_WithWhitespaceOnlyTokens_IgnoresWhitespaceTokens(string whitespaceToken)
    {
        // Arrange
        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("c")).Returns(false);

        var route = $"a -> {whitespaceToken} -> c";

        // Act
        var result = _parser.ResolveRoute(route);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainInOrder("a", "c");
    }

    #endregion

    #region Integration-Style Tests

    [Fact]
    public void ResolveRoute_CompleteScenario_DatabaseStyleRoutes()
    {
        // Arrange - Simulate realistic database route expansion
        var sampleAnalysisChain = new Route { Name = "{m1}", Specification = "b -> d -> e" };

        var geoLocationChain = new Route { Name = "{m2}", Specification = "f -> g" };

        _repository.Setup(x => x.HasRoute("a")).Returns(false);
        _repository.Setup(x => x.HasRoute("{m1}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m1}")).Returns(sampleAnalysisChain);
        _repository.Setup(x => x.HasRoute("{m2}")).Returns(true);
        _repository.Setup(x => x.GetRoute("{m2}")).Returns(geoLocationChain);

        // All concrete tables
        var concreteTables = new[] { "b", "d", "e", "f", "g", "h" };

        foreach (var table in concreteTables)
        {
            _repository.Setup(x => x.HasRoute(table)).Returns(false);
        }

        // Act
        var result = _parser.ResolveRoute("a -> {m1} -> {m2} -> h");

        // Assert
        result.Should().HaveCount(7);
        result.Should().ContainInOrder("a", "b", "d", "e", "f", "g", "h");
    }

    #endregion

    #region DefaultArrow Constant Test

    [Fact]
    public void DefaultArrow_HasExpectedValue()
    {
        // Assert
        ArrowRouteParser.DefaultArrow.Should().Be("->");
    }

    #endregion
}
