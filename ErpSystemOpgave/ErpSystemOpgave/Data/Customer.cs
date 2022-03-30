using System;

namespace ErpSystemOpgave.Data;

public class Customer : Person
{
    public int CustomerId { get; set; }
    public DateTime? LastPurchase { get; set; }

    public Customer(string firstName, string lastName, Address address, string phoneNumber, string? email, int id)
        : base(firstName, lastName, address, phoneNumber, email)
    {
        CustomerId = id;
    }
}