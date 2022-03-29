using System;
using TECHCOOL.UI;

namespace ErpSystemOpgave;

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
record MenuItem(string Description, Action Action);
