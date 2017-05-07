using DataAccessPostgreSqlProvider;
using QuerySeadDomain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QuerySeadDomain {

    public class ViewStateRepository : Repository<ViewState>
    {
        public ViewStateRepository(DomainModelDbContext context) : base(context)
        {
        }

        public List<ViewState> GetBySessionId(string sessionId, int count=5)
        {
            return Context.ViewStates
                .Where(x => x.SessionId == sessionId)
                .OrderByDescending(z => z.CreateTime).Take(count).ToList();
        }
    }

}
