using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Tests
{
    public abstract class InMemoryDbTest : IDisposable
    {
        protected EmployeesDbContext DbContext { get; }

        public InMemoryDbTest()
        {
            var dbInstanceName = Guid.NewGuid().ToString();
            var dbContextOptions = new DbContextOptionsBuilder<EmployeesDbContext>()
                .UseInMemoryDatabase(dbInstanceName)
                .Options;

            DbContext = new EmployeesDbContext(dbContextOptions);
            DbContext.Database.EnsureDeleted();
        }

        /// <summary>
        /// For test arrange - if we setup the DbContext state and SaveChanges(), entities remain attached/tracked by the context.
        /// This doesn't reflect the real case, where entites existing in Db are "detached"
        /// </summary>
        public void DetachAllEntites() =>
            DbContext.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
