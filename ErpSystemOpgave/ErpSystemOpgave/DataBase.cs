using System.Data;

namespace ErpSystemOpgave;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Data;
using System.Text;
using System.Threading.Tasks;
using ErpSystemOpgave.Data;

public sealed class DataBase
{
    static DataBase? _instance = null;
    private SqlConnection connection = null;

    private DataBase()
    {
        string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        connection = new SqlConnection(connectionString);
        connection.Open();
    }
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
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        SqlDataReader dt;

        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM Customers
                                                    INNER JOIN Addresses ON Addresses.Id = Customers.AddressId
                                                    INNER JOIN Contacts ON Contacts.Id = Customers.AddressId";
        dt = cmd.ExecuteReader();
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
                    Int32.Parse(dt["Id"].ToString()!),
                    Convert.ToDateTime(dt["LastPurchase"])
                ));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        // connection.Close();
        dt.Close();
        return customers.GetRange(0, customers.Count);
    }

    public Address? GetAddressById(int id)
    {
        SqlDataReader dt;
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Street, HouseNumber, City, ZipCode, Country FROM Addresses WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        dt = cmd.ExecuteReader();

        if (dt.Read())
        {
            var Street = dt.GetString(1);
            var HouseNumber = dt.GetString(2);
            var City = dt.GetString(3);
            var ZipCode = dt.GetInt16(4);
            var Country = dt.GetString(5);

            Address address = new Address(Street, HouseNumber, City, ZipCode, Country);
            // connection.Close();
            return address;
        }
        dt.Close();
        return null;
    }
    public ContactInfo? GetContactById(int id)
    {
        SqlDataReader dt;
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, PhoneNumber, Email FROM Contacts WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        dt = cmd.ExecuteReader();

        if (dt.Read())
        {
            var PhoneNumber = dt.GetString(1);
            var Email = dt.GetString(2);

            ContactInfo contactInfo = new ContactInfo(PhoneNumber, Email);
            // connection.Close();
            return contactInfo;
        }
        dt.Close();
        return null;
    }
    
    public Customer? GetCustomerById(int customerId)
    {
        SqlDataReader dt;
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, FirstName, LastName, AddressId, ContactId, LastPurchase FROM Customers WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", customerId);
        dt = cmd.ExecuteReader();
        
        if (dt.Read())
        {
            var id = dt.GetInt32(0);
            var firstName = dt.GetString(1);
            var lastName = dt.GetString(2);
            var addressId = dt.GetInt32(3);
            var contactId = dt.GetInt32(4);
            var lastPurchase = dt.GetDateTime(5);
            
            //connection.Close();
            Customer customer = new Customer(firstName, lastName, GetAddressById(addressId), GetContactById(contactId), id, lastPurchase);
            return customer;
        }
        dt.Close();
        return null;
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
        // string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        // SqlConnection connection = new(connectionString);

        SqlCommand cmd = new(@"INSERT INTO Addresses(Street, HouseNumber, City, ZipCode, Country)
                                                     VALUES (@street, @houseNumber, @city, @zipCode, @country)
                                                     INSERT INTO Contacts(PhoneNumber, Email)
                                                     VALUES (@phoneNumber, @email)
                                                     INSERT INTO Customers( FirstName, LastName, AddressId, ContactID, LastPurchase)
                                                     VALUES (@firstName, @lastName, 
                                                         (SELECT TOP 1 Id FROM Addresses ORDER BY Id DESC),
                                                         (SELECT TOP 1 Id FROM Contacts ORDER BY Id DESC), GETDATE())", connection);
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
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        SqlCommand cmd = connection.CreateCommand();
        
        cmd.CommandText = @"UPDATE Customers SET
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
                                AND C.Id = @id";
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
    }

    public void DeleteCustomerById(int customerId)
    {
        customers.RemoveAll(c => c.CustomerId == customerId);
    }


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Products        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public IEnumerable<Product> GetAllProducts()
    {
        using SqlCommand cmd = new("SELECT * FROM Products", connection);
        var dt = cmd.ExecuteReader();
        List<Product> prods = new();
        while (dt.Read())
            prods.Add(Product.FromReader(dt));
        dt.Close();
        return prods;
    }
    public Product? GetProductById(int productId)
    {
        using SqlCommand cmd = new("SELECT * FROM products WHERE ID = @id", connection);
        cmd.Parameters.AddWithValue("@id", productId);
        var dt = cmd.ExecuteReader();
        var res = dt.Read() ? Product.FromReader(dt) : null;
        dt.Close();
        return res;
    }

    public void InsertProduct(Product product)
    {
        using SqlCommand cmd = new("INSERT INTO products (name, description, instock, buyprice, saleprice, location, unit) VALUES (@name, @description, @instock, @buyprice, @saleprice, @location, @unit)", connection);
        cmd.Parameters.AddWithValue("@name", product.Name);
        cmd.Parameters.AddWithValue("@description", product.Description);
        cmd.Parameters.AddWithValue("@instock", product.InStock);
        cmd.Parameters.AddWithValue("@buyprice", product.BuyPrice);
        cmd.Parameters.AddWithValue("@saleprice", product.SalePrice);
        cmd.Parameters.AddWithValue("@location", product.Location);
        cmd.Parameters.AddWithValue("@unit", product.Unit.ToString());
        cmd.ExecuteNonQuery();
        Console.WriteLine("Data tilføjet");
    }
    public void UpdateProduct(int id, string name, string? description, decimal saleprice, decimal buyprice, double instock, string location, string unit, decimal avancepercent, decimal avancekroner)
    {
        // string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        // SqlConnection connection = new SqlConnection(connectionString);
        // connection.Open();
        SqlCommand cmd = new("UPDATE products SET name = @name, description = @description, instock = @instock, buyprice = @buyprice, saleprice = @saleprice, location = @location, unit = @unit WHERE id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@instock", instock);
        cmd.Parameters.AddWithValue("@buyprice", buyprice);
        cmd.Parameters.AddWithValue("@saleprice", saleprice);
        cmd.Parameters.AddWithValue("@location", location);
        cmd.Parameters.AddWithValue("@unit", unit);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Data opdateret");
        // connection.Close();
    }
    public void DeleteProduct(int id)
    {
        // string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        // SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("DELETE FROM products WHERE id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Data slettet");
    }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Orders          /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public void InsertSalesOrderHeader(int customerId, string state, DateTime creationTime)
    {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO SalesOrderHeaders(CustomerId, State, PriceSum, Date)
                                VALUES (@customerId, @state, 0, @date)
                                UPDATE SalesOrderHeaders
                                SET Street = a.Street,
	                                HouseNumber = a.HouseNumber,
	                                City = a.City,
	                                ZipCode = a.ZipCode,
	                                Country = a.Country
                                FROM SalesOrderHeaders s
                                INNER JOIN Customers c ON s.CustomerId = c.Id
                                INNER JOIN Addresses a ON c.AddressId = a.Id
                                WHERE s.Id = (SELECT TOP 1 Id FROM SalesOrderHeaders ORDER BY Id DESC)
                            UPDATE Customers
                                SET LastPurchase = GETDATE()
								WHERE Id = @customerId";
        
        cmd.Parameters.AddWithValue("@customerId", customerId);
        cmd.Parameters.AddWithValue("@state", state);
        //cmd.Parameters.AddWithValue("@priceSum", priceSum);
        cmd.Parameters.AddWithValue("@date", creationTime);
        
        cmd.ExecuteReader();
    }
    
    public IEnumerable<SalesOrderHeader> GetAllSalesOrderHeaders()
    {
        SqlDataReader dt;
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM SalesOrderHeaders
                                INNER JOIN Customers ON Customers.Id = SalesOrderHeaders.CustomerId";
        dt = cmd.ExecuteReader();
        salesOrderHeaders.Clear();
        
        try
        {
            SalesOrderHeader salesOrderHeader = new SalesOrderHeader(0, 0, 0, 0, DateTime.MinValue, "", "", "", 0, "");
            if (dt.Read())
            {
                salesOrderHeader.CustomerId = dt.GetInt32(1);
                salesOrderHeader.State = Enum.Parse<OrderState>(dt.GetString(2));
                salesOrderHeader.Price = dt.GetDecimal(3);
                salesOrderHeader.CreationTime = dt.GetDateTime(4);
                salesOrderHeader.Street = dt.GetString(5);
                salesOrderHeader.HouseNumber = dt.GetString(6);
                salesOrderHeader.City = dt.GetString(7);
                salesOrderHeader.ZipCode = dt.GetInt16(8);
                salesOrderHeader.Country = dt.GetString(9);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        dt.Close();
        return salesOrderHeaders.GetRange(0, salesOrderHeaders.Count);
    }
    public void InsertOrderLine(int productId, int quantity)
    {
        SqlCommand cmd = connection.CreateCommand();

            cmd.CommandText = @"INSERT INTO OrderLines (ProductId, Quantity, SalesOrderHeaderId)
                                    VALUES (@productId, @quantity, (SELECT TOP 1 Id FROM SalesOrderHeaders ORDER BY Id DESC))
                                UPDATE SalesOrderHeaders
                                    SET PriceSum = p.SalePrice * o.Quantity
                                    FROM SalesOrderHeaders s
                                    INNER JOIN OrderLines o ON s.Id = o.SalesOrderHeaderId
                                    INNER JOIN Products p ON o.ProductId = p.Id";
            
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@quantity", quantity);

        cmd.ExecuteReader();
    }
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
}