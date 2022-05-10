namespace ErpSystemOpgave.Data;

// Hele den den nedestående klasse kunne implementeres med denne ene linje:
// public record Person
// {
//     public string FirstName { get; set; }
//     public string LastName { get; set; }
//     public Address Address { get; set; }
//     public ContactInfo ContactInfo { get; set; }

//     public string FullName => $"{FirstName} {LastName}";

//     public Person(string firstName, string lastName, Address address, ContactInfo contactInfo)
//     {
//         FirstName = firstName;
//         LastName = lastName;
//         Address = address;
//         ContactInfo = contactInfo;
//     }


// }

public class Person
{
    // ? Lav `ContactInfo` til sin egen klasse?
    public Person() { }
    public Person(string firstName, string lastName, Address address, ContactInfo contactInfo)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        ContactInfo = contactInfo;
    }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public Address Address { get; set; } = new();
    public ContactInfo ContactInfo { get; set; } = new();
    public string FullAddress => Address.ToString();
    public string FullName => $"{FirstName} {LastName}";
}