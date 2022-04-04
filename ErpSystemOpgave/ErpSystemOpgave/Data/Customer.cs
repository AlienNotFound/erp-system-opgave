using System;

namespace ErpSystemOpgave.Data;

// eller som record:
// public record Customer
//     (string FirstName, string LastName, Address Address, string PhoneNumber, string? Email, int CustomerId) 
//     : Person(FirstName, LastName, Address, PhoneNumber, Email);

public class Customer : Person
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public string FullAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime? LastPurchase { get; set; }

    public Customer(string firstName, string lastName, Address address, ContactInfo contactInfo, int id)
        : base(firstName, lastName, address, contactInfo)
    {
        CustomerId = id;
        FullName = $"{FirstName} {LastName}";
        FullAddress = $"{address.Street} {address.HouseNumber}, {address.ZipCode} {address.City}";
        Email = contactInfo.Email;
        PhoneNumber = contactInfo.PhoneNumber;
    }
}