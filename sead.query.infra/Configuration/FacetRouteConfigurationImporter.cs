using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SeadQueryInfra;

public sealed class FacetRouteConfigurationImporter : IFacetRouteConfigurationImporter
{
    private const string ExpectedTargetSchema = "facet";
    private const string ExpectedImportMode = "merge-into-existing";

    private readonly IFacetContext _context;
    private ImportIdAllocator _idAllocator;

    public FacetRouteConfigurationImporter(IFacetContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void ValidateFile(string filePath)
    {
        var loadedConfiguration = LoadValidatedConfiguration(filePath);
        ValidateResolvedConfiguration(loadedConfiguration.Document);
    }

    public void ImportFromFile(string filePath)
    {
        var loadedConfiguration = LoadValidatedConfiguration(filePath);
        ValidateResolvedConfiguration(loadedConfiguration.Document);

        var dbContext =
            _context as DbContext
            ?? throw new InvalidOperationException("Facet route configuration import requires an EF DbContext-backed facet context.");

        var contentHash = ComputeContentHash(loadedConfiguration.FileContent);
        var document = loadedConfiguration.Document;
        _idAllocator = new ImportIdAllocator(_context);

        using var transaction = dbContext.Database.BeginTransaction();

        var tablesByName = LoadLookup(_context.Tables.ToList(), table => table.TableOrUdfName);
        var facetGroupsByKey = LoadLookup(_context.FacetGroups.ToList(), group => group.FacetGroupKey);
        var facetTypesByName = LoadLookup(_context.FacetTypes.ToList(), facetType => facetType.FacetTypeName);
        var facetsByCode = LoadLookup(_context.Facets.ToList(), facet => facet.FacetCode);
        var anchorsByName = LoadLookup(_context.Set<Anchor>().ToList(), anchor => anchor.Name);
        var routesByName = LoadLookup(_context.Set<Route>().ToList(), route => route.Name);

        UpsertAnchors(document, tablesByName, anchorsByName);
        _context.SaveChanges();

        var generatedRoutes = GenerateRoutes(document, anchorsByName, tablesByName);
        UpsertRoutes(generatedRoutes, routesByName, tablesByName);
        _context.SaveChanges();

        UpsertFacets(document, facetsByCode, facetGroupsByKey, facetTypesByName, tablesByName);
        _context.SaveChanges();

        facetsByCode = LoadLookup(_context.Facets.ToList(), facet => facet.FacetCode);

        UpsertFacetTables(document, facetsByCode, tablesByName);
        UpsertFacetAnchors(document, facetsByCode, anchorsByName, routesByName);
        UpsertFacetTemplates(document, facetsByCode);
        UpsertFacetClauses(document, facetsByCode);
        UpsertConfigRevision(document, filePath, contentHash);

        _context.SaveChanges();
        transaction.Commit();
    }

    private void ValidateResolvedConfiguration(FacetRouteConfigurationDocument document)
    {
        var tables = _context.Tables.ToList();
        var tablesByName = LoadLookup(tables, table => table.TableOrUdfName);
        var tablesById = tables.ToDictionary(table => table.TableId);
        var facetGroupsByKey = LoadLookup(_context.FacetGroups.ToList(), group => group.FacetGroupKey);
        var facetTypesByName = LoadLookup(_context.FacetTypes.ToList(), facetType => facetType.FacetTypeName);

        var facetsByCode = LoadLookup(_context.Facets.ToList(), facet => facet.FacetCode);
        foreach (var facetDefinition in document.Facets)
        {
            if (!facetsByCode.ContainsKey(facetDefinition.Key))
            {
                facetsByCode[facetDefinition.Key] = new Facet { FacetCode = facetDefinition.Key };
            }
        }

        var anchorsByName = CreateValidationAnchors(document, tablesByName);
        var routesByName = CreateValidationRoutes(document, anchorsByName, tablesByName, tablesById);

        ValidateFacetDefinitions(document, facetsByCode, facetGroupsByKey, facetTypesByName, tablesByName);
        ValidateFacetAnchorBindings(document, anchorsByName, routesByName);
    }

    private static IDeserializer CreateDeserializer()
    {
        return new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).IgnoreUnmatchedProperties().Build();
    }

    private static LoadedFacetRouteConfiguration LoadValidatedConfiguration(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Configuration file path is null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Facet route configuration file '{filePath}' was not found.", filePath);

        var fileContent = File.ReadAllText(filePath);
        using var reader = new StringReader(fileContent);
        var document =
            CreateDeserializer().Deserialize<FacetRouteConfigurationDocument>(reader)
            ?? throw new InvalidOperationException($"Facet route configuration file '{filePath}' could not be deserialized.");

        ValidateDocument(document, filePath);
        return new LoadedFacetRouteConfiguration(fileContent, document);
    }

    private static Dictionary<string, T> LoadLookup<T>(IEnumerable<T> items, Func<T, string> selector)
    {
        return items
            .Where(item => !string.IsNullOrWhiteSpace(selector(item)))
            .ToDictionary(selector, item => item, StringComparer.OrdinalIgnoreCase);
    }

    private static void ValidateDocument(FacetRouteConfigurationDocument document, string filePath)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (document.SchemaVersion != 1)
            throw new InvalidOperationException(
                $"Facet route configuration file '{filePath}' uses unsupported schema version '{document.SchemaVersion}'."
            );

        if (document.RuntimeImport is null)
            throw new InvalidOperationException($"Facet route configuration file '{filePath}' is missing runtime_import settings.");

        if (!string.Equals(document.RuntimeImport.TargetSchema, ExpectedTargetSchema, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Facet route configuration file '{filePath}' targets schema '{document.RuntimeImport.TargetSchema}'. "
                    + $"Only '{ExpectedTargetSchema}' is supported by the current importer."
            );
        }

        if (!string.Equals(document.RuntimeImport.Mode, ExpectedImportMode, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Facet route configuration file '{filePath}' uses import mode '{document.RuntimeImport.Mode}'. "
                    + $"Only '{ExpectedImportMode}' is supported by the current importer."
            );
        }

        if (document.Anchors.Count == 0)
            throw new InvalidOperationException($"Facet route configuration file '{filePath}' does not define any anchors.");

        if (document.RouteFamilies.Count == 0)
            throw new InvalidOperationException($"Facet route configuration file '{filePath}' does not define any route families.");

        if (document.Facets.Count == 0)
            throw new InvalidOperationException($"Facet route configuration file '{filePath}' does not define any facets.");

        if (string.IsNullOrWhiteSpace(document.ConfigRevision))
            throw new InvalidOperationException(
                $"Facet route configuration file '{filePath}' is missing required field 'config_revision'."
            );
    }

    private void UpsertAnchors(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Table> tablesByName,
        IDictionary<string, Anchor> anchorsByName
    )
    {
        foreach (var anchorDefinition in document.Anchors)
        {
            ValidateRequiredValue(anchorDefinition.Key, nameof(anchorDefinition.Key), "anchor");
            ValidateRequiredValue(anchorDefinition.Table, nameof(anchorDefinition.Table), $"anchor '{anchorDefinition.Key}'");

            var table = ResolveRequiredLookup(tablesByName, anchorDefinition.Table, $"anchor '{anchorDefinition.Key}' table");

            if (!anchorsByName.TryGetValue(anchorDefinition.Key, out var anchor))
            {
                anchor = new Anchor { Name = anchorDefinition.Key };
                anchor.AnchorId = _idAllocator.NextAnchorId();
                _context.Set<Anchor>().Add(anchor);
                anchorsByName[anchorDefinition.Key] = anchor;
            }

            anchor.TableId = table.TableId;
            anchor.Description = anchorDefinition.Description ?? string.Empty;
        }
    }

    private static Dictionary<string, Anchor> CreateValidationAnchors(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        var anchorsByName = new Dictionary<string, Anchor>(StringComparer.OrdinalIgnoreCase);

        foreach (var anchorDefinition in document.Anchors)
        {
            ValidateRequiredValue(anchorDefinition.Key, nameof(anchorDefinition.Key), "anchor");
            ValidateRequiredValue(anchorDefinition.Table, nameof(anchorDefinition.Table), $"anchor '{anchorDefinition.Key}'");

            var table = ResolveRequiredLookup(tablesByName, anchorDefinition.Table, $"anchor '{anchorDefinition.Key}' table");
            anchorsByName[anchorDefinition.Key] = new Anchor
            {
                Name = anchorDefinition.Key,
                TableId = table.TableId,
                Table = table,
                Description = anchorDefinition.Description ?? string.Empty,
            };
        }

        return anchorsByName;
    }

    private static Dictionary<string, GeneratedRouteDefinition> GenerateRoutes(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Anchor> anchorsByName,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        var macrosByName = document.RouteMacros.ToDictionary(macro => macro.Key, macro => macro, StringComparer.OrdinalIgnoreCase);
        var generatedRoutes = new Dictionary<string, GeneratedRouteDefinition>(StringComparer.OrdinalIgnoreCase);

        foreach (var family in document.RouteFamilies)
        {
            ValidateRequiredValue(family.Key, nameof(family.Key), "route family");
            ValidateRequiredValue(family.SourceTable, nameof(family.SourceTable), $"route family '{family.Key}'");
            ValidateRequiredValue(family.GeneratedRouteKeyPattern, nameof(family.GeneratedRouteKeyPattern), $"route family '{family.Key}'");

            var sourceTable = ResolveRequiredLookup(tablesByName, family.SourceTable, $"route family '{family.Key}' source table");

            foreach (var (anchorKey, binding) in family.Anchors)
            {
                var anchor = ResolveRequiredLookup(anchorsByName, anchorKey, $"route family '{family.Key}' anchor");
                var expandedPath = ExpandPath(binding.Path, macrosByName, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
                var fullPath = new List<string> { sourceTable.TableOrUdfName };
                fullPath.AddRange(expandedPath);

                if (fullPath.Count < 2)
                    throw new InvalidOperationException(
                        $"Generated route '{family.Key}' for anchor '{anchorKey}' must contain at least two tables."
                    );

                var targetTable = ResolveRequiredLookup(tablesByName, fullPath[^1], $"route family '{family.Key}' target table");
                if (targetTable.TableId != anchor.TableId)
                {
                    throw new InvalidOperationException(
                        $"Generated route '{family.Key}' for anchor '{anchorKey}' ends on table '{targetTable.TableOrUdfName}', "
                            + $"but the anchor points at '{anchor.TableId}'."
                    );
                }

                var routeName = family.GeneratedRouteKeyPattern.Replace("{anchor}", anchorKey, StringComparison.Ordinal);
                generatedRoutes[routeName] = new GeneratedRouteDefinition(
                    routeName,
                    sourceTable.TableOrUdfName,
                    targetTable.TableOrUdfName,
                    fullPath
                );
            }
        }

        return generatedRoutes;
    }

    private void UpsertRoutes(
        IReadOnlyDictionary<string, GeneratedRouteDefinition> generatedRoutes,
        IDictionary<string, Route> routesByName,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        foreach (var generatedRoute in generatedRoutes.Values)
        {
            var sourceTable = ResolveRequiredLookup(
                tablesByName,
                generatedRoute.SourceTable,
                $"route '{generatedRoute.Name}' source table"
            );
            var targetTable = ResolveRequiredLookup(
                tablesByName,
                generatedRoute.TargetTable,
                $"route '{generatedRoute.Name}' target table"
            );

            if (!routesByName.TryGetValue(generatedRoute.Name, out var route))
            {
                route = new Route { Name = generatedRoute.Name };
                route.RouteId = _idAllocator.NextRouteId();
                _context.Set<Route>().Add(route);
                routesByName[generatedRoute.Name] = route;
            }

            route.SourceTableId = sourceTable.TableId;
            route.TargetTableId = targetTable.TableId;
            route.Specification = string.Join(" -> ", generatedRoute.FullPath);
            route.Alias ??= string.Empty;
        }
    }

    private Dictionary<string, ValidationRoute> CreateValidationRoutes(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Anchor> anchorsByName,
        IReadOnlyDictionary<string, Table> tablesByName,
        IReadOnlyDictionary<int, Table> tablesById
    )
    {
        var routesByName = _context
            .Set<Route>()
            .ToList()
            .Where(route => !string.IsNullOrWhiteSpace(route.Name))
            .ToDictionary(
                route => route.Name,
                route => new ValidationRoute(
                    route.Name,
                    route.TargetTableId,
                    tablesById.TryGetValue(route.TargetTableId, out var targetTable) ? targetTable.TableOrUdfName : route.TargetTableId.ToString()
                ),
                StringComparer.OrdinalIgnoreCase
            );

        var generatedRoutes = GenerateRoutes(document, anchorsByName, tablesByName);
        foreach (var generatedRoute in generatedRoutes.Values)
        {
            var targetTable = ResolveRequiredLookup(
                tablesByName,
                generatedRoute.TargetTable,
                $"route '{generatedRoute.Name}' target table"
            );
            routesByName[generatedRoute.Name] = new ValidationRoute(generatedRoute.Name, targetTable.TableId, targetTable.TableOrUdfName);
        }

        return routesByName;
    }

    private void UpsertFacets(
        FacetRouteConfigurationDocument document,
        IDictionary<string, Facet> facetsByCode,
        IReadOnlyDictionary<string, FacetGroup> facetGroupsByKey,
        IReadOnlyDictionary<string, FacetType> facetTypesByName,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        foreach (var facetDefinition in document.Facets)
        {
            var facetGroup = ResolveRequiredLookup(facetGroupsByKey, facetDefinition.GroupKey, $"facet '{facetDefinition.Key}' group");
            var facetTypeName = NormalizeFacetTypeName(facetDefinition.Type);
            var facetType = ResolveRequiredLookup(facetTypesByName, facetTypeName, $"facet '{facetDefinition.Key}' type");
            var sourceTable = ResolveRequiredLookup(
                tablesByName,
                facetDefinition.SourceTable,
                $"facet '{facetDefinition.Key}' source table"
            );

            if (!facetsByCode.TryGetValue(facetDefinition.Key, out var facet))
            {
                facet = new Facet { FacetCode = facetDefinition.Key };
                facet.FacetId = _idAllocator.NextFacetId();
                _context.Facets.Add(facet);
                facetsByCode[facetDefinition.Key] = facet;
            }

            facet.DisplayTitle = facetDefinition.DisplayTitle;
            facet.Description = facetDefinition.Description ?? string.Empty;
            facet.FacetGroupId = facetGroup.FacetGroupId;
            facet.FacetTypeId = facetType.FacetTypeId;
            facet.CategoryIdExpr = facetDefinition.Category.IdExpr;
            facet.CategoryIdType = facetDefinition.Category.DataType;
            facet.CategoryIdOperator = facetDefinition.Category.Operator;
            facet.CategoryNameExpr = facetDefinition.Category.NameExpr;
            facet.SortExpr = facetDefinition.SortExpr;
            facet.IsApplicable = facetDefinition.Flags.IsApplicable;
            facet.IsDefault = facetDefinition.Flags.IsDefault;
            facet.AggregateType = facetDefinition.Aggregate.Type;
            facet.AggregateTitle = facetDefinition.Aggregate.Title;
            facet.AggregateFacetId = ResolveAggregateFacetId(facetDefinition.Aggregate.FacetKey, facetsByCode);
        }
    }

    private static void ValidateFacetDefinitions(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Facet> facetsByCode,
        IReadOnlyDictionary<string, FacetGroup> facetGroupsByKey,
        IReadOnlyDictionary<string, FacetType> facetTypesByName,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        foreach (var facetDefinition in document.Facets)
        {
            _ = ResolveRequiredLookup(facetGroupsByKey, facetDefinition.GroupKey, $"facet '{facetDefinition.Key}' group");
            _ = ResolveRequiredLookup(
                facetTypesByName,
                NormalizeFacetTypeName(facetDefinition.Type),
                $"facet '{facetDefinition.Key}' type"
            );
            _ = ResolveRequiredLookup(tablesByName, facetDefinition.SourceTable, $"facet '{facetDefinition.Key}' source table");
            _ = ResolveAggregateFacetId(facetDefinition.Aggregate.FacetKey, new Dictionary<string, Facet>(facetsByCode, StringComparer.OrdinalIgnoreCase));
        }
    }

    private void UpsertFacetTables(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Facet> facetsByCode,
        IReadOnlyDictionary<string, Table> tablesByName
    )
    {
        var facetTableSet = _context.Set<FacetTable>();

        foreach (var facetDefinition in document.Facets)
        {
            var facet = ResolveRequiredLookup(facetsByCode, facetDefinition.Key, $"facet '{facetDefinition.Key}'");
            var sourceTable = ResolveRequiredLookup(
                tablesByName,
                facetDefinition.SourceTable,
                $"facet '{facetDefinition.Key}' source table"
            );
            var existingSourceTable = facetTableSet.FirstOrDefault(table => table.FacetId == facet.FacetId && table.SequenceId == 1);

            if (existingSourceTable is null)
            {
                facetTableSet.Add(
                    new FacetTable
                    {
                        FacetTableId = _idAllocator.NextFacetTableId(),
                        FacetId = facet.FacetId,
                        SequenceId = 1,
                        TableId = sourceTable.TableId,
                        UdfCallArguments = null,
                        Alias = null,
                    }
                );
                continue;
            }

            existingSourceTable.TableId = sourceTable.TableId;
            existingSourceTable.UdfCallArguments = null;
            existingSourceTable.Alias = null;
        }
    }

    private void UpsertFacetAnchors(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Facet> facetsByCode,
        IReadOnlyDictionary<string, Anchor> anchorsByName,
        IReadOnlyDictionary<string, Route> routesByName
    )
    {
        var facetAnchorSet = _context.Set<FacetAnchor>();

        foreach (var facetDefinition in document.Facets)
        {
            var facet = ResolveRequiredLookup(facetsByCode, facetDefinition.Key, $"facet '{facetDefinition.Key}'");
            var existingFacetAnchors = facetAnchorSet.Where(facetAnchor => facetAnchor.FacetId == facet.FacetId).ToList();

            foreach (var anchorBinding in facetDefinition.Anchors)
            {
                var anchor = ResolveRequiredLookup(anchorsByName, anchorBinding.Anchor, $"facet '{facetDefinition.Key}' anchor");
                var route = ResolveRequiredLookup(routesByName, anchorBinding.Route, $"facet '{facetDefinition.Key}' route");

                var existingFacetAnchor = existingFacetAnchors.FirstOrDefault(facetAnchor => facetAnchor.AnchorId == anchor.AnchorId);
                if (existingFacetAnchor is null)
                {
                    var facetAnchor = new FacetAnchor
                    {
                        FacetAnchorId = _idAllocator.NextFacetAnchorId(),
                        FacetId = facet.FacetId,
                        AnchorId = anchor.AnchorId,
                        RouteId = route.RouteId,
                    };
                    facetAnchorSet.Add(facetAnchor);
                    continue;
                }

                existingFacetAnchor.RouteId = route.RouteId;
            }
        }
    }

    private static void ValidateFacetAnchorBindings(
        FacetRouteConfigurationDocument document,
        IReadOnlyDictionary<string, Anchor> anchorsByName,
        IReadOnlyDictionary<string, ValidationRoute> routesByName
    )
    {
        foreach (var facetDefinition in document.Facets)
        {
            foreach (var anchorBinding in facetDefinition.Anchors)
            {
                var anchor = ResolveRequiredLookup(anchorsByName, anchorBinding.Anchor, $"facet '{facetDefinition.Key}' anchor");
                var route = ResolveRequiredLookup(routesByName, anchorBinding.Route, $"facet '{facetDefinition.Key}' route");

                if (route.TargetTableId != anchor.TableId)
                {
                    throw new InvalidOperationException(
                        $"Facet '{facetDefinition.Key}' binds anchor '{anchor.Name}' to route '{route.Name}', "
                            + $"but the route ends on table '{route.TargetTableName}' instead of anchor table '{anchor.Table?.TableOrUdfName ?? anchor.TableId.ToString()}'."
                    );
                }
            }
        }
    }

    private void UpsertFacetTemplates(FacetRouteConfigurationDocument document, IReadOnlyDictionary<string, Facet> facetsByCode)
    {
        var templateSet = _context.Set<FacetTemplate>();

        foreach (var facetDefinition in document.Facets)
        {
            var facet = ResolveRequiredLookup(facetsByCode, facetDefinition.Key, $"facet '{facetDefinition.Key}'");

            foreach (var anchorBinding in facetDefinition.Anchors.Where(binding => !string.IsNullOrWhiteSpace(binding.SqlOverride)))
            {
                var existingTemplate = templateSet.FirstOrDefault(template =>
                    template.FacetId == facet.FacetId && template.AnchorName == anchorBinding.Anchor
                );

                if (existingTemplate is null)
                {
                    templateSet.Add(
                        new FacetTemplate
                        {
                            TemplateId = _idAllocator.NextFacetTemplateId(),
                            FacetId = facet.FacetId,
                            AnchorName = anchorBinding.Anchor,
                            SqlTemplate = anchorBinding.SqlOverride!,
                        }
                    );
                    continue;
                }

                existingTemplate.SqlTemplate = anchorBinding.SqlOverride!;
            }
        }
    }

    private void UpsertFacetClauses(FacetRouteConfigurationDocument document, IReadOnlyDictionary<string, Facet> facetsByCode)
    {
        var clauseSet = _context.Set<FacetClause>();

        foreach (var facetDefinition in document.Facets)
        {
            var facet = ResolveRequiredLookup(facetsByCode, facetDefinition.Key, $"facet '{facetDefinition.Key}'");
            var existingClauses = clauseSet.Where(clause => clause.FacetId == facet.FacetId).ToList();
            var configuredClauseText = new HashSet<string>(facetDefinition.Clauses, StringComparer.Ordinal);

            foreach (var obsoleteClause in existingClauses.Where(clause => !configuredClauseText.Contains(clause.Clause)).ToList())
            {
                clauseSet.Remove(obsoleteClause);
            }

            var existingClauseText = new HashSet<string>(existingClauses.Select(clause => clause.Clause), StringComparer.Ordinal);

            foreach (var clause in facetDefinition.Clauses.Where(clause => !existingClauseText.Contains(clause)))
            {
                clauseSet.Add(
                    new FacetClause
                    {
                        FacetClauseId = _idAllocator.NextFacetClauseId(),
                        FacetId = facet.FacetId,
                        Clause = clause,
                        EnforceConstraint = false,
                    }
                );
            }

            foreach (var existingClause in existingClauses.Where(clause => configuredClauseText.Contains(clause.Clause)))
            {
                existingClause.EnforceConstraint = false;
            }
        }
    }

    private void UpsertConfigRevision(FacetRouteConfigurationDocument document, string filePath, string contentHash)
    {
        var revisionSet = _context.Set<FacetConfigRevision>();
        var existingRevision = revisionSet.FirstOrDefault(revision => revision.ConfigRevision == document.ConfigRevision);

        foreach (var activeRevision in revisionSet.Where(revision => revision.IsActive).ToList())
        {
            activeRevision.IsActive = false;
        }

        if (existingRevision is null)
        {
            existingRevision = new FacetConfigRevision
            {
                RevisionId = _idAllocator.NextConfigRevisionId(),
                ConfigRevision = document.ConfigRevision,
            };
            revisionSet.Add(existingRevision);
        }

        existingRevision.SourceCommit = ResolveSourceCommit();
        existingRevision.ContentHash = contentHash;
        existingRevision.ImportedAt = DateTime.UtcNow;
        existingRevision.ImportedBy = ResolveImportedBy(filePath);
        existingRevision.IsActive = true;
    }

    private static IReadOnlyList<string> ExpandPath(
        IReadOnlyList<RoutePathStepDefinition> path,
        IReadOnlyDictionary<string, RouteMacroDefinition> macrosByName,
        HashSet<string> expansionStack
    )
    {
        var expandedPath = new List<string>();

        foreach (var step in path)
        {
            if (!string.IsNullOrWhiteSpace(step.Table))
            {
                expandedPath.Add(step.Table);
                continue;
            }

            if (string.IsNullOrWhiteSpace(step.Macro))
                throw new InvalidOperationException("Route path step must define either a table or a macro reference.");

            if (!expansionStack.Add(step.Macro))
                throw new InvalidOperationException($"Route macro expansion cycle detected at '{step.Macro}'.");

            var macro = ResolveRequiredLookup(macrosByName, step.Macro, $"route macro '{step.Macro}'");
            expandedPath.AddRange(ExpandPath(macro.Path, macrosByName, expansionStack));
            expansionStack.Remove(step.Macro);
        }

        return expandedPath;
    }

    private static int ResolveAggregateFacetId(string facetKey, IDictionary<string, Facet> facetsByCode)
    {
        if (string.IsNullOrWhiteSpace(facetKey))
            return 0;

        if (facetsByCode.TryGetValue(facetKey, out var facet))
            return facet.FacetId;

        throw new InvalidOperationException($"Aggregate facet '{facetKey}' is not available in the current facet schema.");
    }

    private static string NormalizeFacetTypeName(string facetType)
    {
        return facetType switch
        {
            "intersect" => "rangesintersect",
            _ => facetType,
        };
    }

    private static string ComputeContentHash(string fileContent)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(fileContent);
        return Convert.ToHexString(sha256.ComputeHash(bytes)).ToLowerInvariant();
    }

    private static string ResolveSourceCommit()
    {
        return FirstNonEmpty(
            Environment.GetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_SOURCE_COMMIT"),
            Environment.GetEnvironmentVariable("GIT_COMMIT"),
            Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION"),
            Environment.GetEnvironmentVariable("SOURCE_COMMIT")
        );
    }

    private static string ResolveImportedBy(string filePath)
    {
        return FirstNonEmpty(
            Environment.GetEnvironmentVariable("SEAD_QUERY_FACET_CONFIG_IMPORTED_BY"),
            Environment.GetEnvironmentVariable("USER"),
            Environment.GetEnvironmentVariable("USERNAME"),
            Environment.UserName,
            Path.GetFileName(filePath)
        );
    }

    private static string FirstNonEmpty(params string[] values)
    {
        return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value)) ?? string.Empty;
    }

    private static T ResolveRequiredLookup<T>(IReadOnlyDictionary<string, T> lookup, string key, string description)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new InvalidOperationException($"{description} is missing a required key.");

        if (lookup.TryGetValue(key, out var value))
            return value;

        throw new InvalidOperationException($"{description} '{key}' could not be resolved in the current facet schema.");
    }

    private static void ValidateRequiredValue(string value, string fieldName, string description)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{description} is missing required field '{fieldName}'.");
    }

    private sealed record GeneratedRouteDefinition(string Name, string SourceTable, string TargetTable, IReadOnlyList<string> FullPath);

    private sealed record ValidationRoute(string Name, int TargetTableId, string TargetTableName);

    private sealed record LoadedFacetRouteConfiguration(string FileContent, FacetRouteConfigurationDocument Document);

    private sealed class FacetRouteConfigurationDocument
    {
        public int SchemaVersion { get; set; }

        public string ConfigRevision { get; set; } = string.Empty;

        public RuntimeImportDefinition RuntimeImport { get; set; } = new();

        public List<AnchorDefinition> Anchors { get; set; } = [];

        public List<RouteMacroDefinition> RouteMacros { get; set; } = [];

        public List<RouteFamilyDefinition> RouteFamilies { get; set; } = [];

        public List<FacetDefinition> Facets { get; set; } = [];
    }

    private sealed class RuntimeImportDefinition
    {
        public string TargetSchema { get; set; } = string.Empty;

        public string Mode { get; set; } = string.Empty;
    }

    private sealed class AnchorDefinition
    {
        public string Key { get; set; } = string.Empty;

        public string Table { get; set; } = string.Empty;

        public string KeyColumn { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }

    private sealed class RouteMacroDefinition
    {
        public string Key { get; set; } = string.Empty;

        public List<RoutePathStepDefinition> Path { get; set; } = [];
    }

    private sealed class RouteFamilyDefinition
    {
        public string Key { get; set; } = string.Empty;

        public string SourceTable { get; set; } = string.Empty;

        public string SourceKeyColumn { get; set; } = string.Empty;

        public string GeneratedRouteKeyPattern { get; set; } = string.Empty;

        public Dictionary<string, RouteBindingDefinition> Anchors { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    private sealed class RouteBindingDefinition
    {
        public List<RoutePathStepDefinition> Path { get; set; } = [];
    }

    private sealed class RoutePathStepDefinition
    {
        public string Table { get; set; } = string.Empty;

        public string Macro { get; set; } = string.Empty;
    }

    private sealed class FacetDefinition
    {
        public string Key { get; set; } = string.Empty;

        public string DisplayTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string GroupKey { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string SourceTable { get; set; } = string.Empty;

        public FacetCategoryDefinition Category { get; set; } = new();

        public string SortExpr { get; set; } = string.Empty;

        public FacetFlagsDefinition Flags { get; set; } = new();

        public FacetAggregateDefinition Aggregate { get; set; } = new();

        public List<string> Clauses { get; set; } = [];

        public List<FacetAnchorBindingDefinition> Anchors { get; set; } = [];
    }

    private sealed class FacetCategoryDefinition
    {
        public string IdExpr { get; set; } = string.Empty;

        public string NameExpr { get; set; } = string.Empty;

        public string DataType { get; set; } = string.Empty;

        public string Operator { get; set; } = string.Empty;
    }

    private sealed class FacetFlagsDefinition
    {
        public bool IsApplicable { get; set; }

        public bool IsDefault { get; set; }
    }

    private sealed class FacetAggregateDefinition
    {
        public string Type { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string FacetKey { get; set; } = string.Empty;
    }

    private sealed class FacetAnchorBindingDefinition
    {
        public string Anchor { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string SqlOverride { get; set; } = string.Empty;
    }

    private sealed class ImportIdAllocator
    {
        private readonly IFacetContext _context;
        private int _nextAnchorId;
        private int _nextRouteId;
        private int _nextRouteStepId;
        private int _nextFacetId;
        private int _nextFacetTableId;
        private int _nextFacetAnchorId;
        private int _nextConfigRevisionId;
        private int? _nextFacetTemplateId;
        private int? _nextFacetClauseId;

        public ImportIdAllocator(IFacetContext context)
        {
            _context = context;
            _nextAnchorId = GetNextId(context.Set<Anchor>(), anchor => anchor.AnchorId);
            _nextRouteId = GetNextId(context.Set<Route>(), route => route.RouteId);
            _nextRouteStepId = GetNextId(context.Set<RouteStep>(), routeStep => routeStep.RouteStepId);
            _nextFacetId = GetNextId(context.Facets, facet => facet.FacetId);
            _nextFacetTableId = GetNextId(context.Set<FacetTable>(), facetTable => facetTable.FacetTableId);
            _nextFacetAnchorId = GetNextId(context.Set<FacetAnchor>(), facetAnchor => facetAnchor.FacetAnchorId);
            _nextConfigRevisionId = GetNextId(context.Set<FacetConfigRevision>(), revision => revision.RevisionId);
        }

        public int NextAnchorId() => _nextAnchorId++;

        public int NextRouteId() => _nextRouteId++;

        public int NextRouteStepId() => _nextRouteStepId++;

        public int NextFacetId() => _nextFacetId++;

        public int NextFacetTableId() => _nextFacetTableId++;

        public int NextFacetAnchorId() => _nextFacetAnchorId++;

        public int NextConfigRevisionId() => _nextConfigRevisionId++;

        public int NextFacetTemplateId()
        {
            _nextFacetTemplateId ??= GetNextId(_context.Set<FacetTemplate>(), template => template.TemplateId);
            var nextId = _nextFacetTemplateId.Value;
            _nextFacetTemplateId = nextId + 1;
            return nextId;
        }

        public int NextFacetClauseId()
        {
            _nextFacetClauseId ??= GetNextId(_context.Set<FacetClause>(), clause => clause.FacetClauseId);
            var nextId = _nextFacetClauseId.Value;
            _nextFacetClauseId = nextId + 1;
            return nextId;
        }

        private static int GetNextId<TEntity>(IQueryable<TEntity> query, Func<TEntity, int> selector)
            where TEntity : class
        {
            return query.Any() ? query.Max(selector) + 1 : 1;
        }
    }
}
