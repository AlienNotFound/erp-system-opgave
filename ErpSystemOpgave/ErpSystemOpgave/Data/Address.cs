using System;

namespace ErpSystemOpgave.Data;

public class Address
{
    public Address(string street, string houseNumber, string city, short zipCode, string country)
    {
        Street = street;
        HouseNumber = houseNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
    }

    public String Street { get; set; }
    public String HouseNumber { get;  set; } //? Et husnummer kunne fx være "2B" eller "2.S t.v". Af den grund String.
    public String City { get; set; }
    public short ZipCode { get; set; }
    public String Country { get; set; }
}