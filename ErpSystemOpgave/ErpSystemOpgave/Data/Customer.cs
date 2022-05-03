using System;
using System.Data.SqlClient;

namespace ErpSystemOpgave.Data;

public class Customer : Person
{
    public int CustomerId { get; set; }
    public DateTime? LastPurchase { get; set; }


    public Customer(string firstName, string lastName, Address address, ContactInfo contactInfo, int id)
        : base(firstName, lastName, address, contactInfo)
    {
        CustomerId = id;
    }

}