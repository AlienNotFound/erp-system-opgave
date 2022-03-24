using ErpSystemOpgave;
using static System.Console;

Customer cus1 = new Customer();

DataBase db = new DataBase();
WriteLine(db.GetAllCustomers());