using System.Data.SqlClient;

namespace ErpSystemOpgave;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Data;
using TECHCOOL.UI;

public sealed class DataBase
{
    static DataBase? _instance = null;
    private const string CONNECTION_STRING = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
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
    private List<Product> products = new();
    public List<SalesOrderHeader> salesOrderHeaders = new();

    //HACK: Dette er blot for at simulere en IDENTITY på Customer mens vi ikke har en database
    private int _nextCustomerId;
    private int NextCustomerId => _nextCustomerId++;

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Utility         /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Customer        /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public Customer? GetCustomerFromId(int customerId)
        => customers.FirstOrDefault(c => c.CustomerId == customerId);

    // Hvis vi blot returnerede en reference til _customers, ville consumeren kunne ændre i listen.
    // Med GetRange() returnerer vi en kopi af indholdet i stedet.
    public IEnumerable<Customer> GetAllCustomers()
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new(connectionString);
        connection.Open();

        SqlCommand cmd = new(@"SELECT * FROM Customers
                                                    INNER JOIN Addresses ON Addresses.Id = Customers.AddressId
                                                    INNER JOIN Contacts ON Contacts.Id = Customers.AddressId", connection);
        var dt = cmd.ExecuteReader();
        customers.Clear();

        try
        {
            while (dt.Read()) //TODO: Unit test at den finder data og kolonner
            {
                customers.Add(new Customer(
                    dt["FirstName"].ToString()!,
                    dt["LastName"].ToString()!,
                    new Address(dt["Street"].ToString()!,
                        dt["HouseNumber"].ToString()!,
                        dt["City"].ToString()!,
                        short.Parse(dt["ZipCode"].ToString()!),
                        dt["Country"].ToString()!),
                    new ContactInfo(dt["PhoneNumber"].ToString()!,
                        dt["Email"].ToString()),
                    Int32.Parse(dt["Id"].ToString()!)
                ));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        connection.Close();
        return customers.GetRange(0, customers.Count);
    }

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
        SqlConnection connection = new(connectionString);
        connection.Open();

        SqlCommand cmd = new(@"INSERT INTO Addresses(Street, HouseNumber, City, ZipCode, Country)
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
        cmd.ExecuteReader();

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
        SqlConnection connection = new(connectionString);
        connection.Open();

        SqlCommand cmd = new(@"UPDATE Customers SET
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
        cmd.ExecuteReader();

        connection.Close();
    }

    public void DeleteCustomerFromId(int customerId)
    {
        customers.RemoveAll(c => c.CustomerId == customerId);
    }


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Products        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public IEnumerable<Product> GetAllProducts()
    {
        using SqlConnection connection = new(CONNECTION_STRING);
        using SqlCommand cmd = new("SELECT * FROM Products", connection);
        cmd.Connection.Open();
        var dt = cmd.ExecuteReader();
        List<Product> prods = new();
        while (dt.Read())
            prods.Add(Product.FromReader(dt));
        return prods;
    }
    public Product? GetProductById(int productId)
    {
        using var connection = new SqlConnection(CONNECTION_STRING);
        using SqlCommand cmd = new("SELECT * FROM products WHERE ID = @id", connection);
        cmd.Parameters.AddWithValue("@id", productId);
        cmd.Connection.Open();
        var dt = cmd.ExecuteReader();
        if (dt.Read())
            return Product.FromReader(dt);
        return null;
    }

    public void InsertProduct(Product product)
    {
        using SqlConnection connection = new(CONNECTION_STRING);
        using SqlCommand cmd = new("INSERT INTO products (name, description, instock, buyprice, saleprice, location, unit, avancepercent, avancekroner) VALUES (@name, @description, @instock, @buyprice, @saleprice, @location, @unit, @avancepercent, @avancekroner)", connection);
        cmd.Connection.Open();
        cmd.Parameters.AddWithValue("@name", product.Name);
        cmd.Parameters.AddWithValue("@description", product.Description);
        cmd.Parameters.AddWithValue("@instock", product.InStock);
        cmd.Parameters.AddWithValue("@buyprice", product.BuyPrice);
        cmd.Parameters.AddWithValue("@saleprice", product.SalePrice);
        cmd.Parameters.AddWithValue("@location", product.Location);
        cmd.Parameters.AddWithValue("@unit", product.Unit);
        cmd.Parameters.AddWithValue("@avancepercent", product.AvancePercent);
        cmd.Parameters.AddWithValue("@avancekroner", product.AvanceKroner);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Data tilføjet");
        connection.Close();
    }
    public void UpdateProduct(int id, string name, string description, decimal saleprice, decimal buyprice, double instock, string location, string unit, decimal avancepercent, decimal avancekroner)
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand cmd = new("UPDATE products SET name = @name, description = @description, instock = @instock, buyprice = @buyprice, saleprice = @saleprice, location = @location, unit = @unit, avancepercent = @avancepercent, avancekroner = @avancepercent WHERE id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@description", description);
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

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Orders          /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////

    public void UpdateSalesOrder(int orderNumber, int customerId, decimal price)
    {
        var result = from s in salesOrderHeaders
                     where s.OrderNumber == orderNumber
                     select s;
        foreach (var salesOrder in result)
        {
            Console.WriteLine("\nOriginalt: ");
            Console.WriteLine("Kunde id: " + salesOrder.CustomerId);
            Console.WriteLine("Pris: " + salesOrder.Price);
            salesOrder.CustomerId = customerId;
            salesOrder.Price = price;
            Console.WriteLine("\nOpdateret: ");
            Console.WriteLine("Kunde id: " + salesOrder.CustomerId);
            Console.WriteLine("Pris: " + salesOrder.Price);
        }
    }

    public void DeleteSalesOrder(int orderNumber)
    {
        salesOrderHeaders.RemoveAll(s => s.OrderNumber == orderNumber);
    }


    public SalesOrderHeader? GetSalesOrderById(int orderId)
    {
        var Order = salesOrderHeaders.Find(id => id.OrderNumber == orderId);
        if (Order is null)
            return null;
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

    public void CreateSalesOrder(int orderNumber, int customerId, decimal price)
    {
        salesOrderHeaders.Add(new SalesOrderHeader(orderNumber, customerId, OrderState.Created, price, new List<SalesOrderLine>()));
    }
}