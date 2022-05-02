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
            string connectionString =
                @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
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

        ///////////////////////////////////////////////////////////////////////////
        /////////////         Products        /////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        public Product GetProductById(int productId)
        {
            SqlDataReader dt;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, [Name], [Description], SalePrice, BuyPrice, InStock, [Location], Unit, AvancePercent, AvanceKroner FROM products WHERE Id = @id";
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
                product.AvancePercent = dt.GetDecimal(8);
                product.AvanceKroner = dt.GetDecimal(9);
            }
            connection.Close();
            return product;
        }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Orders          /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public IEnumerable<SalesOrderHeader> GetAllSalesOrderHeaders()
    {
        string connectionString =
            @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        SqlConnection connection = new(connectionString);
        connection.Open();
            
        SqlCommand cmd = new(@"SELECT * FROM SalesOrderHeaders
                                            INNER JOIN Customers ON Customers.Id = SalesOrderHeaders.CustomerId
                                            ", connection);
        var dt = cmd.ExecuteReader();
        salesOrderHeaders.Clear();

        try
        {
            while (dt.Read())
            {
                salesOrderHeaders.Add(new SalesOrderHeader(
                    Int32.Parse(dt["Id"].ToString()!),
                    Int32.Parse(dt["CustomerId"].ToString()!),
                    Enum.Parse<OrderState>(dt["State"].ToString()!),
                    Decimal.Parse(dt["PriceSum"].ToString()!),
                    DateTime.Parse(dt["Date"].ToString()!)
                ));
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
    public void InsertOrderLine(int quantity)
        {
            string connectionString =
                @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
            SqlConnection connection = new(connectionString);
            connection.Open();

            SqlCommand cmd = new(@"INSERT INTO OrderLines(ProductId, Quanity, PricePer, PriceSum, SalesOrderHeaderId)
                                                     VALUES (@productId, @quantity, @priceper, @pricesum, @salesOrderHeaderId)
                                                     INSERT INTO Contacts(PhoneNumber, Email)
                                                     VALUES (@phoneNumber, @email)
                                                     INSERT INTO Customers( FirstName, LastName, AddressId, ContactID)
                                                     VALUES (@firstName, @lastName, 
                                                         (SELECT TOP 1 Id FROM Addresses ORDER BY Id DESC),
                                                         (SELECT TOP 1 Id FROM Contacts ORDER BY Id DESC))",
                connection);
            /*cmd.Parameters.AddWithValue("@street", street);
            cmd.Parameters.AddWithValue("@houseNumber", houseNumber);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@zipCode", zipCode);
            cmd.Parameters.AddWithValue("@country", country);
            cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);*/
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

    public void CreateSalesOrder(int orderNumber, int customerId, decimal price)
    {
        salesOrderHeaders.Add(new SalesOrderHeader(orderNumber, customerId, OrderState.Created, price, new List<SalesOrderLine>()));
    }
}