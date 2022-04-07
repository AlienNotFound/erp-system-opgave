﻿using ErpSystemOpgave.Data;
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
        DataBase db = DataBase.Instance;
        CustomerListScreen customerListScreen = new CustomerListScreen();
        ListPage<Customer> listPage = new ListPage<Customer>();

        Clear(this);
        Customer customer = db.GetCustomerFromId(CustomerId) ?? throw new Exception("Invalid customer ID");

        listPage.Add(customer!);

        listPage.AddColumn("Navn", "FirstName");
        listPage.AddColumn("Adresse", "Address");
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