using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class OrderLineListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Ordre";
    private ListPage<SalesOrderLine> listPage;
    private int OrderNumber;

    public OrderLineListScreen(int orderNumber)
    {
        OrderNumber = orderNumber;
        listPage = Refresh(orderNumber);

        Console.WriteLine("create {0}", orderNumber);
        Console.ReadKey();
        listPage.AddKey(ConsoleKey.F1, o =>
        {
            Console.WriteLine("adding {0}", orderNumber);
            Console.ReadKey();
            var product = Program.CreateListPageWith(
                DataBase.Instance.GetAllProducts(),
                ("ID", "ProductId"),
                ("Navn", "Name"),
                ("På lager", "InStock"),
                ("Enhed", "Unit"),
                ("Pris", "SalePrice")).Select();
            EditScreen<SalesOrderLine> editscreen = new(
                $"Tilføj {product.Name} til ordren {orderNumber}",
                SalesOrderLine.FromProduct(product, 0),
                ("Produkt", "ProductName"),
                ("Pris", "Price"),
                ("Antal", "Quantity"));
            if (editscreen.Show() is SalesOrderLine ol)
            {
                Console.WriteLine("adding {0}", orderNumber);
                Console.ReadKey();
                DataBase.Instance.InsertOrderLine(product.ProductId, ol.Quantity, ol.Price, orderNumber);
                listPage = Refresh(orderNumber);
            }
            else
            {
                Console.WriteLine("didn't find order in db: {0}", orderNumber);
                Console.ReadKey();
            }

        });
    }

    private ListPage<SalesOrderLine> Refresh(int orderNumber)
    {
        var lines = DataBase.Instance.GetOrderLinesByHeader(orderNumber);
        return Program.CreateListPageWith(lines,
        ("Produkt", "ProductName"),
        ("Mængde", "Amount"),
        ("Enhedspris", "Price"),
        ("Total", "TotalPrice"));
    }

    protected override void Draw()
    {
        Clear();
        Console.WriteLine("Enter\t\tInspicer Ordrelinie\nF1\t\tTilføj ny Ordrelinje\nF5\t\tSlet Ordrelinje");
    }
}