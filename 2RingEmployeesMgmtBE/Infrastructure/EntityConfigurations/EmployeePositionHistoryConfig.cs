using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    class EmployeePositionHistoryConfig : IEntityTypeConfiguration<EmployeePositionHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeePositionHistory> builder)
        {
            builder.HasOne(h => h.Position).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
