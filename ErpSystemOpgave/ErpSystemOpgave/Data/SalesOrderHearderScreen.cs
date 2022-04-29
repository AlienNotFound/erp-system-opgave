using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using TECHCOOL.UI;
namespace ErpSystemOpgave.Data;

public class SalesOrderHearderScreen : Screen
{
    public override string Title { get; set; } = "Salgsordrehoved";

    protected override void Draw()
    {
        DataBase db = new DataBase();
        /*List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        List<Customer> customers = new List<Customer>();*/

        Clear(this);
        ListPage<Customer> customerListPage = new ListPage<Customer>();
        foreach (var customer in db.GetAllCustomers())
        {
            customerListPage.Add(customer);
        }

        ListPage<SalesOrderHeader> listPage = new ListPage<SalesOrderHeader>();
        foreach (var salesOrder in db.GetAllSalesOrderHeaders())
        {
            listPage.Add(salesOrder);
        }
        
        listPage.AddColumn("Ordrenummer", "OrderNumber");
        listPage.AddColumn("Dato", "CreationTime", 25);
        listPage.AddColumn("Kunde id", "CustomerId");
        listPage.AddColumn("Kunde navn", "CustomerName");
        listPage.AddColumn("Pris", "Price");
        listPage.Draw();
        customerListPage.Draw();
    }
}