using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class ProductListScreen : Screen
{
    public override string Title { get; set; } = "Produkt liste";

    protected override void Draw()
    {
        //Guide: https://github.com/sinb-dev/TECHCOOL/tree/master/UI
        Clear(this);
        ListPage<ProductDetails> listPage = new ListPage<ProductDetails>();

        listPage.Add(new ProductDetails(001, "Gaffel med meget", "Lang beskrivelse", 100, 10, 50, "Hylde 5", 100, "Unit?",50, 25));
        listPage.Add(new ProductDetails(002, "Tallerken med lidt", "Lang beskrivelse", 350, 20, 100, "Hylde 2",350, "Unit?",55, 125));
        listPage.Add(new ProductDetails(003, "Kop fuld af tom", "Lang beskrivelse", 200, 5, 20, "Hylde 72",200,"Unit?",10, 100));
        
        listPage.AddColumn("Varenr.", "ProductNumber");
        listPage.AddColumn("Produktnavn", "Name");
        listPage.AddColumn("Lagerantal", "StockUnits");
        listPage.AddColumn("Købspris", "BuyPrice");
        listPage.AddColumn("Salgspris", "SalesPrice");
        listPage.AddColumn("Avance i procent", "AvancePercent");
        
        //Prints the selected product name out in the console, after pressing enter. Preparation for P3 
        Console.WriteLine("Valgte: " + listPage.Select().Name);
        
        /*Menu menu = new Menu();

        menu.Add(new ProductDetailScreen());
        menu.Start(this);*/
    }
}