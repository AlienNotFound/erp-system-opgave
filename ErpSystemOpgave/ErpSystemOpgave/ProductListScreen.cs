using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Produkter";

    protected override void Draw()
    {
        // TODO: Change to pull data from Products
        Clear(this);
        try
        {
            var products = DataBase.Instance.GetAllProducts();
            System.Console.WriteLine("products: {0}", products.Count());
        }
        catch
        {
            throw;
        }
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllProducts(),
            ("ID", "ProductId"),
            ("Navn", "Name"),
            ("På lager", "InStock"),
            ("Enhed", "Unit"),
            ("Plads", "Location"),
            ("Salgspris", "SalePrice"),
            ("Købspris", "BuyPrice"),
            ("Avance", "AvanceKroner"),
            ("Avance procent", "AvancePercent"));

        Console.WriteLine("\nTryk på ENTER på det valgte produkt, for at se detaljer\nTryk F2 for at redigere kunde");
        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear();
            if (DataBase.Instance.GetProductById(c.ProductId) is Product p)
                if (new EditScreen<Product>("Rediger Produkt", p,
                    ("Navn", "Name"),
                    ("På lager", "InStock"),
                    ("Enhed", "Unit"),
                    ("Plads", "Location"),
                    ("Salgspris", "SalePrice"),
                    ("Købspris", "BuyPrice")).Show() is Product updated)
                {
                    DataBase.Instance.UpdateProduct(updated.ProductId, updated.Name, updated.Description, updated.SalePrice, updated.BuyPrice, updated.InStock, updated.Location, updated.Unit.ToString(), updated.AvancePercent, updated.AvancePercent);
                }
            // Display(new CustomerUpdateScreen("Updater produkt", c.ProductId));
        });

        listPage.AddKey(ConsoleKey.F3, _ =>
        {
            Clear();
            new EditScreen<Product>("Rediger Produkt", p,
                ("Navn", "Name"),
                ("På lager", "InStock"),
                ("Enhed", "Unit"),
                ("Plads", "Location"),
                ("Salgspris", "SalePrice"),
                ("Købspris", "BuyPrice")).Show();
            // Display(new CustomerUpdateScreen("Updater produkt", c.ProductId));
        });

        if (listPage.Select() is Product selected)
        {
            Clear();
            if (DataBase.Instance.GetProductById(selected.ProductId) is Product p)
                Display(new ProductDetailScreen(p));
        }
    }
}