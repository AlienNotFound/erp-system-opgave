namespace ErpSystemOpgave;

using ErpSystemOpgave.Data;
using TECHCOOL.UI;



public class CustomerUpdateScreen : Screen
{
    public override string Title { get; set; }
    private Customer customer;
    private int customer_id;
    private ListPage<Customer> listpage = new();
    private DataBase db;

    public CustomerUpdateScreen(string title, int customer_id)
    {
        Title = title;
        db = DataBase.Instance;
        this.customer_id = customer_id;
        customer = db.GetCustomerFromId(customer_id) ?? throw new Exception("Invalid customer ID " + customer_id);
    }

    protected override void Draw()
    {
        System.Console.WriteLine("Change field");
        Program.ShowMenu(
            ($"Fornavn: {customer.FirstName}", () =>
            {
                string? newname = null;
                while (newname is null)
                    newname = Console.ReadLine();
                customer.FirstName = newname;
                //db.UpdateCustomer(customer_id, customer);
            }
        ),
            ($"Efternavn: {customer.LastName}", () =>
            {
                string? newname = null;
                while (newname is null)
                    newname = Console.ReadLine();
                customer.LastName = newname;
                db.UpdateCustomer(customer_id, customer);
                Screen.Clear();
            }
        ),
            ($"Tilbage", () => Screen.Display(new CustomerListScreen()))
        );
    }
}

record UpdateItem(string Name, string Value);