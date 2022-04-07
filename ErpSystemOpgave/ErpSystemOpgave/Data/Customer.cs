using System;

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
    public DateTime? LastPurchase { get; set; }


    public Customer(string firstName, string lastName, Address address, ContactInfo contactInfo, int id)
        : base(firstName, lastName, address, contactInfo)
    {
        CustomerId = id;
    }
}