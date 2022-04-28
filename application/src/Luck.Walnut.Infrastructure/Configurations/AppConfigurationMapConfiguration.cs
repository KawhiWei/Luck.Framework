using Luck.Walnut.Domain.AggregateRoots.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Luck.Walnut.Infrastructure.Configurations
{
    public class AppConfigurationMapConfiguration : IEntityTypeConfiguration<AppConfiguration>
    {
        public void Configure(EntityTypeBuilder<AppConfiguration> builder)
        {
            builder.HasKey(x => x.Id);
         
            builder.ToTable("configuration");
            builder.Property(x => x.Key);
            builder.Property(x => x.Value);
            builder.Property(x => x.Type);
            builder.Property(x => x.IsOpen);
            builder.Property(x => x.IsPublish);
            builder.Property<string>("EnvironmentId").HasMaxLength(95).IsRequired();
            builder.HasIndex(x => new { x.Key,x.Value});
        }
    }
}
