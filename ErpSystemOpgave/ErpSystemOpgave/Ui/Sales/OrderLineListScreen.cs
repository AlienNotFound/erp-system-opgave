using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class OrderLineListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Ordre";
    private ListPage<SalesOrderLine> listPage;
    private int OrderNumber;
    private List<SalesOrderLine> rows = new();

    public OrderLineListScreen(int orderNumber)
    {
        OrderNumber = orderNumber;
        listPage = Refresh();

    }

    private void CreateOrderLine()
    {
        Console.WriteLine("adding {0}", OrderNumber);
        Console.ReadKey();
        var product = Program.CreateListPageWith(
            DataBase.Instance.GetAllProducts(),
            ("ID", "ProductId"),
            ("Navn", "Name"),
            ("På lager", "InStock"),
            ("Enhed", "Unit"),
            ("Pris", "SalePrice")).Select();
        EditScreen<SalesOrderLine> editscreen = new(
            $"Tilføj {product.Name} til ordren {OrderNumber}",
            SalesOrderLine.FromProduct(product, 0),
            ("Produkt", "ProductName"),
            ("Pris", "Price"),
            ("Antal", "Quantity"));
        if (editscreen.Show() is SalesOrderLine ol)
        {
            Console.WriteLine("adding {0}", OrderNumber);
            Console.ReadKey();
            DataBase.Instance.InsertOrderLine(product.ProductId, ol.Quantity, ol.Price, OrderNumber);
            listPage = Refresh();
        }
        else
        {
            Console.WriteLine("didn't find order in db: {0}", OrderNumber);
            Console.ReadKey();
        }

    }

    private ListPage<SalesOrderLine> Refresh()
    {
        rows = DataBase.Instance.GetOrderLinesByHeader(OrderNumber).ToList();
        // if (!rows.Any())
        //     return new();
        var lp = Program.CreateListPageWith(rows,
        ("Produkt", "ProductName"),
        ("Mængde", "Amount"),
        ("Enhedspris", "Price"),
        ("Total", "TotalPrice"));
        lp.AddKey(ConsoleKey.F1, o => CreateOrderLine());
        return lp;
    }

    protected override void Draw()
    {
        Clear();
        Console.WriteLine("Enter\t\tInspicer Ordrelinie\nF1\t\tTilføj ny Ordrelinje\nF5\t\tSlet Ordrelinje");
        if (!rows.Any())
        {
            Clear();
            var confirm = false;
            var dialog = new Menu<bool>($"Der er ingen ordrelinjer for {OrderNumber}.\nVil du oprette en?");
            dialog.InputFields.Add(new Button("Fuck ja!", () => { confirm = true; dialog.Done = true; }));
            dialog.InputFields.Add(new Button("næ.", () => { dialog.Done = true; }));
            dialog.Show();
            if (confirm)
                CreateOrderLine();
        }
        else if (listPage.Select() is { } orderLine)
        {
            Console.WriteLine("bet you expected a details view, huh?");
            Console.ReadKey();
        }
    }
}