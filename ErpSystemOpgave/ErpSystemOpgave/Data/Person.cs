namespace ErpSystemOpgave.Data;


public class Person
{
    // ? Lav `ContactInfo` til sin egen klasse?
    public Person(string firstName, string lastName, Address address, ContactInfo contactInfo)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        ContactInfo = contactInfo;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Address Address { get; set; }
    public ContactInfo ContactInfo { get; set; }
    public string FullAddress => Address.ToString();
    public string FullName => $"{FirstName} {LastName}";
}