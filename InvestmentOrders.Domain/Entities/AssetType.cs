namespace InvestmentOrders.Domain.Entities;
public class AssetType
{
    public int Id { get; private set; }
    public string Description { get; private set; }

    private AssetType() { }

    public AssetType(int id, string description)
    {
        Id = id;
        Description = description;
    }
}
