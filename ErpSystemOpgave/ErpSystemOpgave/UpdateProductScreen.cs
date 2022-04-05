using System.Data.SqlClient;
using System;
using System.Linq;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductUpdateScreen : Screen
{
    public override string Title { get => $"Update product: {_product.Name}"; set { } }

    private readonly Product _product;

    public ProductUpdateScreen(Product product)
    {
        _product = product;
    }
    
    protected override void Draw()
    {
        Clear(this);

        //Skriver hen over detaljerne fra ProductDetailScreen.
        //Dette er også kun et udkast, til redigering af produkt detaljer.
        Console.WriteLine("Navn: ");
        string productName = Console.ReadLine();
        Console.WriteLine("Beskrivelse: ");
        string details = Console.ReadLine();
        Console.Write("Salgspris: ");
        string salePrice = Console.ReadLine();
        
        UpdateProduct(_product.ProductId, productName, details, decimal.TryParse(salePrice, out decimal salePriceDecimal) ? salePriceDecimal : _product.SalePrice);
        //throw new NotImplementedException();
    }
    
    public void UpdateProduct(int Id, string ProductName, string Details, decimal SalePrice)
    {
        string connectionString = @"Server=ADAMS-BÆRBAR\MSSQLSERVER01;Database=erp_system;Trusted_Connection=True;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        
        SqlCommand cmd = new SqlCommand("UPDATE tbl_productmodule SET ProductName = @ProductName, Details = @Details, SalesPrice = @SalePrice WHERE Id = @Id", connection);
        cmd.Parameters.AddWithValue("@Id", Id);
        cmd.Parameters.AddWithValue("@ProductName", ProductName);
        cmd.Parameters.AddWithValue("@Details", Details);
        cmd.Parameters.AddWithValue("@SalePrice", SalePrice);
        cmd.ExecuteNonQuery();
        connection.Close();
    }
}