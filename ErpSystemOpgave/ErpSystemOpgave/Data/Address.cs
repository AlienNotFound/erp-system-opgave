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

    public string Street { get; private set; }
    public string HouseNumber { get; private set; } //? Et husnummer kunne fx være "2B" eller "2.S t.v". Af den grund String.
    public string City { get; private set; }
    public short ZipCode { get; private set; }
    public string Country { get; private set; }
}