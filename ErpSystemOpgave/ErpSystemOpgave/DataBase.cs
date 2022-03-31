using System;
using System.Collections.Generic;
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

        List<Customer> customers = new List<Customer>();
        public void CreateCustomerList()
        {
            customers.Add(new Customer("Svend","Aage", new Address("Duevej", "2", "Fugleby", 0001, "Airbornia"), "12345678", "svend@aage.com", 1));
        }
        public void GetCustomerFromId(int CustomerId)
        {
            foreach (var customer in customers)
            {
                if (customer.CustomerId == CustomerId)
                {
                    Console.WriteLine("Kunde id: " + customer.CustomerId 
                                        + "\nKunde navn: " + customer.FirstName + " " + customer.LastName );
                }
            }
        }
        public void GetAllCustomers()
        {
            if(customers.Count == 0)
            {
                Console.WriteLine("Der er ingen kunder");
            }
            else
            {
                for (int i = 0; i < customers.Count; i++)
                {
                    Console.WriteLine("Kunde id: " + customers[i].CustomerId
                                                   + "\nKunde navn: " + customers[i].FirstName + " " + customers[i].LastName
                                                   + "\nAdresse: " + customers[i].Address.Street + " " + customers[i].Address.HouseNumber + " " + customers[i].Address.ZipCode + " " + customers[i].Address.City
                                                   + "\nTelefon: " + customers[i].PhoneNumber
                                                   + "\nEmail: " + customers[i].Email
                    );
                }                
            }
            
        }   

        public void InsertCustomer(string firstName, string lastName, string street, string houseNumber, string city, short zipCode, string country, string phoneNumber, string email)
        {
            int lastId = customers.Last().CustomerId;
            customers.Add(new Customer(firstName, lastName, new Address(street, houseNumber, city, zipCode, country), phoneNumber, email, lastId+1));
            
            Console.WriteLine("\nTilføjede ny kunde: "
                            + "\nKunde navn: " + firstName + " " + lastName
                            + "\nAdresse: " + street + " " + houseNumber + " " + zipCode + " " + city
                            + "\nTelefon: " + phoneNumber
                            + "\nEmail: " + email
                            );
        }
        public void UpdateCustomer(int CustomerId, string firstName, string lastName, string street, string houseNumber, string city, short zipCode, string country, string phoneNumber, string email)
        {
            var result = from c in customers
                where c.CustomerId == CustomerId
                select c;
            foreach (var customer in result)
            {
                
                customer.FirstName = firstName;
                customer.LastName = lastName;
                customer.Address.Street = street;
                customer.Address.HouseNumber = houseNumber;
                customer.Address.City = city;
                customer.Address.ZipCode = zipCode;
                customer.PhoneNumber = phoneNumber;
                customer.Email = email;
                
                Console.WriteLine("\nOpdateret: ");
                Console.WriteLine("Kunde id: " + customer.CustomerId);
                Console.WriteLine("Fornavn: " + customer.FirstName);
                Console.WriteLine("Efternavn: " + customer.LastName);
                Console.WriteLine("Vej: " + customer.Address.Street);
                Console.WriteLine("Husnummer: " + customer.Address.HouseNumber);
                Console.WriteLine("Postnummer: " + customer.Address.ZipCode);
                Console.WriteLine("By: " + customer.Address.City);
                Console.WriteLine("Telefonnr.: " + customer.PhoneNumber);
                Console.WriteLine("Email: " + customer.Email);
            }
        }

        public void DeleteCustomerFromId(int CustomerId)
        {
            customers.RemoveAll(c => c.CustomerId == CustomerId);
        }
    }
}
