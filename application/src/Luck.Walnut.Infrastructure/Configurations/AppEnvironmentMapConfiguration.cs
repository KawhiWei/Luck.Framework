using Luck.Walnut.Domain.AggregateRoots.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Walnut.Infrastructure.Configurations
{
    public class AppEnvironmentMapConfiguration : IEntityTypeConfiguration<AppEnvironment>
    {
        public void Configure(EntityTypeBuilder<AppEnvironment> builder)
        {
            builder.ToTable("environment");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EnvironmentName);
            builder.Property(e => e.ApplicationId).HasMaxLength(95);
            builder.HasIndex(e => e.EnvironmentName);
            builder.HasMany(o => o.Configurations).WithOne();
            //var navigation = builder.Metadata.FindNavigation(nameof(AppEnvironment.Configurations));
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Property);

        }
    }
}
