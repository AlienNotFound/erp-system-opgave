using System;
using System.Collections.Generic;
using System.Data;

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

    public static SalesOrderHeader FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }

    public static SalesOrderHeader FromReader(IDataReader reader, ref int offset)
    {
        SalesOrderHeader result = new(
            reader.GetInt32(offset++),
            reader.GetInt32(offset++),
            Enum.Parse<OrderState>(reader.GetString(offset++)),
            reader.GetDecimal(offset++),
            reader.GetDateTime(offset++),
            reader.GetString(offset++),
            reader.GetString(offset++),
            reader.GetString(offset++),
            reader.GetInt16(offset++),
            reader.GetString(offset++)
        );
        return result;
    }

    public SalesOrderHeader() { }

    public SalesOrderHeader(int orderNumber,
        int customerId,
        OrderState state,
        decimal price,
        DateTime creationTime,
        string street,
        string houseNumber,
        string city,
        short zipCode,
        string country
        )
    {
        CustomerId = customerId;
        Customer = DataBase.Instance.GetCustomerById(customerId) ?? new();
        OrderNumber = orderNumber;
        State = state;
        Price = price;
        CreationTime = creationTime;
        Street = street;
        HouseNumber = houseNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
        CreationTime = DateTime.Now;
    }

    public int OrderNumber { get; set; } = default;
    public DateTime CreationTime { get; set; } = default;
    public OrderState State { get; set; } = default;
    public decimal Price { get; set; } = default;
    public int CustomerId { get; set; } = default;
    public Customer Customer { get; set; } = new();
    public Address Address { get; set; } = new();
    public string Street { get; set; } = "";
    public string HouseNumber { get; set; } = "";
    public string City { get; set; } = "";
    public short ZipCode { get; set; } = default;
    public string Country { get; set; } = "";
    public DateTime? CompletionTime { get; set; }


    // public ContactInfo Contact => Customer.ContactInfo;
    // public Address CustomerAddress => Customer.Address;
    public override string ToString() => $"{Customer} {OrderNumber}, {CreationTime}, {State}, {Price}";

}
