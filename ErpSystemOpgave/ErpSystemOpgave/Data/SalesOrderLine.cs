namespace ErpSystemOpgave.Data;

public class SalesOrderLine
{
    public static SalesOrderLine FromProduct(Product product, int quantity)
    {
        return new SalesOrderLine(product.ProductId, product.SalePrice, product.Name, product.Description, product.Unit, quantity);
    }
    public SalesOrderLine(int productId, decimal price, string productName, string? productDescription, ProductUnit unit, int quantity)
    {
        ProductId = productId;
        Price = price;
        ProductName = productName;
        ProductDescription = productDescription;
        Product = DataBase.Instance.GetProductById(ProductId)!;
        Unit = unit;
        Quantity = quantity;
    }

    public int ProductId { get; set; }
    public decimal TotalPrice => Price *  Quantity;
    public decimal Price { get; set; }
    public string ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public Product Product { get; set; }
    public ProductUnit Unit { get; set; }
    public int Quantity { get; set; } 
}
