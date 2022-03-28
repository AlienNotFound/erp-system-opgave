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

    public String Street { get; private set; }
    public String HouseNumber { get; private set; } //? Et husnummer kunne fx være "2B" eller "2.S t.v". Af den grund String.
    public String City { get; private set; }
    public short ZipCode { get; private set; }
    public String Country { get; private set; }
}