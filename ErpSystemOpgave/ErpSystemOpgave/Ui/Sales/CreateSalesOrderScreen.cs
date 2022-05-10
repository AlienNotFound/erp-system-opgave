using ErpSystemOpgave.Data;
using TECHCOOL;
using TECHCOOL.UI;
namespace ErpSystemOpgave;

public class CreateSalesOrderScreen : Screen
{
    public CreateSalesOrderScreen()
    {

    }

    protected override void Draw()
    {
        DataBase db = DataBase.Instance;
        Clear(this);

        //Mock-up for at få idéen ud. Men kan angive kunde ud fra Id og derefter tilføje ét produkt.
        //Nu skal man derfter gerne kunde tilføje flere produkter

        /*Console.Write("Angiv kundenummer: ");
        int customerId = 0;
        Int32.TryParse(Console.ReadLine(), out customerId);
        
        var customer = db.GetCustomerById(customerId);
        db.InsertSalesOrderHeader(customerId, "Created", 0, DateTime.Now);
        
        int productId = 0;
        decimal productQuantity = 0;

        Console.WriteLine("Indtast det ønskede produkt nummer:");
        Int32.TryParse(Console.ReadLine(), out productId);

        Console.WriteLine("Indtast det ønskede antal:");
        decimal.TryParse(Console.ReadLine(), out productQuantity);
        
        db.InsertOrderLine(productId, Convert.ToInt32(productQuantity));
        
        var product = db.GetProductById(productId);

        Console.WriteLine("Angivede oplysnigner:");
        Console.WriteLine("Kunde: " + customer.FullName);
        Console.WriteLine("Produkt: " + product.Name);
        Console.WriteLine("Antal: " + productQuantity);
        Console.WriteLine("Samlet pris: " + productQuantity * product.SalePrice);*/

        // SalesOrderHeader salesOrderHeader = new SalesOrderHeader(0,0, 0, 0, DateTime.Now, "", "", "", 0, "");
        // SalesOrderHeader salesOrderHeader = new SalesOrderHeader(0, 0, 0, 0, DateTime.Now);
        // var addSalesOrderHeader = new EditScreen<SalesOrderHeader>("Add SalesOrderHeader", salesOrderHeader,
        //     ("Kunde nummer", "CustomerId"),
        //     ("Status", "State")
        // );
        // var addedS = addSalesOrderHeader.Show();
        // var customer = db.GetCustomerById(addedS.CustomerId);

        // db.InsertSalesOrderHeader(addedS.CustomerId, addedS.State.ToString(), addedS.CreationTime);
        // Clear(this);

        // SalesOrderLine salesOrderLine = new SalesOrderLine(0, 0, "", "", 0, 0);
        // var addOrderLine = new EditScreen<SalesOrderLine>("Add orderline", salesOrderLine,
        //     ("Produkt nummer", "ProductId"),
        //     ("Antal", "Quantity")
        // );
        // var addedOL = addOrderLine.Show();
        // var product = db.GetProductById(addedOL.ProductId);

        // db.InsertOrderLine(addedOL.ProductId, addedOL.Quantity);

        // Console.WriteLine("Angivede oplysnigner:");
        // Console.WriteLine("Kunde: " + customer.FullName);
        // Console.WriteLine("Produkt: " + product.Name);
        // Console.WriteLine("Antal: " + addedOL.Quantity);
        // Console.WriteLine("Samlet pris: " + product.SalePrice * addedOL.Quantity);
    }

    public override string Title
    {
        get { return "Opret salgsordre"; }
        set { }
    }
}