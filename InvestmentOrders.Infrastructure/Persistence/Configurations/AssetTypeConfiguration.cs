using InvestmentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvestmentOrders.Infrastructure.Persistence.Configurations;

public class AssetTypeConfiguration : IEntityTypeConfiguration<AssetType>
{
    public void Configure(EntityTypeBuilder<AssetType> builder)
    {
        builder.ToTable("AssetTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasData(
            new AssetType(1, "Acción"),
            new AssetType(2, "Bono"),
            new AssetType(3, "FCI")
        );
    }
}
