using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Surname).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(50);
            builder.Property(x => x.Birthday).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.Salary).IsRequired();
            builder.Property(x => x.IsArchived).IsRequired();

            builder.HasMany(e => e.PositionHistory).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Position).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
