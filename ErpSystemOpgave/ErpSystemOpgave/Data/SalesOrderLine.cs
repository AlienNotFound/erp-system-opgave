using System;
using System.Security.AccessControl;

namespace ErpSystemOpgave.Data;

public class SalesOrderLine
{
    public static SalesOrderLine FromProduct(Product product, decimal amount)
    {
        return new SalesOrderLine(product.SalePrice, product.Name, product.Description, product.Unit, amount);
    }
    public SalesOrderLine(decimal price, string productName, string? productDescription, ProductUnit unit, decimal amount)
    {
        Price = price;
        ProductName = productName;
        ProductDescription = productDescription;
        Unit = unit;
        Amount = amount;
    }

    public decimal TotalPrice => Price *  Amount;
    public decimal Price { get; set; }
    public String ProductName { get; set; }
    public String? ProductDescription { get; set; }
    public ProductUnit Unit { get; set; }
    public decimal Amount { get; set; } 
}
