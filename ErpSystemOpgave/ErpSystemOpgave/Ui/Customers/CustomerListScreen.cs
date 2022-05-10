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

        Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer");
        listPage.AddKey(ConsoleKey.F1, c =>
        {
            Clear(this);
            Customer customer = new Customer("","", new Address("", "", "",0, ""), new ContactInfo("", ""), 0, DateTime.MinValue);
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
            //Display(new CustomerUpdateScreen("Updater kunde", c.CustomerId));
        });
        
        /*switch (Console.ReadKey().Key)
        {
            case ConsoleKey.Backspace:
                Screen.Display(customerListScreen);
                break;
            case ConsoleKey.F1:
                Clear(this);
                Customer customer = new Customer("", "", null, null, 0, DateTime.MinValue);
                if (new EditScreen<Customer>("Opret ny kunde", customer,
                        ("Fornavn", "FirstName"),
                        ("Efternavn", "LastName"),
                        ("Vej", "Address.Street"),
                        ("Husnummer", "Address.HouseNumber"),
                        ("Postnummer", "Address.ZipCode"),
                        ("By", "Address.City"),
                        ("Telefonnummer", "ContactInfo.PhoneNumber"),
                        ("Email", "contactInfo.Email")).Show() is Customer created)
                {
                    db.InsertCustomer(
                        created.FirstName,
                        created.LastName,
                        created.Address.Street,
                        created.Address.HouseNumber,
                        created.Address.City,
                        created.Address.ZipCode,
                        created.Address.Country,
                        created.ContactInfo.PhoneNumber,
                        created.ContactInfo.Email!);
                }
                Display(customerListScreen);
                break;
            case ConsoleKey.F2:
                Clear(this);
                if (db.GetCustomerById(customer.CustomerId) is Customer cu)
                {
                    if (new EditScreen<Customer>("Rediger kundeoplysninger for " + customer.FullName, cu,
                            ("Fornavn", "FirstName"),
                            ("Efternavn", "LastName"),
                            ("Vej", "Address.Street"),
                            ("Husnummer", "Address.HouseNumber"),
                            ("Postnummer", "Address.ZipCode"),
                            ("By", "Address.City"),
                            ("Telefonnummer", "ContactInfo.PhoneNumber"),
                            ("Email", "ContactInfo.Email")).Show() is Customer updated)
                    {
                        db.UpdateCustomer(
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
                Display(customerListScreen);
                break;
        }*/

        if (listPage.Select() is Customer selected)
        {
            Clear();
            Display(new CustomerDetailsScreen(selected.CustomerId));
        }
    }
}