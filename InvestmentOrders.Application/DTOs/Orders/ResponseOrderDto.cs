using InvestmentOrders.Domain.Enums;

namespace InvestmentOrders.Application.DTOs.Orders
{
    public class ResponseOrderDto
    {
        public Guid Id { get; set; }
        public Guid InvestorId { get; set; }

        public int AssetId { get; set; }
        public string AssetTicker { get; set; }
        public string AssetName { get; set; }

        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderType OrderType { get; set; }
    }

}
