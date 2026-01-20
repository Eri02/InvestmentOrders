using InvestmentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentOrders.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.AssetId)
                .IsRequired();

        builder.Property(o => o.OrderStatusId)
               .IsRequired();

        builder.Property(o => o.Price)
               .HasPrecision(18, 4);

        builder.Property(o => o.TotalAmount)
               .HasPrecision(18, 4);

        builder.HasOne(o => o.Asset)
               .WithMany()
               .HasForeignKey(o => o.AssetId);

        builder.HasOne(o => o.Status)
               .WithMany()
               .HasForeignKey(o => o.OrderStatusId);

        builder.Property(o => o.OrderType)
                .HasConversion<int>()
                .IsRequired();
    }
}
