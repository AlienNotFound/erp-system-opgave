using System;
using System.Data;

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
    public static Product FromReader(IDataReader reader)
    {
        return new(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetDecimal(3),
                reader.GetDecimal(4),
                reader.GetDouble(5),
                reader.GetString(6),
                Enum.Parse<ProductUnit>(reader.GetString(7)),
                reader.GetDecimal(8),
                reader.GetDecimal(9)
        );
    }

    public Product(
        int productId,
        string name,
        string? description,
        decimal salePrice,
        decimal buyPrice,
        double inStock,
        string location,
        ProductUnit unit,
        decimal avancePercent,
        decimal avanceKroner)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        SalePrice = salePrice;
        BuyPrice = buyPrice;
        InStock = inStock;
        Location = location;
        Unit = unit;
        AvancePercent = avancePercent;
        AvanceKroner = avanceKroner;
    }

    public int ProductId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal BuyPrice { get; set; }
    public double InStock { get; set; }
    public string Location { get; set; } // What is this even? spec siger "*Lokation er nummer på 4 bogstaver/tal"
    public ProductUnit Unit { get; set; }

    //TODO: these probably shouldn't have a backing field. we can just use a plain setter.
    public decimal AvancePercent { get; set; }
    public decimal AvanceKroner { get; set; }
}

