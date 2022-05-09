using System;
using System.Data;
using System.Data.SqlClient;

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
        var offset = 0;
        return FromReader(reader, ref offset);
    }
    public static Product FromReader(IDataReader reader, ref int offset)
    {
        return new(
                reader.GetInt32(offset++),
                reader.GetString(offset++),
                reader.GetString(offset++),
                reader.GetDecimal(offset++),
                reader.GetDecimal(offset++),
                reader.GetDouble(offset++),
                reader.GetString(offset++),
                Enum.Parse<ProductUnit>(reader.GetString(offset++))
        // reader.GetDecimal(8),
        // reader.GetDecimal(9)
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
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal SalePrice { get; set; }
    public decimal BuyPrice { get; set; }
    public double InStock { get; set; }
    public string Location { get; set; } // What is this even? spec siger "*Lokation er nummer på 4 bogstaver/tal"
    public ProductUnit Unit { get; set; }

    public SqlParameter[] SqlParameters => new SqlParameter[] {
        new("@id", ProductId),
        new("@name", Name),
        new("@description", Description),
        new("@instock", InStock),
        new("@buyprice", BuyPrice),
        new("@saleprice", SalePrice),
        new("@location", Location),
        new("@unit", Unit.ToString())
    };

    //TODO: these probably shouldn't have a backing field. we can just use a plain getter.
    public decimal AvancePercent => AvanceKroner / SalePrice * 100;
    public decimal AvanceKroner => SalePrice - BuyPrice;
}

