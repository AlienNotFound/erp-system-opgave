namespace ErpSystemOpgave.Data; 

public class ContactInfo {
    public ContactInfo(string phoneNumber, string? email) {
        PhoneNumber = phoneNumber;
        Email = email;
    }
    public string PhoneNumber { get; set; }
    public string? Email { get; set; }
}