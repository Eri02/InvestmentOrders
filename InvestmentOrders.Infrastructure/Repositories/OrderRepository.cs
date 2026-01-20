using InvestmentOrders.Application.Interfaces.Repositories;
using InvestmentOrders.Domain.Entities;
using InvestmentOrders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvestmentOrders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly InvestmentDbContext _context;

    public OrderRepository(InvestmentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
        .Include(o => o.Asset)
        .Include(o => o.Status)
        .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
        .Include(o => o.Asset)
        .Include(o => o.Status)
        .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AssetExistsAsync(int assetId)
    {
        return await _context.Assets.AnyAsync(a => a.Id == assetId);
    }

    public async Task<Asset?> GetAssetByIdAsync(int assetId)
    {
        return await _context.Assets
            .Include(a => a.AssetType)
            .FirstOrDefaultAsync(a => a.Id == assetId);
    }


}
