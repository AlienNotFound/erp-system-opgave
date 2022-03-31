namespace ErpSystemOpgave.Data;

public class Person
{
    // ? Lav `ContactInfo` til sin egen klasse?
    public Person(string firstName, string lastName, Address address, string phoneNumber, string? email)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public Address Address { get; set; }
    public String PhoneNumber { get; set; }
    public String? Email { get; set; }
}