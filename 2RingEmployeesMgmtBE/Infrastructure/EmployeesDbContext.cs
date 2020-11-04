using Domain.Models;
using Domain.Models.Interfaces;
using Domain.Services;
using Domain.Services.Enums;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EmployeesDbContext : DbContext, IUnitOfWork
    {
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePositionHistory> PositionHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasMany(e => e.PositionHistory).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EmployeePositionHistory>().HasOne(h => h.Position).WithMany().OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Employee>().HasOne(e => e.Position).WithMany().OnDelete(DeleteBehavior.SetNull);
            modelBuilder.SingularizeTableNames();
        }

        //IQueryable<T> IUnitOfWork.Query<T, TResult>(Expression<Func<T, TResult>> include)
        //{
        //    return Set<T>().Include(include);
        //}

        IQueryable<T> IUnitOfWork.Query<T>()
        {
            return Set<T>();
        }

        Task<int> IUnitOfWork.SaveChangesAsync()
        {
            // Unspecified = Invariant with regard to time zone (dayligt saving)
            var now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            ChangeTracker.Entries()
                         .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added)
                         .Select(x => x.Entity)
                         .OfType<ITrackLastModified>()
                         .ToList()
                         .ForEach(e => e.LastModified = now);

            try
            {
                return SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Task.FromResult(0);
            }
        }

        void IUnitOfWork.Add<T>(T entity)
        {
            Set<T>().Add(entity);
        }

        void IUnitOfWork.Update<T>(T entity)
        {
            Update(entity);
        }

        void IUnitOfWork.Delete<T>(T entity)
        {
            Remove(entity);
        }
        void IUnitOfWork.SetEntityState<T>(T entity, State entityState)
        {
            Entry(entity).State = (EntityState)entityState;
        }
    }
}