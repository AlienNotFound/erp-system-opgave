using System.Data;

namespace ErpSystemOpgave.Data;

public class ContactInfo
{

    public static ContactInfo FromReader(IDataReader reader)
    {
        var offset = 0;
        return FromReader(reader, ref offset);
    }

    public static ContactInfo FromReader(IDataReader reader, ref int offset)
    {
        offset++;
        return new(
            reader.GetString(offset++), //Phone
            reader.GetString(offset++)  //Email
        );
    }
    public ContactInfo() { }
    public ContactInfo(string phoneNumber, string? email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }
    public string PhoneNumber { get; set; } = "";
    public string? Email { get; set; } = "";

    public override string ToString() => $"Tlf: {PhoneNumber} Mail: {Email,16}";
}