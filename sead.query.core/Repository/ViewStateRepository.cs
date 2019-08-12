using DataAccessPostgreSqlProvider;
using SeadQueryCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SeadQueryCore {

    public class ViewStateRepository : Repository<ViewState, string>
    {
        public ViewStateRepository(DomainModelDbContext context) : base(context)
        {
        }

    }

}
