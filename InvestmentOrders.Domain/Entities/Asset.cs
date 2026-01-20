namespace InvestmentOrders.Domain.Entities;
public class Asset
{
    public int Id { get; private set; }
    public string Ticker { get; private set; }
    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }

    public int AssetTypeId { get; private set; }
    public AssetType AssetType { get; private set; }

    private Asset() { }

    public Asset(int id, string ticker, string name, decimal unitPrice, int assetTypeId)
    {
        Id = id;
        Ticker = ticker;
        Name = name;
        UnitPrice = unitPrice;
        AssetTypeId = assetTypeId;
    }
}
