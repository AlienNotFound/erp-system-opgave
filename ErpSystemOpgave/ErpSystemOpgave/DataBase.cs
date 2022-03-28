using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TECHCOOL.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemOpgave.Data;

namespace ErpSystemOpgave
{
    public class DataBase
    {
        public List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        //salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,20, new List<SalesOrderLine>()));
        public SalesOrderHeader GetSalesOrderById(int orderId)
        {
            var Order = salesOrderHeaders.Find(id => id.OrderNumber == orderId);
            Console.WriteLine("Ordre nummer: " + Order.OrderNumber
                                               + " Kundeid: " + Order.CustomerId
                                               + " Status: " + Order.State
                                               + " Pris: " + Order.Price
                                               //+ " Ordrelinje: " + salesOrderHeaders[orderId].OrderLines
                                               );
            return Order;
        }

        public void GetAllSalesOrders()
        {
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));

            for (int i = 0; i < salesOrderHeaders.Count; i++)
            {
                Console.WriteLine("Ordre nummer: " + salesOrderHeaders[i].OrderNumber
                                                   + " Kundeid: " + salesOrderHeaders[i].CustomerId
                                                   + " Status: " + salesOrderHeaders[i].State
                                                   + " Pris: " + salesOrderHeaders[i].Price
                                                   + " Oprettet: " + salesOrderHeaders[i].CreationTime
                );
            }
        }

        public void CreateSalesOrder(int OrderNumber, int CustomerId, decimal Price)
        {
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(OrderNumber, CustomerId, OrderState.Created, Price, new List<SalesOrderLine>()));
        }

        public void UpdateSalesOrder(int OrderNumber, int CustomerId, decimal Price)
        {
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            var result = from s in salesOrderHeaders
                where s.OrderNumber == OrderNumber
                select s;
            foreach (var salesOrder in result)
            {
                Console.WriteLine("\nOriginalt: ");
                Console.WriteLine("Kunde id: " + salesOrder.CustomerId);
                Console.WriteLine("Pris: " + salesOrder.Price);
                salesOrder.CustomerId = CustomerId;
                salesOrder.Price = Price;
                Console.WriteLine("\nOpdateret: ");
                Console.WriteLine("Kunde id: " + salesOrder.CustomerId);
                Console.WriteLine("Pris: " + salesOrder.Price);
            }
        }

        public void DeleteSalesOrder(int OrderNumber)
        {
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));

            salesOrderHeaders.RemoveAll(s => s.OrderNumber == OrderNumber);
        }
        public void GetAllProducts()
      {
          string connectionString = @"Server=ADAMS-BÆRBAR\MSSQLSERVER01;Database=erp_system;Trusted_Connection=True;";
          SqlConnection connection = new SqlConnection(connectionString);
          connection.Open();

          SqlDataReader dt;
          SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_productmodule", connection);
          dt = cmd.ExecuteReader();
          
          ListPage<Product> listPage = new ListPage<Product>();
          while (dt.Read())
          {
              listPage.Add(new Product(
                  Convert.ToInt32(dt["id"]),
                  dt["productname"].ToString(),
                  dt["details"].ToString(),
                  Convert.ToDecimal(dt["salesprice"]),
                  Convert.ToDecimal(dt["buyprice"]),
                  Convert.ToInt32(dt["stockunits"]),
                  dt["location"].ToString(),
                  ProductUnit.Meters
              ));
          }
          
          listPage.AddColumn("Varenr.", "ProductId");
          listPage.AddColumn("Produktnavn", "Name");
          listPage.AddColumn("Lagerantal", "InStock");
          listPage.AddColumn("Købspris", "BuyPrice");
          listPage.AddColumn("Salgspris", "SalePrice");
          //listPage.AddColumn("Avance i procent", "AvancePercent");
          
          //Prints the selected product name out in the console, after pressing enter. Preparation for P3 
          Screen.Display(new ProductDetailScreen(listPage.Select()));

          connection.Close();
      }
    }
}
