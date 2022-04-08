using System.Data.SqlClient;

namespace ErpSystemOpgave;
using System;
using System.Collections.Generic;
using System.Linq;
using Data;


public sealed class DataBase
{
    static DataBase? _instance = null;
    private DataBase() { }

    public static DataBase Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataBase();
            return _instance;
        }
    }
    private List<Customer> customers = new();
    public List<SalesOrderHeader> salesOrderHeaders = new();

    //HACK: Dette er blot for at simulere en IDENTITY på Customer mens vi ikke har en database
    private int _nextCustomerId;
    private int NextCustomerId => _nextCustomerId++;

    ////////////////////////////////////////////////////////////////////////////
    /////////////         Customer        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    // Hvis vi blot returnerede en reference til _customers, ville consumeren kunne ændre i listen.
    // Med GetRange() returnerer vi en kopi af indholdet i stedet.
    public IEnumerable<Customer> GetAllCustomers()
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        SqlDataReader dt;
        SqlCommand cmd = new SqlCommand(@"SELECT * FROM Customers
                                                    + INNER JOIN Addresses ON Addresses.Id = Customers.AddressId
                                                    + INNER JOIN Contacts ON Contacts.Id = Customers.AddressId", connection);
        dt = cmd.ExecuteReader();

        if (customers.Count == 0)
        {
            while (dt.Read())
            {
                customers.Add(new Customer(
                    dt["FirstName"].ToString(),
                    dt["LastName"].ToString(),
                    new Address(dt["Street"].ToString(),
                        dt["HouseNumber"].ToString(),
                        dt["City"].ToString(), 
                        short.Parse(dt["ZipCode"].ToString()),
                        dt["Country"].ToString()),
                    new ContactInfo(dt["PhoneNumber"].ToString(),
                        dt["Email"].ToString()),
                    Int32.Parse(dt["Id"].ToString())
                    
                ));
            }
        }
        connection.Close();
        return customers.GetRange(0, customers.Count);
    }
    
    public Customer? GetCustomerFromId(int customerId)
        => customers.FirstOrDefault(c => c.CustomerId == customerId);
    
    public void InsertCustomer(
        string firstName,
        string lastName,
        string street,
        string houseNumber,
        string city,
        short zipCode,
        string country,
        string phoneNumber,
        string email)
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        SqlDataReader dt;
        SqlCommand cmd = new SqlCommand(@"INSERT INTO Addresses(Street, HouseNumber, City, ZipCode, Country)
                                                     VALUES (@street, @houseNumber, @city, @zipCode, @country)
                                                     INSERT INTO Contacts(PhoneNumber, Email)
                                                     VALUES (@phoneNumber, @email)
                                                     INSERT INTO Customers( FirstName, LastName, AddressId, ContactID)
                                                     VALUES (@firstName, @lastName, 
                                                         (SELECT TOP 1 Id FROM Addresses ORDER BY Id DESC),
                                                         (SELECT TOP 1 Id FROM Contacts ORDER BY Id DESC))", connection);
        cmd.Parameters.AddWithValue("@street", street);
        cmd.Parameters.AddWithValue("@houseNumber", houseNumber);
        cmd.Parameters.AddWithValue("@city", city);
        cmd.Parameters.AddWithValue("@zipCode", zipCode);
        cmd.Parameters.AddWithValue("@country", country);
        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@firstName", firstName);
        cmd.Parameters.AddWithValue("@lastName", lastName);
        dt = cmd.ExecuteReader();

        connection.Close();
    }

    public void UpdateCustomer(int id,
        string firstName,
        string lastName,
        string street,
        string houseNumber,
        string city,
        short zipCode,
        string country,
        string phoneNumber,
        string email)
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        SqlDataReader dt;
        SqlCommand cmd = new SqlCommand(@"UPDATE Customers SET
                                                    FirstName = @firstName,
                                                    LastName = @lastName
                                                    WHERE Id = @id

                                                    UPDATE Addresses SET
                                                    Street = @street,
                                                    HouseNumber = @houseNumber,
                                                    City = @city,
                                                    ZipCode = @zipCode,
                                                    Country = @country
                                                    FROM Customers C, Addresses A
                                                    WHERE C.AddressId = A.Id
                                                    AND C.Id = @id

                                                    UPDATE Contacts SET
                                                    PhoneNumber = @phoneNumber,
                                                    Email = @email
                                                    FROM Customers C, Contacts CO
                                                    WHERE C.ContactId = CO.Id
                                                    AND C.Id = @id", connection);         
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@street", street);
        cmd.Parameters.AddWithValue("@houseNumber", houseNumber);
        cmd.Parameters.AddWithValue("@city", city);
        cmd.Parameters.AddWithValue("@zipCode", zipCode);
        cmd.Parameters.AddWithValue("@country", country);
        cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@firstName", firstName);
        cmd.Parameters.AddWithValue("@lastName", lastName);
        dt = cmd.ExecuteReader();

        connection.Close();
    }

    public void DeleteCustomerFromId(int customerId)
    {
        customers.RemoveAll(c => c.CustomerId == customerId);
    }


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Products        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Orders          //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public void UpdateSalesOrder(int OrderNumber, int CustomerId, decimal Price)
    {
        List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
        salesOrderHeaders.Add(new SalesOrderHeader(3, 24, OrderState.Created, 22, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(2, 35, OrderState.Created, 20, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(5, 89, OrderState.Created, 30, new List<SalesOrderLine>()));

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
        salesOrderHeaders.Add(new SalesOrderHeader(3, 24, OrderState.Created, 22, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(2, 35, OrderState.Created, 20, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(5, 89, OrderState.Created, 30, new List<SalesOrderLine>()));

        salesOrderHeaders.RemoveAll(s => s.OrderNumber == OrderNumber);
    }


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
        salesOrderHeaders.Add(new SalesOrderHeader(3, 24, OrderState.Created, 22, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(2, 35, OrderState.Created, 20, new List<SalesOrderLine>()));
        salesOrderHeaders.Add(new SalesOrderHeader(5, 89, OrderState.Created, 30, new List<SalesOrderLine>()));

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
}