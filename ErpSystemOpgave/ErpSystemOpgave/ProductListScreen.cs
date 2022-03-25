using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        //Guide: https://github.com/sinb-dev/TECHCOOL/tree/master/UI
        Clear(this);
        ListPage<Product> listPage = new ListPage<Product>();

        listPage.Add(new Product(001, "Gaffel med meget", "Lang beskrivelse", 100, 10, 50, "Hylde 5", ProductUnit.Hours));
        listPage.Add(new Product(002, "Tallerken med lidt", "Lang beskrivelse", 350, 20, 100, "Hylde 2", ProductUnit.Meters));
        listPage.Add(new Product(003, "Kop fuld af tom", "Lang beskrivelse", 200, 5, 20, "Hylde 72", ProductUnit.Kilos));

        listPage.AddColumn("Varenr", "ProductId");
        listPage.AddColumn("Produktnavn", "Name");
        listPage.AddColumn("Lagerantal", "InStock");
        listPage.AddColumn("Købspris", "BuyPrice");
        listPage.AddColumn("Salgspris", "SalePrice");
        // listPage.AddColumn("Avance i procent", "AvancePercent");

        //Prints the selected product name out in the console, after pressing enter. Preparation for P3 
        // Console.WriteLine("Valgte: " + listPage.Select().Name);

        //? Burde man gaa til "Rediger" frem for "vis" her?
        Screen.Display(new ProductDetailScreen(listPage.Select()));
        /*Menu menu = new Menu();

        menu.Add(new ProductDetailScreen());
        menu.Start(this);*/
    }
}