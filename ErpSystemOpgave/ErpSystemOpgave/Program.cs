namespace ErpSystemOpgave;
using Data;
using static System.Console;


class Program {
    public static void Main(string[] args) {
        var db = new DataBase();
        db.InsertCustomer(
            "Bob", 
            "Bobsen", 
            new Address("Vejgade Alle", "28B", "Herrens Mark", 1234, "Lalaland"), 
            new ContactInfo("88888888", "test@mail.com")
            );
        db.InsertCustomer(
            "Søren",  
            "Sørensen",  
            new Address("Østre-nøresøndergade", "2. sal t.v", "Beyond Herrens Mark", 1234, "Lalaland"),
            new ContactInfo("12341234", "test2@mail.com") 
            );
        
        foreach (var customer in db.GetAllCustomers()) {
            WriteLine(customer);
        }
        
    }
}