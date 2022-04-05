using System;

namespace ErpSystemOpgave.Data;

public enum ProductUnit
{
    Hours,
    Meters,
    Kilos,
    Quantity,
}
public class Product
{
    public Product(
        int productId,
        string name,
        string? description,
        decimal salePrice,
        decimal buyPrice,
        double inStock,
        string location,
        ProductUnit unit)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        SalePrice = salePrice;
        BuyPrice = buyPrice;
        InStock = inStock;
        Location = location;
        Unit = unit;
    }

    public int ProductId { get; set; }
    public String Name { get; set; }
    public String? Description { get; set; }
    public Decimal SalePrice { get; set; }
    public Decimal BuyPrice { get; set; }
    public double InStock { get; set; }
    public String Location { get; set; } // What is this even? spec siger "*Lokation er nummer på 4 bogstaver/tal"
    public ProductUnit Unit { get; set; }
}

