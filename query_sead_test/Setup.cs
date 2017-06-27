using QuerySeadAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuerySeadTests
{
    public class TestDependencyService : DependencyService
    {
        public TestDependencyService()
        {

        }

        public void Register()
        {
            Register(null);
        }
    }
}
