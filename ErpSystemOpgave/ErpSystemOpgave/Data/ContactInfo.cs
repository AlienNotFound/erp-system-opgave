namespace ErpSystemOpgave.Data;

// public record ContactInfo(string PhoneNumber, string? Email);

public class ContactInfo
{
    public ContactInfo(string phoneNumber, string? email)
    {
        PhoneNumber = phoneNumber;
        Email = email;
    }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }

    public override string ToString() => $"{PhoneNumber}, {Email}";
}