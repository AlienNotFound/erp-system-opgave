namespace ErpSystemOpgave;

using ErpSystemOpgave.Data;
using TECHCOOL.UI;



public class CustomerUpdateScreen : Screen
{
    public override string Title { get; set; }
    private int customer_id;
    private EditScreen<Customer> editScreen;
    private DataBase db;

    public CustomerUpdateScreen(string title, int customer_id)
    {
        Title = title;
        db = DataBase.Instance;
        this.customer_id = customer_id;
        Customer customer = db.GetCustomerFromId(customer_id) ?? throw new Exception("Invalid customer ID " + customer_id);
        editScreen = new(
            "Opdater kunde", customer,
            ("first name", "FirstName"),
            ("last name", "LastName"),
            ("Vej", "Address.Street"),
            ("Nr.", "Address.HouseNumber"),
            ("By", "Address.City"),
            ("Phone", "ContactInfo.PhoneNumber"),
            ("Mail", "ContactInfo.Email"));
        editScreen.Show();
        Quit();
    }

    protected override void Draw() { }
}

record UpdateItem(string Name, string Value);