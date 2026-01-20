using InvestmentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentOrders.Infrastructure.Persistence.Configurations;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("OrderStatus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DescripcionEstado)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasData(
            new OrderStatus(1, "En proceso"),
            new OrderStatus(2, "Ejecutada"),
            new OrderStatus(3, "Cancelada")
        );
    }
}

