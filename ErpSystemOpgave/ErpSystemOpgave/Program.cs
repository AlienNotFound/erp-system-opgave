using ErpSystemOpgave;
using ErpSystemOpgave.Data;
using TECHCOOL.UI;
using System;

namespace ErpSystemOpgave;
public class Program
{
    public static void Main(string[] args)
    {
        ProductListScreen productListScreen = new ProductListScreen();
        Screen.Display(productListScreen);
        
        // ProductDetailScreen detailScreen = new(
        //     new Product(
        //         42069,
        //         "Half a unicorn",
        //         "Unicorn is length 0.5???",
        //         (decimal)80085.0,
        //         (decimal)8008135.0,
        //         10.0,
        //         "n008",
        //         ProductUnit.Quantity));
        // Screen.Display(detailScreen);
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