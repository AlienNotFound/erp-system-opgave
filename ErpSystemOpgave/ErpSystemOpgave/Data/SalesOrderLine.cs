using System.Data;

namespace ErpSystemOpgave.Data;

public class SalesOrderLine
{
    public static SalesOrderLine FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }

    public static SalesOrderLine FromReader(IDataReader reader, ref int offset)
    {
        var id = reader.GetInt32(offset++);
        var productId = reader.GetInt32(offset++);
        var quantity = reader.GetInt32(offset++);
        var orderId = reader.GetInt32(offset++);
        var price = reader.GetDecimal(offset++);
        return new(id, productId, "", "", ProductUnit.Meters, price, quantity);
    }
    public static SalesOrderLine FromProduct(Product product, int quantity)
        => new(0, product.ProductId, product.Name, product.Description, product.Unit, product.SalePrice, quantity);

    public SalesOrderLine() { }
    public SalesOrderLine(
        int id,
        int productId,
        string productName,
        string? productDescription,
        ProductUnit unit,
        decimal price,
        int quantity)
    {
        Id = id;
        ProductId = productId;
        Price = price;
        var product = DataBase.Instance.GetProductById(productId);
        ProductName = product!.Name;
        ProductDescription = productDescription;
        Unit = unit;
        Quantity = quantity;
    }

    public int Id { get; set; } = default;
    public int ProductId { get; set; } = default;
    public decimal TotalPrice => Price * Quantity;
    public decimal Price { get; set; } = default;
    public string ProductName { get; set; } = "";
    public string? ProductDescription { get; set; } = "";
    public ProductUnit Unit { get; set; } = ProductUnit.Quantity;
    public int Quantity { get; set; } = default;

    public string Amount
    {
        get
        {
            var suffix = Unit switch
            {
                ProductUnit.Hours => " timer",
                ProductUnit.Meters => "m",
                ProductUnit.Kilos => "kg",
                ProductUnit.Quantity => " stk",
                _ => ""
            };
            return $"{Quantity}{suffix}";
        }

    }
}
