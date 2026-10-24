using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiClient.Cache.Entity
{
    public class StructuredRequestConfiguration : IEntityTypeConfiguration<StructuredRequest>
    {
        public void Configure(EntityTypeBuilder<StructuredRequest> builder)
        {
            builder.ToTable("CachedRequests");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Command)
                .HasMaxLength(RequestParameter.MaxNameLength)
                .IsRequired();

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            // EF Core 10: complex collection stored in a single JSON column.
            builder.ComplexCollection(r => r.Parameters, parameter =>
            {
                parameter.ToJson();
                parameter.Property(p => p.ValueType).HasConversion<string>();
            });
        }
    }
}

