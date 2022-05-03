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
            builder.Property(x => x.Key);
            builder.Property(x => x.Value).HasColumnType("longtext");
            builder.Property(x => x.Type);
            builder.Property(x => x.IsOpen);
            builder.Property(x => x.IsPublish);
            ////todo 不这样，当Remove时候，会把AppEnvironmentId清空
            //builder.Property<string>("AppEnvironmentId").IsRequired().HasMaxLength(95);
            builder.HasIndex(x => new { x.Key, x.Value });

            builder.ToTable("configurations");
        }
    }
}
