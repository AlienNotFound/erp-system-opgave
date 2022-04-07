using System;
namespace ErpSystemOpgave.Data;

public class Organisation
{
    public Organisation(string name, Address address, string currency)
    {
        Name = name;
        Address = address;
        Currency = currency;
    }

    public string Name { get; set; }
    public Address Address { get; set; }
    public string Currency { get; set; }
}