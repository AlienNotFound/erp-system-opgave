using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;


public class CustomerUpdateScreen : Screen
{
    public override string Title { get; set; }
    private int customer_id;
    private EditScreen<Customer> editScreen;
    private DataBase db;

    public CustomerUpdateScreen(string title, int customer_id)
    {
        Title = title;
        this.customer_id = customer_id;

    }

    protected override void Draw()
    {
        db = DataBase.Instance;
        Customer customer = db.GetCustomerFromId(customer_id) ?? throw new Exception("Invalid customer ID " + customer_id);
        editScreen = new(
            "Opdater kunde", customer,
            ("first name", "FirstName"),
            ("last name", "LastName"),
            ("Vej", "Address.Street"),
            ("Husnummer", "Address.HouseNumber"),
            ("Postnummer", "Address.ZipCode"),
            ("By", "Address.City"),
            ("Telefonnummer", "ContactInfo.PhoneNumber"),
            ("Email", "ContactInfo.Email")
        );
        editScreen.Show();
        db.UpdateCustomer(customer.CustomerId,
            customer.FirstName,
            customer.LastName, 
            customer.Address.Street,
            customer.Address.HouseNumber,
            customer.Address.City,
            customer.Address.ZipCode,
            customer.Address.Country,
            customer.ContactInfo.PhoneNumber,
            customer.ContactInfo.Email!
        );
        
        Console.WriteLine(customer.FullName + " blev opdateret!");
        Console.WriteLine("Tryk på BACKSPACE for at vende tilbage til kundelisten");
    }
}

record UpdateItem(string Name, string Value);