using InvestmentOrders.Domain.Enums;

namespace InvestmentOrders.Application.DTOs.Orders;

public class CreateOrderDto
{
    public Guid InvestorId { get; set; }
    public int AssetId { get; set; }
    public OrderType OrderType { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
