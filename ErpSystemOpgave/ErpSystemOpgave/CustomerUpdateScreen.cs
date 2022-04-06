namespace ErpSystemOpgave;

using ErpSystemOpgave.Data;
using TECHCOOL.UI;



public class CustomerUpdateScreen : Screen
{
    public override string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    private Customer customer;
    private ListPage<Customer> listpage = new();

    public CustomerUpdateScreen(string title, int customer_id)
    {
        Title = title;
        var db = DataBase.Instance;
        customer = db.GetCustomerFromId(customer_id) ?? throw new Exception("Invalid customer ID " + customer_id);
    }

    protected override void Draw()
    {
        System.Console.WriteLine("Change field");
        listpage.Select();
    }
}

record UpdateItem(string Name, string Value);