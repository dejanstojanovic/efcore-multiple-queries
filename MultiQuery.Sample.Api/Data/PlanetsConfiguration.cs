using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MultiQuery.Sample.Api.Data
{
    public class PlanetsConfiguration : IEntityTypeConfiguration<Planet>
    {
        public void Configure(EntityTypeBuilder<Planet> builder)
        {
            builder.ToTable("Planets", "Database2.dbo");
            builder.HasKey(e => e.Id);
            builder.Property(a => a.Name).IsRequired();
        }
    }
}
