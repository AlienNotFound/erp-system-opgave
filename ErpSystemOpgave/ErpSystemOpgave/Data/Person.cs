using System;

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
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String FullName => $"{FirstName} {LastName}";
    public Address Address { get; set; }
    public String PhoneNumber { get; set; }
    public String? Email { get; set; }
}