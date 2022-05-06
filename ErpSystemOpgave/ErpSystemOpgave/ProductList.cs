namespace ErpSystemOpgave;

public class ProductList
{
    public int ProductNumber { get; set; }
    public string Name { get; set; } = "";
    public int StockUnits { get; set; }
    public decimal BuyPrice { get; set; }
    public decimal SalesPrice { get; set; }
    public double AvancePercent { get; set; }

    public ProductList(int productNumber, string name, int stockUnits, decimal buyPrice, decimal salesPrice,
        double avancePercent)
    {
        ProductNumber = productNumber;
        Name = name;
        StockUnits = stockUnits;
        BuyPrice = buyPrice;
        SalesPrice = salesPrice;
        AvancePercent = avancePercent;
    }
}