using ErpSystemOpgave.Data;
using TECHCOOL.UI;

namespace ErpSystemOpgave.Ui;

public class OrderListScreen : Screen
{
    public static int SelectedId;
    public override string Title { get; set; } = "Ordre";
    private ListPage<SalesOrderHeader> listPage;

    public OrderListScreen()
    {
        listPage = Program.CreateListPageWith(DataBase.Instance.GetAllSalesOrderHeaders(),
            ("Ordrenummer", "OrderNumber"),
            ("Oprettet", "CreationTime"),
            ("Status", "State"),
            ("Kunde nummer", "CustomerId"),
            ("Navn", "Customer"),
            ("Pris", "Price"));

        listPage.AddKey(ConsoleKey.F2, c =>
        {
            Clear();
            if (DataBase.Instance.GetSalesOrderById(c.OrderNumber) is not SalesOrderHeader order)
            {
                Console.WriteLine("didn't find order in db: {0}", c);
                Console.ReadKey();
                return;
            }
            if (new OrderEditScreen(
                "Opdater ordre", order,
                ("Status", "State"),
                ("Pris", "Price")).Show() is SalesOrderHeader updateHeader)
                DataBase.Instance.UpdateSalesOrder(
                    updateHeader.OrderNumber,
                    updateHeader.CustomerId,
                    updateHeader.Price);
        });
        listPage.AddKey(ConsoleKey.F1, c =>
        {
            Clear();
            CreateOrder();
        });
        listPage.AddKey(ConsoleKey.F5, c =>
        {
            Clear();
            var confirm = false;
            var dialog = new Menu<bool>($"yeet order {c.OrderNumber}?");
            dialog.InputFields.Add(new Button("Yeet away", () => { confirm = true; dialog.Done = true; }));
            dialog.InputFields.Add(new Button("On second thought...", () => { dialog.Done = true; }));
            dialog.Show();
            if (confirm)
                DataBase.Instance.DeleteSalesOrder(c.OrderNumber);
        });
    }

    private void CreateOrder()
    {
        //? Show a menu that asks if the order should be made for an existing customer
        //? or if a new customer should be created.
        var menu = new Menu<bool?>("Opret for eksisterende kunde eller opret?");
        menu.InputFields.Add(new Button("Eksisterende kunde", () => { menu.ReturnValue = false; menu.Done = true; }));
        menu.InputFields.Add(new Button("Ny Kunde", () => { menu.ReturnValue = true; menu.Done = true; }));
        menu.InputFields.Add(new Button("Tilbage", () => { menu.Done = true; }));
        if (menu.Show() is not { } createNew) return;

        //? Open a page to create a customer or select one from the database, depending on the users choice
        var customer = createNew ? GetNewCustomer() : GetExistingCustomer();
        if (customer is null) return;

        //? Actually create the order, and present the user with an Update screen.
        //? This is because we need an order ID to add OrderLines (or lazy evaluate the entire thing, but that seems impractical)
        SalesOrderHeader order = new()
        {
            Customer = customer,
            CustomerId = customer.CustomerId,
            State = OrderState.Created,
            CreationTime = DateTime.Now
        };
        order.OrderNumber = DataBase.Instance.InsertSalesOrderHeader(customer.CustomerId, order.State.ToString(), order.CreationTime);

        if (new OrderEditScreen(
            "Tilføj ordre", order,
            ("Fornavn", "Customer.FirstName"),
            ("Efternavn", "Customer.FirstName"),
            ("Telefon", "Customer.ContactInfo.PhoneNumber"),
            ("Email", "Customer.ContactInfo.Email"),
            ("Vej", "Customer.Address.Street"),
            ("Husnummer", "Customer.Address.HouseNumber"),
            ("By", "Customer.Address.City"),
            ("Postnummer", "Customer.Address.ZipCode"),
            ("Status", "State")).Show() is SalesOrderHeader updated)
        {
            DataBase.Instance.UpdateSalesOrder(updated.OrderNumber, updated.CustomerId, 0);
        }
    }

    private Customer GetExistingCustomer()
    {
        return Program.CreateListPageWith(DataBase.Instance.GetAllCustomers(),
                        ("Kundenummer", "CustomerId"),
                        ("Navn", "FullName"),
                        ("Contact", "ContactInfo")).Select();
    }

    private Customer? GetNewCustomer()
    {
        var c = new EditScreen<Customer>("Ny Kunde", new(),
                        ("first name", "FirstName"),
                        ("last name", "LastName"),
                        ("Vej", "Address.Street"),
                        ("Nr.", "Address.HouseNumber"),
                        ("By", "Address.City"),
                        ("Phone", "ContactInfo.PhoneNumber"),
                        ("Mail", "ContactInfo.Email")).Show();
        if (c is not null)
            DataBase.Instance.InsertCustomer(
                c.FirstName, c.LastName,
                c.Address.Street, c.Address.HouseNumber, c.Address.City, c.Address.ZipCode, c.Address.Country,
                c.ContactInfo.PhoneNumber, c.ContactInfo.Email ?? "");
        return c;
    }

    protected override void Draw()
    {
        // TODO: Change to pull data from orders
        Clear(this);

        Console.WriteLine("\nTryk på ENTER på den valgte ordre, for at se detaljer\nTryk F2 for at redigere kunde");
        if (listPage.Select() is not { } selected) {
            Quit();
            return;
        }
        Clear();
        Display(new SalesOrderHearderScreen(selected.OrderNumber));
    }
}
