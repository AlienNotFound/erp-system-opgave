using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class CustomerDetailsScreen : Screen
{
    public int CustomerId = CustomerListScreen.SelectedId;

    public override string Title { get; set; } = "Kunde detaljer";


    public CustomerDetailsScreen(int customerId)
    {
        CustomerId = customerId;
    }
    protected override void Draw()
    {
        Clear(this);
        DataBase db = DataBase.Instance;
        CustomerListScreen customerListScreen = new CustomerListScreen();
        ListPage<Customer> listPage = new ListPage<Customer>();
        Customer customer = db.GetCustomerFromId(CustomerId) ?? throw new Exception("Invalid customer ID");

        listPage.Add(customer);
        listPage.AddColumn("Navn", "FirstName");
        listPage.AddColumn("Adresse", "Address", customer.FullAddress.Length + 3);
        listPage.AddColumn("Sidste køb", "LastPurchase");
        listPage.Draw();
        
        Console.WriteLine("\nTryk på BACKSPACE for at vende tilbage til kundelisten");

        ConsoleKey key;
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Backspace)
        {
            Screen.Display(customerListScreen);
        }
    }
}