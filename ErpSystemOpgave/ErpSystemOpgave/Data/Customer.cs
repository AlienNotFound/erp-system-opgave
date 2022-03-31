using System;

namespace ErpSystemOpgave.Data;

// eller som record:
// public record Customer
//     (string FirstName, string LastName, Address Address, string PhoneNumber, string? Email, int CustomerId) 
//     : Person(FirstName, LastName, Address, PhoneNumber, Email);

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