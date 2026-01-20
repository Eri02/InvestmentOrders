using InvestmentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentOrders.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Ticker)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(x => x.UnitPrice)
               .HasPrecision(18, 4);

        builder.HasOne(x => x.AssetType)
               .WithMany()
               .HasForeignKey(x => x.AssetTypeId);

        builder.HasData(
            new Asset(1, "AAPL", "Apple", 177.97m, 1),
            new Asset(2, "GOOGL", "Alphabet Inc", 138.21m, 1),
            new Asset(3, "MSFT", "Microsoft", 329.04m, 1),
            new Asset(4, "KO", "Coca Cola", 58.3m, 1),
            new Asset(5, "WMT", "Walmart", 163.42m, 1),
            new Asset(6, "AL30", "BONOS ARGENTINA USD 2030 L.A", 307.4m, 2),
            new Asset(7, "GD30", "Bonos Globales Argentina USD Step Up 2030", 336.1m, 2),
            new Asset(8, "Delta.Pesos", "Delta Pesos Clase A", 0.0181m, 3),
            new Asset(9, "Fima.Premium", "Fima Premium Clase A", 0.0317m, 3)
        );
    }
}
