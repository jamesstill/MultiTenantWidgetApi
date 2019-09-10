using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantWidgetApi.Models;

namespace MultiTenantWidgetApi.DataStore
{
    public class WidgetMap : IEntityTypeConfiguration<Widget>
    {
        public void Configure(EntityTypeBuilder<Widget> builder)
        {
            builder.ToTable("Widget");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Shape)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
