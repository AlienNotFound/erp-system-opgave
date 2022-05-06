using System;
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
        CustomerListScreen customerListScreen = new();

        Clear(this);

        if (db.GetCustomerFromId(CustomerId) is not Customer customer)
        {
            System.Console.WriteLine("ouper douper");
            return;
        }

        ListPage<Customer> listPage = new();
        Console.WriteLine("\nTryk F2 for at redigere kunde");
        listPage.Add(customer);
        listPage.AddColumn("Navn", "FullName", customer.FullName.Length + 3);
        listPage.AddColumn("Adresse", "FullAddress", customer.FullAddress.Length + 3);
        listPage.AddColumn("Sidste køb", "LastPurchase", customer.LastPurchase.ToString()!.Length + 3);
        listPage.Draw();

        Console.WriteLine("\nTryk på BACKSPACE for at vende tilbage til kundelisten");

        /*ConsoleKey key;
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Backspace)
            // ListPage<Customer> listPage = new ListPage<Customer>();

            Program.CreateDetailsView(customer,
            ("Navn", "FullName"),
            ("Adresse", "Address"),
            ("Sidste kob", "LastPurchase"))
            .Draw();*/
        /*listPage.AddKey(ConsoleKey.F2, c =>
        {*/
            
            //Display(new CustomerUpdateScreen("Updater kunde", c.CustomerId));
        //});

        // ConsoleKey key;
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.Backspace:
                Screen.Display(customerListScreen);
                break;
            case ConsoleKey.F2:
                Clear(this);
                if (db.GetCustomerById(CustomerId) is Customer cu)
                {
                    if (new EditScreen<Customer>("Rediger kundeoplysninger for " + customer.FullName, cu,
                            ("first name", "FirstName"),
                            ("last name", "LastName"),
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
        }
    }
}