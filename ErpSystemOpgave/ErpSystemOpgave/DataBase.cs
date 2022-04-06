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
        List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        List<SalesOrderLine> salesOrderLines = new List<SalesOrderLine>();
        
        public void GetAllProducts()
        {
              string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
              SqlConnection connection = new SqlConnection(connectionString);
              connection.Open();

              SqlDataReader dt;
              SqlCommand cmd = new SqlCommand("SELECT * FROM Products", connection);
              dt = cmd.ExecuteReader();
              
              ListPage<ProductDetails> listPage = new ListPage<ProductDetails>();
              while (dt.Read())
              {
                  listPage.Add(new ProductDetails(
                      Convert.ToInt32(dt["id"]),
                      dt["name"].ToString(),
                      dt["details"].ToString(),
                      Convert.ToInt32(dt["instock"]),
                      Convert.ToDecimal(dt["buyprice"]),
                      Convert.ToDecimal(dt["saleprice"]),
                      dt["location"].ToString(),
                      Convert.ToDecimal(dt["saleprice"]),
                      dt["unit"].ToString(),
                      Convert.ToDouble(dt["avancepercent"]),
                      Convert.ToDouble(dt["avancekroner"])));
              }
              
              listPage.AddColumn("Varenr.", "ProductNumber");
              listPage.AddColumn("Produktnavn", "Name");
              listPage.AddColumn("Lagerantal", "StockUnits");
              listPage.AddColumn("Købspris", "BuyPrice");
              listPage.AddColumn("Salgspris", "SalesPrice");
              listPage.AddColumn("Avance i procent", "AvancePercent");
              listPage.AddColumn("Avance i kroner", "AvanceKroner");
              listPage.Select();
              
              connection.Close();
        }
        public void GetProductById(int ProductID)
        {
            string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlDataReader dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM products WHERE ID = @id", connection);
            cmd.Parameters.AddWithValue("@id", ProductID);
            dt = cmd.ExecuteReader();

            ListPage<ProductDetails> listPage = new ListPage<ProductDetails>();
            while (dt.Read())
            {
                listPage.Add(new ProductDetails(
                    Convert.ToInt32(dt["id"]),
                    dt["name"].ToString(),
                    dt["details"].ToString(),
                    Convert.ToInt32(dt["instock"]),
                    Convert.ToDecimal(dt["buyprice"]),
                    Convert.ToDecimal(dt["saleprice"]),
                    dt["location"].ToString(),
                    Convert.ToDecimal(dt["saleprice"]),
                    dt["unit"].ToString(),
                    Convert.ToDouble(dt["avancepercent"]),
                    Convert.ToDouble(dt["avancekroner"])));
            }
            
            listPage.AddColumn("Varenr.", "ProductNumber");
            listPage.AddColumn("Produktnavn", "Name");
            listPage.AddColumn("Lagerantal", "StockUnits");
            listPage.AddColumn("Købspris", "BuyPrice");
            listPage.AddColumn("Salgspris", "SalesPrice");
            listPage.AddColumn("Avance i procent", "AvancePercent");
            listPage.AddColumn("Avance i kroner", "AvanceKroner");
            listPage.Draw();
            connection.Close();
        }

        public void InsertProduct(string name, string description, decimal saleprice, decimal buyprice, double instock, string location, string unit, decimal avancepercent, decimal avancekroner)
        {
            string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO products (name, details, instock, buyprice, saleprice, location, unit, avancepercent, avancekroner) VALUES (@name, @details, @instock, @buyprice, @saleprice, @location, @unit, @avancepercent, @avancekroner)", connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@details", description);
            cmd.Parameters.AddWithValue("@instock", instock);
            cmd.Parameters.AddWithValue("@buyprice", buyprice);
            cmd.Parameters.AddWithValue("@saleprice", saleprice);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@avancepercent", avancepercent);
            cmd.Parameters.AddWithValue("@avancekroner", avancekroner);
            cmd.ExecuteNonQuery();
            
            Console.WriteLine("Data tilføjet");
            connection.Close();
        }

        public void UpdateProduct(int id, string name, string description, decimal saleprice, decimal buyprice, double instock, string location, string unit, decimal avancepercent, decimal avancekroner)
        {
            string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("UPDATE products SET name = @name, details = @details, instock = @instock, buyprice = @buyprice, saleprice = @saleprice, location = @location, unit = @unit, avancepercent = @avancepercent, avancekroner = @avancepercent WHERE id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@details", description);
            cmd.Parameters.AddWithValue("@instock", instock);
            cmd.Parameters.AddWithValue("@buyprice", buyprice);
            cmd.Parameters.AddWithValue("@saleprice", saleprice);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@avancepercent", avancepercent);
            cmd.Parameters.AddWithValue("@avancekroner", avancekroner);
            cmd.ExecuteNonQuery();
            
            Console.WriteLine("Data opdateret");
            connection.Close();
        }
        
        public void DeleteProduct(int id)
        {
            string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM products WHERE id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            
            Console.WriteLine("Data slettet");
            connection.Close();
        }
        
        /*public List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,20, new List<SalesOrderLine>()));*/
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

        

        /*public void GetAllSalesOrders()
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
        }*/

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
    }
}
