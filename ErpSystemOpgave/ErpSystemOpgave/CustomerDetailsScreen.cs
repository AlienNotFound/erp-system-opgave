using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class CustomerDetailsScreen : Screen
{
    public int customerId = CustomerListScreen.SelectedId;

    public override string Title { get; set; } = "Kunde detaljer";
    protected override void Draw()
    {
        DataBase db = DataBase.Instance;
        CustomerListScreen customerListScreen = new CustomerListScreen();
        ListPage<Customer> listPage = new ListPage<Customer>();
        db.GetAllCustomers();

        // db.InsertCustomer(
        //     "Bob",
        //     "Bobsen",
        //     new Address("Vejgade Alle", "28B", "Herrens Mark", 1234, "Lalaland"),
        //     new ContactInfo("88888888", "test@mail.com")
        // );
        // db.InsertCustomer(
        //     "Søren",
        //     "Sørensen",
        //     new Address("Østre-nøresøndergade", "2. sal t.v", "Beyond Herrens Mark", 1234, "Lalaland"),
        //     new ContactInfo("12341234", "test2@mail.com")
        // );
        Clear(this);
        Customer? customer = db.GetCustomerFromId(customerId);

        listPage.Add(customer!);

        listPage.AddColumn("Navn", "FirstName");
        listPage.AddColumn("Adresse", "FullAddress", customer?.FullAddress.Length + 3 ?? 10);
        listPage.AddColumn("Sidste køb", "LastPurchase");
        listPage.Draw();

        ConsoleKey key;
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Backspace)
        {
            Screen.Display(customerListScreen);
        }
    }
}