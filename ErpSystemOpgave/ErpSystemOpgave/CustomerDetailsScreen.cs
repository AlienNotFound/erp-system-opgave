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

        listPage.Add(customer);
        listPage.AddColumn("Navn", "FirstName");
        listPage.AddColumn("Adresse", "Address", customer.FullAddress.Length + 3);
        listPage.AddColumn("Sidste køb", "LastPurchase");
        listPage.Draw();

        Console.WriteLine("\nTryk på BACKSPACE for at vende tilbage til kundelisten");

        ConsoleKey key;
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Backspace)
            // ListPage<Customer> listPage = new ListPage<Customer>();

            Program.CreateDetailsView(customer,
            ("Navn", "FullName"),
            ("Adresse", "Address"),
            ("Sidste kob", "LastPurchase"))
            .Draw();


        // ConsoleKey key;
        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.Backspace:
                Screen.Display(customerListScreen);
                break;
            case ConsoleKey.F2:
                Screen.Display(new CustomerUpdateScreen($"Opdater {customer.FullName}", customer.CustomerId));
                break;
        }
    }
}