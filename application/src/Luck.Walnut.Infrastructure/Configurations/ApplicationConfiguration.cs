using Luck.Walnut.Domain.AggregateRoots.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Luck.Walnut.Infrastructure.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("application");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.EnglishName);
            builder.Property(e => e.DepartmentName);
            builder.Property(e => e.ChinessName);
            builder.Property(e => e.LinkMan).IsRequired(false);
            builder.Property(e => e.AppId);
            builder.Property(e => e.Status);
            builder.HasIndex(e => e.AppId).IsUnique();

        }
    }
}
