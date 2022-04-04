using TECHCOOL.UI;
using ErpSystemOpgave.Data;

namespace ErpSystemOpgave;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        DataBase db = new DataBase();
        CustomerDetailsScreen customerDetailsScreen = new CustomerDetailsScreen();

        db.InsertCustomer(
            "Bob",
            "Bobsen",
            new Address("Vejgade Alle", "28B", "Herrens Mark", 1234, "Lalaland"),
            new ContactInfo("88888888", "test@mail.com")
        );
        db.InsertCustomer(
            "Søren",
            "Sørensen",
            new Address("Østre-nøresøndergade", "2. sal t.v", "Beyond Herrens Mark", 1234, "Lalaland"),
            new ContactInfo("12341234", "test2@mail.com")
        );

        Clear(this);
        ListPage<Customer> listPage = new ListPage<Customer>();
        foreach (var customer in db.GetAllCustomers())
        {
            listPage.Add(customer);
        }

        listPage.AddColumn("Kundenummer", "CustomerId");
        listPage.AddColumn("Navn", "FullName");
        listPage.AddColumn("Telefon", "PhoneNumber");
        listPage.AddColumn("Email", "Email");
        Customer selected = listPage.Select();
        SelectedId = selected.CustomerId;

        if (selected != null)
        {
            Screen.Display(customerDetailsScreen);
        }
    }
}