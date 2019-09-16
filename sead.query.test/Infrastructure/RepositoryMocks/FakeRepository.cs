using DataAccessPostgreSqlProvider;
using Microsoft.EntityFrameworkCore;
using Moq;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure.Scaffolding;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SeadQueryTest.RepositoryMocks
{
    public class FakeRepository<TEntity, K> : Repository<TEntity, K> where TEntity : class
    {
        public Mock<DbSet<TEntity>> MockSet { get; }
        public Mock<FacetContext> MockContext { get; }

        public FakeRepository(IQueryable<TEntity> entities = null) : base(new Mock<FacetContext>().Object)
        {
            if (entities == null) {
                entities = ScaffoldUtility.LoadJSON<TEntity>().AsQueryable<TEntity>();
            }

            var mockSet = new Mock<DbSet<TEntity>>();
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

            MockSet = mockSet;

            MockContext.Setup(c => c.Set<TEntity>()).Returns(mockSet.Object);
        }

        protected override DbSet<TEntity> GetDbSet()
        {
            return MockSet.Object;
        }

        protected override IFacetContext GetContext()
        {
            return MockContext.Object;
        }

        protected List<TEntity> LoadJSON(int count = 0)
        {
            return ScaffoldUtility.LoadJSON<TEntity>(count);
        }

        protected TEntity LoadSingleJSON(int index = 0)
        {
            return ScaffoldUtility.LoadSingleJSON<TEntity>(index);
        }


    }
}
