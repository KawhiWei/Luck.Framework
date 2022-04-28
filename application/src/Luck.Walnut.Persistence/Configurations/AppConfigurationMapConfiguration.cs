using Luck.Walnut.Domain.AggregateRoots.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luck.Walnut.Persistence
{
    public class AppConfigurationMapConfiguration : IEntityTypeConfiguration<AppConfiguration>
    {
        public void Configure(EntityTypeBuilder<AppConfiguration> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("configurations");
            builder.Property(x => x.Key);
            builder.Property(x => x.Value);
            builder.Property(x => x.Type);
            builder.Property(x => x.IsOpen);
            builder.Property(x => x.IsPublish);

            builder.HasIndex(x => new { x.Key, x.Value });
        }
    }
}
