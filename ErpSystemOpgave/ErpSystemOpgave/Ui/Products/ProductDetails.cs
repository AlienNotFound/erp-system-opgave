namespace ErpSystemOpgave;

public class ProductDetails
{
    public int ProductNumber { get; set; }
    public string Name { get; set; } = "";
    public string Details { get; set; } = "";
    public int StockUnits { get; set; }
    public decimal BuyPrice { get; set; }
    public decimal SalesPrice { get; set; }
    public string Location { get; set; }
    public decimal StockUnitsDecimal { get; set; } 
    public  string Unit { get; set; }
    public double AvancePercent { get; set; }
    public double AvanceKroner { get; set; }

    public ProductDetails(int productNumber, string name, string details, int stockUnits, decimal buyPrice, decimal salesPrice,
        string location, decimal stockUnitsDecimal, string unit, double avancePercent, double avanceKroner)
    {
        ProductNumber = productNumber;
        Name = name;
        Details = details;
        StockUnits = stockUnits;
        BuyPrice = buyPrice;
        SalesPrice = salesPrice;
        Location = location;
        StockUnitsDecimal = stockUnitsDecimal;
        Unit = unit;
        AvancePercent = avancePercent;
        AvanceKroner = avanceKroner;
    }
}