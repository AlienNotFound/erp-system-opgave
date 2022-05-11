using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        DataBase db = DataBase.Instance;
        CustomerListScreen customerListScreen = new();
        Clear(this);
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCustomers(),
            ("Kundenummer", "CustomerId"),
            ("Navn", "FullName"),
            ("Telefonnummer", "PhoneNumber"),
            ("Email", "Email"));

        Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer" +
                          "\nTryk på F1 for at oprette en ny kunde" +
                          "\nTryk på F2 for at redigere en kunde" +
                          "\nTryk på F5 for at slette en kunde");
        listPage.AddKey(ConsoleKey.F1, c =>
        {
            Clear(this);
            Customer customer = new Customer();
            var addCustomer = new EditScreen<Customer>("Opret ny kunde", customer,
                ("Fornavn", "FirstName"),
                ("Efternavn", "LastName"),
                ("Vej", "Address.Street"),
                ("Husnummer", "Address.HouseNumber"),
                ("Postnummer", "Address.ZipCode"),
                ("By", "Address.City"),
                ("Land", "Address.Country"),
                ("Telefonnummer", "ContactInfo.PhoneNumber"),
                ("Email", "ContactInfo.Email")
            );
            var addedCustomer = addCustomer.Show();
            
            db.InsertCustomer(
                addedCustomer.FirstName,
                addedCustomer.LastName,
                addedCustomer.Address.Street,
                addedCustomer.Address.HouseNumber,
                addedCustomer.Address.City,
                addedCustomer.Address.ZipCode,
                addedCustomer.Address.Country,
                addedCustomer.ContactInfo.PhoneNumber,
                addedCustomer.ContactInfo.Email!);
            Display(customerListScreen);
        });
        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear(this);
            if (db.GetCustomerById(c.CustomerId) is Customer cu)
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
            Display(customerListScreen);
        });
        listPage.AddKey(ConsoleKey.F5, c =>
        {
            Console.WriteLine("Du er ved at slette kunde: " + c.FullName + ". Dette kan ikke fortrydes." + 
                              "\nTryk på ENTER for at forsætte");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                db.DeleteCustomerById(c.CustomerId);
                Clear(this);
                Display(customerListScreen);
            }
            else if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                Clear(this);
                Quit();
            }
        });

        if (listPage.Select() is not { } selected) {
            if(!listPage.redraw)
                Quit();
            return;
        }
        Clear();
        Display(new CustomerDetailsScreen(selected.CustomerId));
    }
}