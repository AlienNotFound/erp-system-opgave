using System.Data;

namespace ErpSystemOpgave;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

public sealed class DataBase
{
    static DataBase? _instance = null;
    private SqlConnection connection;

    private DataBase()
    {
        string connectionString = @"
        Server=docker.data.techcollege.dk;
        Database=H1PD021122_Gruppe3;
        User Id=H1PD021122_Gruppe3;
        Password=H1PD021122_Gruppe3;
        MultipleActiveResultSets=True";
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


    public T? GetEntryById<T>(string tableName, int id, Func<IDataReader, T> readerFunc) where T : class
    {
        using SqlCommand cmd = new("SELECT * FROM @table_name WHERE Id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@table_name", tableName);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? readerFunc(dt) : null;
    }

    public IEnumerable<T> GetAllEntries<T>(string tableName, Func<IDataReader, T> readerFunc) where T : class
    {
        using SqlCommand cmd = new("SELECT * FROM @table_name", connection);
        cmd.Parameters.AddWithValue("@table_name", tableName);
        using var dt = cmd.ExecuteReader();
        List<T> entries = new();
        while (dt.Read())
            entries.Add(readerFunc(dt));
        return entries;
    }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Customer        /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    // Hvis vi blot returnerede en reference til _customers, ville consumeren kunne ændre i listen.
    // Med GetRange() returnerer vi en kopi af indholdet i stedet.
    public IEnumerable<Customer> GetAllCustomers()
    {
        SqlCommand cmd = new(@"SELECT Customers.*, ISNULL(LastPurchase,'0001-01-01') LastPurchase FROM Customers LEFT JOIN LastPurchases ON CustomerId = Id", connection);
        using var dt = cmd.ExecuteReader();
        List<Customer> customers = new();
        while (dt.Read())
            customers.Add(Customer.FromReader(dt));
        return customers;
    }

    public Address? GetAddressById(int id)
    {
        SqlCommand cmd = new("SELECT Id, Street, HouseNumber, City, ZipCode, Country FROM Addresses WHERE Id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? Address.FromReader(dt) : null;
    }

    public ContactInfo? GetContactById(int id)
    {
        using SqlCommand cmd = new("SELECT Id, PhoneNumber, Email FROM Contacts WHERE Id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? ContactInfo.FromReader(dt) : null;
    }

    public Customer? GetCustomerById(int customerId)
    {
        using SqlCommand cmd = new(@"SELECT Id, FirstName, LastName, AddressId, ContactId, ISNULL(LastPurchase,'0001-01-01') AS LastPurchase 
                                                FROM Customers
                                                LEFT JOIN LastPurchases ON CustomerId = Id
                                                WHERE Customers.Id = @id", connection);
        cmd.Parameters.AddWithValue("@id", customerId);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? Customer.FromReader(dt) : null;
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

        using SqlCommand cmd = new(@"INSERT INTO Addresses(Street, HouseNumber, City, ZipCode, Country)
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
        // string connectionString = @"Server=docker.data.techcollege.dk;Database=H1PD021122_Gruppe3;User Id=H1PD021122_Gruppe3;Password=H1PD021122_Gruppe3;";
        // SqlConnection connection = new(connectionString);

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
    }

    public void DeleteCustomerById(int Id)
    {
        SqlCommand cmd = new(@"SELECT c.Id as Id, AddressId, ContactId INTO #TempTable FROM Customers c
                                        join Addresses as a on a.Id = c.AddressId
                                        join Contacts as co on co.Id = c.ContactId
                                        where c.Id = @id;
                                        DELETE FROM Addresses where Id IN (SELECT AddressId from #TempTable)
                                        DELETE FROM Contacts where Id IN (SELECT ContactId FROM #TempTable)
                                        DELETE FROM Customers WHERE Id in (SELECT Id From #TempTable);
                                        DROP TABLE #TempTable;", connection);
        cmd.Parameters.AddWithValue("@id", Id);
        cmd.ExecuteNonQuery();
    }


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Products        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public IEnumerable<Product> GetAllProducts()
    {
        using SqlCommand cmd = new("SELECT * FROM Products", connection);
        using var dt = cmd.ExecuteReader();
        List<Product> prods = new();
        while (dt.Read())
            prods.Add(Product.FromReader(dt));
        return prods;
    }

    public Product? GetProductById(int productId)
    {
        using SqlCommand cmd = new("SELECT * FROM products WHERE ID = @id", connection);
        cmd.Parameters.AddWithValue("@id", productId);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? Product.FromReader(dt) : null;
    }

    public void InsertProduct(Product product)
    {
        using SqlCommand cmd = new("INSERT INTO products (name, description, instock, buyprice, saleprice, location, unit) VALUES (@name, @description, @instock, @buyprice, @saleprice, @location, @unit)", connection);
        cmd.Parameters.AddRange(product.SqlParameters);
        cmd.ExecuteNonQuery();
    }
    public void UpdateProduct(Product product)
    {
        SqlCommand cmd = new(@"UPDATE products SET name = @name, description = @description, instock = @instock, buyprice = @buyprice, saleprice = @saleprice, location = @location, unit = @unit WHERE id = @id", connection);
        cmd.Parameters.AddRange(product.SqlParameters);
        cmd.ExecuteNonQuery();
    }
    public void DeleteProduct(int id)
    {
        SqlCommand cmd = new("DELETE FROM products WHERE id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }

    ///////////////////////////////////////////////////////////////////////////
    /////////////         Orders          /////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////
    public int InsertSalesOrderHeader(int customerId, string state, DateTime creationTime)
    {
        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = @"INSERT INTO SalesOrderHeaders(CustomerId, State, PriceSum, Date)
                                VALUES (@customerId, @state, @priceSum, @date)
                                UPDATE SalesOrderHeaders
                                SET Street = a.Street,
	                                HouseNumber = a.HouseNumber,
	                                City = a.City,
	                                ZipCode = a.ZipCode,
	                                Country = a.Country
                                FROM SalesOrderHeaders s
                                INNER JOIN Customers c ON s.CustomerId = c.Id
                                INNER JOIN Addresses a ON c.AddressId = a.Id
                                WHERE s.Id = (SELECT TOP 1 Id FROM SalesOrderHeaders ORDER BY Id DESC);
                                SELECT SCOPE_IDENTITY()";

        cmd.Parameters.AddWithValue("@customerId", customerId);
        cmd.Parameters.AddWithValue("@state", state);
        cmd.Parameters.AddWithValue("@priceSum", (decimal)0);
        cmd.Parameters.AddWithValue("@date", creationTime);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public IEnumerable<SalesOrderLine> GetOrderLinesByHeader(int orderId)
    {
        using SqlCommand cmd = new("SELECT * FROM OrderLines WHERE SalesOrderHeaderId = @id", connection);
        cmd.Parameters.AddWithValue("@id", orderId);
        using var dt = cmd.ExecuteReader();
        List<SalesOrderLine> orderLines = new();
        while (dt.Read())
            orderLines.Add(SalesOrderLine.FromReader(dt));
        return orderLines;
    }

    public IEnumerable<SalesOrderHeader> GetAllSalesOrderHeaders()
    {
        using SqlCommand cmd = new(@"
            SELECT SalesOrderHeaders.*, ISNULL(TotalPrice, 0) AS TotalPrice FROM SalesOrderHeaders
            LEFT JOIN OrderTotals
            ON Id = SalesOrderHeaderId", connection);
        using var dt = cmd.ExecuteReader();
        List<SalesOrderHeader> orders = new();

        while (dt.Read())
        {
            var offset = 0;
            var order = SalesOrderHeader.FromReader(dt, ref offset);
            order.Price = dt.GetDecimal(offset);
            orders.Add(order);
        }
        return orders;
    }

    public void InsertOrderLine(int productId, int quantity, decimal price, int orderNumber)
    {
        SqlCommand cmd = connection.CreateCommand();

        cmd.CommandText = @"INSERT INTO OrderLines (ProductId, Quantity, Price, SalesOrderHeaderId)
                                    VALUES (@productId, @quantity, @price, @order)
                                UPDATE SalesOrderHeaders
                                    SET PriceSum = p.SalePrice * o.Quantity
                                    FROM SalesOrderHeaders s
                                    INNER JOIN OrderLines o ON s.Id = o.SalesOrderHeaderId
                                    INNER JOIN Products p ON o.ProductId = p.Id";

        cmd.Parameters.AddWithValue("@productId", productId);
        cmd.Parameters.AddWithValue("@quantity", quantity);
        cmd.Parameters.AddWithValue("@price", price);
        cmd.Parameters.AddWithValue("@order", orderNumber);

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

    public SalesOrderHeader? GetSalesOrderById(int orderId)
    {
        using SqlCommand cmd = new("SELECT * FROM SalesOrderHeaders WHERE ID = @id", connection);
        cmd.Parameters.AddWithValue("@id", orderId);
        using var dt = cmd.ExecuteReader();
        return dt.Read() ? SalesOrderHeader.FromReader(dt) : null;
    }

    public void DeleteSalesOrder(int id)
    {
        //TODO: set tables on delete behavior to cascade. this method bugs out if the order has any orderlines.
        SqlCommand cmd = new("DELETE FROM SalesOrderHeaders WHERE id = @id", connection);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();
    }
}