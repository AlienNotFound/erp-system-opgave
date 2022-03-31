using System.Collections.Generic;
using System.Linq;
using ErpSystemOpgave.Data;


namespace ErpSystemOpgave; 

public class DataBase {
    private List<Customer> _customers;

    public DataBase() {
        _customers = new List<Customer>();
        _nextCustomerId = 1;
    }

    //HACK: Dette er blot for at simulere en IDENTITY på Customer mens vi ikke har en database
    private int _nextCustomerId;
    private int NextCustomerId => _nextCustomerId++;

    ////////////////////////////////////////////////////////////////////////////
    /////////////         Customer        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////
    public Customer? GetCustomerFromId(int customerId)
        => _customers.FirstOrDefault(c => c.CustomerId == customerId);
        
    // Hvis vi blot returnerede en reference til _customers, ville consumeren kunne ændre i listen.
    // Med GetRange() returnerer vi en kopi af indholdet i stedet.
    public IEnumerable<Customer> GetAllCustomers()
        => _customers.GetRange(0, _customers.Count); 

    public void InsertCustomer(
        string firstName,
        string lastName,
        Address address,
        ContactInfo contactInfo)
    {
        _customers.Add(new Customer(
            firstName,
            lastName,
            address,
            contactInfo,
            NextCustomerId
        ));
    }

    public void UpdateCustomer(int customerId, Customer updatedCustomer) {
        if (_customers.FindIndex(c => c.CustomerId == customerId) is var index && index != -1)
            _customers[index] = updatedCustomer;
    }

    public void DeleteCustomerFromId(int customerId) {
        _customers.RemoveAll(c => c.CustomerId == customerId);
    }


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Products        //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////


    ////////////////////////////////////////////////////////////////////////////
    /////////////         Orders          //////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////
}