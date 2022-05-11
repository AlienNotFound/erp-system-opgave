using System;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class CustomerDetailsScreen : Screen
{
    public int CustomerId = CustomerListScreen.SelectedId;
    public override string Title { get; set; } = "Kunde detaljer";

    public CustomerDetailsScreen(int customerId)
    {
        CustomerId = customerId;
    }
    private string FormatDate(object date)
    {
        return ((DateTime)date).ToString("dd MMMM yyyy");
    }
    protected override void Draw()
    {
        Clear(this);
        DataBase db = DataBase.Instance;
        CustomerListScreen customerListScreen = new();

        Clear(this);

        if (db.GetCustomerById(CustomerId) is not Customer customer)
        {
            System.Console.WriteLine("Den valgte kunde kunde ikke findes.");
            return;
        }
        
        ListPage<Customer> listPage = new();
        listPage.Add(customer);
        listPage.AddColumn("Navn", "FullName", customer.FullName.Length + 3);
        listPage.AddColumn("Adresse", "FullAddress", customer.FullAddress.Length + 3);
        listPage.AddColumn("Sidste køb", "LastPurchase", customer.LastPurchase.ToString()!.Length + 3, FormatDate);
        listPage.Draw();

        Console.WriteLine("\nTryk på BACKSPACE for at vende tilbage til kundelisten");

        ConsoleKey key;
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Backspace)
        {
            Clear(this);
            Quit();
        }
        
        Program.CreateDetailsView(customer,
            ("Navn", "FullName"),
            ("Adresse", "Address"),
            ("Sidste kob", "LastPurchase"))
            .Draw();
    }
}