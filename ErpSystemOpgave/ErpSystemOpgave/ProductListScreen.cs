using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        //ProductNumber, Name, StockUnits, BuyPrice, SalesPrice, AvancePercent
        Clear(this);
        ListPage<ProductList> listPage = new ListPage<ProductList>();

        listPage.Add(new ProductList(001, "Gaffel med meget", 100, 10, 50, 50));
        listPage.Add(new ProductList(002, "Tallerken med lidt", 350, 20, 100, 55));
        listPage.Add(new ProductList(003, "Kop fuld af tom", 200, 5, 20, 10));
        
        listPage.AddColumn("Varenr.", "ProductNumber");
        listPage.AddColumn("Produktnavn", "Name");
        listPage.AddColumn("Lagerantal", "StockUnits");
        listPage.AddColumn("Købspris", "BuyPrice");
        listPage.AddColumn("Salgspris", "SalesPrice");
        listPage.AddColumn("Avance i procent", "AvancePercent");

        listPage.Select();
    }
}