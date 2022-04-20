using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        Clear(this);
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCustomers(),
            ("Kundenummer", "CustomerId"),
            ("Navn", "FullName"),
            ("Contact", "ContactInfo"));
        
        Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer\n");
        if (listPage.Select() is Customer selected)
        {
            Screen.Clear();
            Screen.Display(new CustomerDetailsScreen(selected.CustomerId));
        }
    }
}