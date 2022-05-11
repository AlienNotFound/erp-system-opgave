using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class ProductListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Produkter";

    protected override void Draw() {
        var refresh = false;
        Clear(this);
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

        Console.WriteLine(@"
Tryk på ENTER på det valgte produkt, for at se detaljer
Tryk F2 for at redigere produkt");
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
                    DataBase.Instance.UpdateProduct(p);
                    Clear();
                    listPage.Draw();
                }
            // Display(new CustomerUpdateScreen("Updater produkt", c.ProductId));
        });

        listPage.AddKey(ConsoleKey.F1, _ =>
        {
            Clear();
            if (new EditScreen<Product>("Tilføj Produkt", new Product(0, "", "", 0, 0, 0, "0000", ProductUnit.Quantity),
                ("Navn", "Name"),
                ("På lager", "InStock"),
                ("Enhed", "Unit"),
                ("Plads", "Location"),
                ("Salgspris", "SalePrice"),
                ("Købspris", "BuyPrice")).Show() is { } p)
            {
                DataBase.Instance.InsertProduct(p);
                Clear();
                listPage.Draw();
            }
        });

        //Delete product on F5
        listPage.AddKey(ConsoleKey.F5, c =>
        {
            DataBase.Instance.DeleteProduct(c.ProductId);
            Clear();
        });

        if (listPage.Select() is not { } selected) {
            Quit();
            return;
        }
        Clear();
        if (DataBase.Instance.GetProductById(selected.ProductId) is { } p)
            Display(new ProductDetailScreen(p));
    }
}