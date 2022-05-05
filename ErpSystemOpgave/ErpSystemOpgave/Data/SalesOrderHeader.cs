using System;
using System.Collections.Generic;

namespace ErpSystemOpgave.Data;

public enum OrderState
{
    None,
    Created,
    Confirmed,
    Packed,
    Done,
}

public class SalesOrderHeader
{
    public SalesOrderHeader(int orderNumber,
        int customerId,
        OrderState state,
        decimal price,
        DateTime creationTime,
        string street,
        string houseNumber,
        string city,
        short zipCode,
        string country)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        State = state;
        Price = price;
        CreationTime = creationTime;
        Customer = DataBase.Instance.GetCustomerById(customerId)!;
        Street = street;
        HouseNumber = houseNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        //CreationTime = DateTime.Now;
    }

    public int OrderNumber { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string CustomerName
    {
        get { return Customer.FullName; } }
    public OrderState State { get; set; }
    public decimal Price { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string City { get; set; }
    public short ZipCode { get; set; }
    public string Country { get; set; }
}
