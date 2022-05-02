using ErpSystemOpgave.Data;
using TECHCOOL;
using TECHCOOL.UI;
namespace ErpSystemOpgave;

public class CreateSalesOrderScreen : Screen
{
    protected override void Draw()
    {
        DataBase db = DataBase.Instance;
        Clear(this);

        //Mock-up for at få idéen ud. Man skal kunne angive hvilken kunne der køber, hvad de køber og hvor mange.
        //Der skal kunne tilføjes flere produkter
        Console.Write("Angiv kundenummer: ");
        int customerId = 0;
        Int32.TryParse(Console.ReadLine(), out customerId);
        
        var customer = db.GetCustomerFromId(customerId);

        Console.WriteLine("Indtast det ønskede produkt nummer:");
        int productId = 0;
        Int32.TryParse(Console.ReadLine(), out productId);
        var product = db.GetProductById(productId);

        Console.WriteLine("Indtast det ønskede antal:");
        double productQuantity = 0;
        Double.TryParse(Console.ReadLine(), out productQuantity);

        Console.WriteLine("Angivede oplysnigner:");
        Console.WriteLine("Kunde: " + customer.FullName);
        Console.WriteLine("Produkt: " + product.Name);
        Console.WriteLine("Antal: " + productQuantity);
        
        return;
        SalesOrderHeader salesOrderHeader = new SalesOrderHeader(0,0, 0, 0, DateTime.MinValue);
        var editScreen = new EditScreen<SalesOrderHeader>("Edit SalesOrderHeader", salesOrderHeader!,
             ("Ordre nummer", "OrderNumber"),
             ("Kunde nummer", "CustomerId"),
             ("Ordre status", "State"),
             ("Pris", "Price"),
             ("Dato", "CreationTime")
        );
        editScreen.Show();
    }

    public override string Title
    {
        get { return "Opret salgsordre"; } set {} }
    
}