using System.Data;

namespace ErpSystemOpgave.Data;

// Hele den den nedestående klasse kunne implementeres med denne ene linje:
// public record Address(string Street, string HouseNumber, string City, short ZipCode, string Country);

public class Address
{
    public static Address FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }

    public static Address FromReader(IDataReader reader, ref int offset)
    {
        return new(
            reader.GetInt32(offset++), //Id
            reader.GetString(offset++), //Street
            reader.GetString(offset++), //HouseNumber
            reader.GetString(offset++), //City
            reader.GetInt16(offset++),  //ZipCode
            reader.GetString(offset++)  //Country
        );
    }

    public Address() { }
    public Address(int id, string street, string houseNumber, string city, short zipCode, string country)
    {
        Id = id;
        Street = street;
        HouseNumber = houseNumber;
        City = city;
        ZipCode = zipCode;
        Country = country;
    }

    public override string ToString() => $"{Street} {HouseNumber}, {City} {ZipCode} - {Country}";

    public int Id { get; set; } = default;
    public string Street { get; private set; } = "";
    public string HouseNumber { get; private set; } = "";  //? Et husnummer kunne fx være "2B" eller "2.S t.v". Af den grund String.
    public string City { get; set; } = "";
    public short ZipCode { get; private set; } = default;
    public string Country { get; private set; } = "";
}