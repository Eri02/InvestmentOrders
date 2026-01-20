using InvestmentOrders.Application.DTOs.Orders;

namespace InvestmentOrders.Application.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<ResponseOrderDto>> GetAllAsync();
    Task<ResponseOrderDto> GetByIdAsync(Guid id);
    Task<ResponseOrderDto> CreateAsync(CreateOrderDto dto);
    Task DeleteAsync(Guid id);
    Task ChangeStatusAsync(Guid orderId, int newStatusId);
}
