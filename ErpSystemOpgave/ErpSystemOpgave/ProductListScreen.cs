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
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCustomers(),
            ("Kundenummer", "CustomerId"),
            ("Navn", "FullName"),
            ("Contact", "ContactInfo"));

        Console.WriteLine("\nTryk på ENTER på det valgte produkt, for at se detaljer\nTryk F2 for at redigere kunde");
        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear();
            Display(new CustomerUpdateScreen("Updater produkt", c.CustomerId));
        });

        if (listPage.Select() is Customer selected)
        {
            Clear();
            Display(new CustomerDetailsScreen(selected.CustomerId));
        }
    }
}