using InvestmentOrders.Domain.Entities;

namespace InvestmentOrders.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(Order order);
    Task<bool> AssetExistsAsync(int assetId);
    Task<Asset?> GetAssetByIdAsync(int assetId);
}
