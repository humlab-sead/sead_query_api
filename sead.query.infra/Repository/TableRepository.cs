﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class TableRepository(IFacetContext context) : Repository<Table, int>(context), ITableRepository
    {
    }

    public class TableRelationRepository(IFacetContext context) : Repository<TableRelation, int>(context), ITableRelationRepository
    {
        public override IEnumerable<TableRelation> GetAll()
        {
            return Context.Set<TableRelation>().BuildEntity().ToList();
        }

        public TableRelation FindByName(string sourceName, string targetName)
        {
            string[] names = [sourceName, targetName];
            return Context.Set<TableRelation>().BuildEntity()
                .Where(
                    r => (r.SourceTable.TableOrUdfName == sourceName && r.TargetTable.TableOrUdfName == targetName)
                ).FirstOrDefault();
        }
    }

    public static class EdgeRepositoryEagerBuilder
    {
        public static IQueryable<TableRelation> BuildEntity(this IQueryable<TableRelation> query)
        {
            return query.Include(x => x.SourceTable).Include(x => x.TargetTable);
        }
    }
}
