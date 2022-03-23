using System.Security.AccessControl;

namespace ErpSystemOpgave.Data;

public class SalesOrderLine
{
    public int ProductId  { get; set; }
    public double Amount { get; set; } // angivet i enhed svarende til produktet
    public decimal TotalPrice => (decimal) 0.0; // TODO: calculate total price based on the product and amount
}