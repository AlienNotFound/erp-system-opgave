using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        DataBase db = DataBase.Instance;
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

        CustomerDetailsScreen customerDetailsScreen = new CustomerDetailsScreen();

        if (selected != null)
        {
            Screen.Display(customerDetailsScreen);
        }
    }
}