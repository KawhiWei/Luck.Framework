using Luck.Walnut.Domain.AggregateRoots.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luck.Walnut.Persistence
{
    public class AppEnvironmentMapConfiguration : IEntityTypeConfiguration<AppEnvironment>
    {
        public void Configure(EntityTypeBuilder<AppEnvironment> builder)
        {
            builder.ToTable("environments");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EnvironmentName);
            builder.Property(e => e.ApplicationId).HasMaxLength(95);
            builder.HasIndex(e => e.EnvironmentName);
            builder.HasMany(o => o.Configurations).WithOne().HasForeignKey(x=>x.AppEnvironmentId).OnDelete(DeleteBehavior.Cascade);
            //var navigation = builder.Metadata.FindNavigation(nameof(AppEnvironment.Configurations));
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Property);

        }
    }
}
