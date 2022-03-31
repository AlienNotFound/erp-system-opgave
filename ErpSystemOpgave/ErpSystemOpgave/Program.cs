using System;
using TECHCOOL.UI;
using ErpSystemOpgave;
using static System.Console;

namespace ErpSystemOpgave;
record MenuItem(string Description, Action Action);

public static class Program
{
    public static void Main(string[] args)
    {

    }
    public static void ShowMenu(params (String Description, Action Action)[] items)
    {
        var menu = new ListPage<MenuItem>();
        menu.AddColumn("Action:", "Description");
        foreach (var (desc, action) in items)
        {
            menu.Add(new MenuItem(desc, action));
        }
        menu.Select().Action.Invoke();
    }
}
/* Har bare haft stået for sig selv da jeg mergede? where dis go
DataBase db = new DataBase();

Customer cus1 = new Customer();

WriteLine(db.GetAllCustomers());*/