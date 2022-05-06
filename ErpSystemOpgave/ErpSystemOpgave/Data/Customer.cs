using System;
using System.Data.SqlClient;

namespace ErpSystemOpgave.Data;

//eller som record:
// public record Customer : Person
// {
//     public int CustomerId { get; set; }

//     public Customer(string firstName, string lastName, Address address, ContactInfo contactInfo, int customerId) : base(firstName, lastName, address, contactInfo)
//     {
//         CustomerId = customerId;
//     }
// }

public class Customer : Person
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public string FullAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime? LastPurchase { get; set; }


    public Customer(string firstName, string lastName, Address address, ContactInfo contactInfo, int id, DateTime? lastPurchase)
        : base(firstName, lastName, address, contactInfo)
    {
        CustomerId = id;
        FullName = $"{FirstName} {LastName}";
        FullAddress = $"{Address.Street}, {Address.ZipCode} {Address.City}";
        PhoneNumber = $"{ContactInfo.PhoneNumber}";
        Email = $"{ContactInfo.Email}";
        LastPurchase = lastPurchase;
    }
}