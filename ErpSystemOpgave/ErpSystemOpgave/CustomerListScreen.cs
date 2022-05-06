using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        LandingPage landingPage = new LandingPage();
        Clear(this);
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCustomers(),
            ("Kundenummer", "CustomerId"),
            ("Navn", "FullName"),
            ("Telefonnummer", "PhoneNumber"),
            ("Email", "Email"));

        Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer");
        /*listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear(this);
            if (DataBase.Instance.GetCustomerById(c.CustomerId) is Customer cu)
            {
                if (new EditScreen<Customer>("Rediger kundeoplysninger for " + c.FullName, cu,
                        ("first name", "FirstName"),
                        ("last name", "LastName"),
                        ("Vej", "Address.Street"),
                        ("Husnummer", "Address.HouseNumber"),
                        ("Postnummer", "Address.ZipCode"),
                        ("By", "Address.City"),
                        ("Telefonnummer", "ContactInfo.PhoneNumber"),
                        ("Email", "ContactInfo.Email")).Show() is Customer updated)
                {
                    DataBase.Instance.UpdateCustomer(
                        updated.CustomerId,
                        updated.FirstName,
                        updated.LastName,
                        updated.Address.Street,
                        updated.Address.HouseNumber,
                        updated.Address.City,
                        updated.Address.ZipCode,
                        updated.Address.Country,
                        updated.ContactInfo.PhoneNumber,
                        updated.ContactInfo.Email!);
                }
            };
            Clear(this);
            //Display(new CustomerUpdateScreen("Updater kunde", c.CustomerId));
        });
        */

        if (listPage.Select() is Customer selected)
        {
            Clear();
            Display(new CustomerDetailsScreen(selected.CustomerId));
        }
    }
}