using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemOpgave.Data;

namespace ErpSystemOpgave
{
    public class DataBase
    {
        private List<Customer> customers = new();
        public List<SalesOrderHeader> salesOrderHeaders = new();
        
        //HACK: Dette er blot for at simulere en IDENTITY på Customer mens vi ikke har en database
        private int _nextCustomerId;
        private int NextCustomerId => _nextCustomerId++;
        
        ////////////////////////////////////////////////////////////////////////////
        /////////////         Customer        //////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        public Customer? GetCustomerFromId(int customerId)
            => customers.FirstOrDefault(c => c.CustomerId == customerId);
        
        // Hvis vi blot returnerede en reference til _customers, ville consumeren kunne ændre i listen.
        // Med GetRange() returnerer vi en kopi af indholdet i stedet.
        public IEnumerable<Customer> GetAllCustomers()
            => customers.GetRange(0, customers.Count);
        
        public void InsertCustomer(
            string firstName,
            string lastName,
            Address address,
            ContactInfo contactInfo)
        {
            customers.Add(new Customer(
                firstName,
                lastName,
                address,
                contactInfo,
                NextCustomerId
            ));
        }

        public void UpdateCustomer(int customerId, Customer updatedCustomer) {
            if (customers.FindIndex(c => c.CustomerId == customerId) is var index && index != -1)
                customers[index] = updatedCustomer;
        }
        
        public void DeleteCustomerFromId(int customerId) {
            customers.RemoveAll(c => c.CustomerId == customerId);
        }
        
        ////////////////////////////////////////////////////////////////////////////
        /////////////         Products        //////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        
        
        
        ////////////////////////////////////////////////////////////////////////////
        /////////////         Orders          //////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////
        public void CreateSaleOrderList()
        {
            string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlDataReader dt;
            SqlCommand cmd = new SqlCommand("SELECT * FROM SaleOrders", connection);
            dt = cmd.ExecuteReader();

            foreach (var sale in dt)
            {

            }
            
            connection.Close();
        }
        public SalesOrderHeader GetSalesOrderById(int orderId)
        {
            var Order = salesOrderHeaders.Find(id => id.OrderNumber == orderId);
            Console.WriteLine("Ordre nummer: " + Order.OrderNumber
                                               + " Kundeid: " + Order.CustomerId
                                               + " Status: " + Order.State
                                               + " Pris: " + Order.Price
                                               + " Ordrelinje: " + salesOrderHeaders[orderId].OrderLines
            );
            return Order;
        }
        public IEnumerable<SalesOrderHeader> GetAllSalesOrders()
            => salesOrderHeaders.GetRange(0, salesOrderHeaders.Count);
        /*public void GetAllSalesOrders()
        {
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();

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
