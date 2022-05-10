﻿using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class CustomerListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Kunde liste";

    protected override void Draw()
    {
        Clear(this);
        var listPage = Program.CreateListPageWith(
            DataBase.Instance.GetAllCustomers(),
            ("Kundenummer", "CustomerId"),
            ("Navn", "FullName"),
            ("Contact", "ContactInfo"));

        Console.WriteLine("\nTryk på ENTER på den valgte kunde, for at se detaljer\nTryk F2 for at redigere kunde");
        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear();
            Display(new CustomerUpdateScreen("Updater kunde", c.CustomerId));
        });

        if (listPage.Select() is Customer selected)
        {
            Clear();
            Display(new CustomerDetailsScreen(selected.CustomerId));
        }
    }
}