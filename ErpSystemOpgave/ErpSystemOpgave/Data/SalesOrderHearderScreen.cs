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
        salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
        
        List<Customer> customerHeader = new List<Customer>(); //Til når vi har en liste af kunder, som så kan strikkes sammen med salgsordrehovederne

        Clear(this);
        ListPage<SalesOrderHeader> listPage = new ListPage<SalesOrderHeader>();
        foreach (var salesOrder in salesOrderHeaders)
        {
            listPage.Add(salesOrder);
        }
        
        listPage.AddColumn("Ordrenummer", "OrderNumber");
        listPage.AddColumn("Dato", "CreationTime", 25);
        listPage.AddColumn("Kunde id", "CustomerId");
        //listPage.AddColumn("Kunde navn", "CustomerName");
        listPage.AddColumn("Pris", "Price");
        listPage.Draw();
    }
}