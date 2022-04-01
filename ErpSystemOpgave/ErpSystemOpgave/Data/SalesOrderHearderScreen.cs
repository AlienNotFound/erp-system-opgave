using System;
using Microsoft.VisualBasic;
using TECHCOOL.UI;
namespace ErpSystemOpgave.Data;

public class SalesOrderHearderScreen : Screen
{
    public override string Title { get; set; } = "Salgsordrehoved";

    protected override void Draw()
    {
        DataBase db = new DataBase();
        List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        List<Customer> customers = new List<Customer>();
        
        db.InsertCustomer(
            "Bob",
            "Bobsen",
            new Address("Vejgade Alle", "28B", "Herrens Mark", 1234, "Lalaland"),
            new ContactInfo("88888888", "test@mail.com")
        );
        db.InsertCustomer(
            "Søren",
            "Sørensen",
            new Address("Østre-nøresøndergade", "2. sal t.v", "Beyond Herrens Mark", 1234, "Lalaland"),
            new ContactInfo("12341234", "test2@mail.com")
        );
        
        db.CreateSalesOrder(2,1,200);
        db.CreateSalesOrder(3,0,200);
        
        Clear(this);
        ListPage<Customer> customerListPage = new ListPage<Customer>();
        foreach (var customer in db.GetAllCustomers())
        {
            customerListPage.Add(customer);
        }

        ListPage<SalesOrderHeader> listPage = new ListPage<SalesOrderHeader>();
        foreach (var salesOrder in db.GetAllSalesOrders())
        {
            listPage.Add(salesOrder);
        }
        
        listPage.AddColumn("Ordrenummer", "OrderNumber");
        listPage.AddColumn("Dato", "CreationTime", 25);
        listPage.AddColumn("Kunde id", "CustomerId");
        customerListPage.AddColumn("Kunde id", "CustomerId");
        customerListPage.AddColumn("Kunde navn", "FullName");
        listPage.AddColumn("Pris", "Price");
        listPage.Draw();
        customerListPage.Draw();
    }
}