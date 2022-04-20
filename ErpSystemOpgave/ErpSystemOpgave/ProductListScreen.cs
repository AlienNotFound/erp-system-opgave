using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        Clear(this);
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllProducts(),
            ("Varenummer", "ProductId"),
            ("Produkt", "Name"),
            ("Lagerantal", "InStock"),
            ("Indkøbspris", "BuyPrice"),
            ("Salgspris", "SalePrice"),
            ("Avance i procent", "AvancePercent")
            );

            Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer\n");
        if (listPage.Select() is Product selected)
            Screen.Display(new ProductDetailScreen(selected));
    }
}