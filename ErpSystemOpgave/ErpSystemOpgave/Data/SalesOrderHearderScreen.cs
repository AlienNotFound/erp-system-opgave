using TECHCOOL.UI;
namespace ErpSystemOpgave.Data;

public class SalesOrderHearderScreen : Screen
{
    private readonly SalesOrderHeader _order;
    public override string Title { get; set; } = "Salgsordrehoved";

    public SalesOrderHearderScreen(int orderId) {
        _order = DataBase.Instance.GetSalesOrderById(orderId) ?? throw new Exception("Invalid order ID");
    }

    protected override void Draw()
    {
        Clear(this);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Ordrenummer:", _order.OrderNumber);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Kunde:", _order.Customer);
        Console.WriteLine("{0,-30} \x1b[32m{1} {2}\x1b[0m", "Adresse:", _order.Street, _order.HouseNumber);
        Console.WriteLine("{0,-30} \x1b[32m{1} {2}\x1b[0m", "By:", _order.City, _order.ZipCode);
        Console.WriteLine("{0,-30} \x1b[32m{1}\x1b[0m", "Pris: ", _order.Price);
        Quit();
    }
}