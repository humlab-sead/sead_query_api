using DataAccessPostgreSqlProvider;
using QuerySeadDomain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QuerySeadDomain {

    public class ResultRepository : Repository<ResultDefinition>
    {
        public ResultRepository(DomainModelDbContext context) : base(context)
        {
        }

        public Dictionary<string, ResultDefinition> ToDictionary()
        {
            return GetAll().ToDictionary(x => x.Key);
        }

        public override IEnumerable<ResultDefinition> GetAll()
        {
            return Context.Set<ResultDefinition>().BuildEntity().ToList();
        }

        public ResultDefinition GetByKey(string key)
        {
            return GetAll().FirstOrDefault(x => x.Key == key);
        }
    }

    public static class ResultDefinitionRepositoryEagerBuilder {

        public static IQueryable<ResultDefinition> BuildEntity(this IQueryable<ResultDefinition> query)
        {
            return query.Include(x => x.Fields);
        }

    }
}
