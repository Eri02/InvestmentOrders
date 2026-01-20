using InvestmentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestmentOrders.Infrastructure.Persistence;

public class InvestmentDbContext : DbContext
{
    public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options)
        : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<AssetType> AssetTypes => Set<AssetType>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvestmentDbContext).Assembly);
    }

}
