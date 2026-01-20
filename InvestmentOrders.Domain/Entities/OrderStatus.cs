namespace InvestmentOrders.Domain.Entities; 
public class OrderStatus
{
    public int Id { get; private set; }
    public string DescripcionEstado { get; private set; }

    private OrderStatus() { }

    public OrderStatus(int id, string descriptionEstado)
    {
        Id = id;
        DescripcionEstado = descriptionEstado;
    }
}
