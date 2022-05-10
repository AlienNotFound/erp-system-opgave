using System;
using System.Data;
using System.Data.SqlClient;

namespace ErpSystemOpgave.Data;

public class Customer : Person
{
    public static readonly string SELECT_QUERY = "SELECT * FROM Customers";

    public static Customer FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }
    public static Customer FromReader(IDataReader reader, ref int offset) => new(
            reader.GetInt32(offset++),
            reader.GetString(offset++),
            reader.GetString(offset++),
            DataBase.Instance.GetAddressById(reader.GetInt32(offset++))!,
            DataBase.Instance.GetContactById(reader.GetInt32(offset++))!,
            reader.GetDateTime(offset++));

    public int CustomerId { get; set; }
    public DateTime? LastPurchase { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }

    public Customer() { }

    public Customer(int id, string firstName, string lastName, Address address, ContactInfo contactInfo, DateTime? lastPurchase)
        : base(firstName, lastName, address, contactInfo)
    {
      
        CustomerId = id;
        PhoneNumber = contactInfo.PhoneNumber;
        Email = contactInfo.Email;
        LastPurchase = lastPurchase;
    }

    public override string ToString() => FullName;
}