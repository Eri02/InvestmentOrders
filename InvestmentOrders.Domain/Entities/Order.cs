using InvestmentOrders.Domain.Enums;

namespace InvestmentOrders.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }

    public Guid InvestorId { get; private set; }
    public int AssetId { get; private set; }
    public Asset Asset { get; private set; }
    public int OrderStatusId { get; private set; }
    public OrderStatus Status { get; private set; }

    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderType OrderType { get; private set; }

    private Order() { }

    public Order(Guid investorId, int assetId, int orderStatusId, int quantity, decimal price, OrderType orderType, decimal totalAmount)
    {
        Id = Guid.NewGuid();
        InvestorId = investorId;
        AssetId = assetId;
        OrderStatusId = orderStatusId;
        Quantity = quantity;
        Price = price;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
        OrderType = orderType;
    }

    public void Update(int quantity, decimal price)
    {
        Quantity = quantity;
        Price = price;
        TotalAmount = quantity * price;
    }

    public void ChangeStatus(int newStatusId)
    {
        OrderStatusId = newStatusId;
    }
}
