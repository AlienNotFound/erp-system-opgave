namespace ErpSystemOpgave.Data;

// Hele den den nedestående klasse kunne implementeres med denne ene linje:
// public record Address(string Street, string HouseNumber, string City, short ZipCode, string Country);

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

    public override string ToString() => $"{Street} {HouseNumber}, {City} {ZipCode} - {Country}";

    public string Street { get; private set; }
    public string HouseNumber { get; private set; } //? Et husnummer kunne fx være "2B" eller "2.S t.v". Af den grund String.
    public string City { get; set; }
    public short ZipCode { get; private set; }
    public string Country { get; private set; }
}