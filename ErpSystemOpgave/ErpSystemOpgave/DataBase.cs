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
            while(dt.Read())
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
        connection.Close();
        return customers.GetRange(0, customers.Count);
    }

    public Address GetAddressById(int id)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
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
            connection.Close();
            return address;
        }
        connection.Close();
        return null;
    }
    public ContactInfo GetContactById(int id)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
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
            connection.Close();
            return contactInfo;
        }
        connection.Close();
        return null;
    }
    
    public Customer? GetCustomerById(int customerId)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
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
            
            connection.Close();
            Customer customer = new Customer(firstName, lastName, GetAddressById(addressId), GetContactById(contactId), id, lastPurchase);
            return customer;
        }
        connection.Close();
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

        connection.Close();
    }

    public void DeleteCustomerById(int customerId)
    {
        customers.RemoveAll(c => c.CustomerId == customerId);
    }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Products        /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public Product GetProductById(int productId)
    {
        connection.Open();
        SqlDataReader dt;
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, [Name], [Description], SalePrice, BuyPrice, InStock, [Location], Unit FROM products WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", productId);
        dt = cmd.ExecuteReader();
        
        Product product = new Product(0, "", "", 0, 0, 0, "", 0, 0, 0);
        if(dt.Read())
        {
            product.ProductId = dt.GetInt32(0);
            product.Name = dt.GetString(1);
            product.Description = dt.GetString(2);
            product.SalePrice = dt.GetDecimal(3);
            product.BuyPrice = dt.GetDecimal(4);
            product.InStock = dt.GetDouble(5);
            product.Location = dt.GetString(6);
            product.Unit = Enum.Parse<ProductUnit>(dt.GetString(7));
        }
        connection.Close();
        return product;
    }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Orders          /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public void InsertSalesOrderHeader(int customerId, string state, DateTime creationTime)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        SqlDataReader dt;
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
        connection.Close();
    }
    
    public IEnumerable<SalesOrderHeader> GetAllSalesOrderHeaders()
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
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

        connection.Close();
        return salesOrderHeaders.GetRange(0, salesOrderHeaders.Count);
    }
    public void InsertOrderLine(int productId, int quantity)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
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
            connection.Close();
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