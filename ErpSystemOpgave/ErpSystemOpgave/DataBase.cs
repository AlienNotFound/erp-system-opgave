using System;
using static System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemOpgave
{
    internal class DataBase
    {

        public string GetCustomerFromId(int CustomerId)
        {
            return null;
        }

        public string[] GetAllCustomers()
        {
            Customer customer = new Customer();
            for (int CustomerNum = 0; CustomerNum < customer.CustomerNumber;)
            {
                WriteLine($"{customer.FullName} {customer.Email} {customer.Phone} {customer.Address} {customer.CustomerNumber} {customer.ContactInfo}");
            }

            return null;
        }

        public void InsertCustomer()
        {

        }

        public void UpdateCustomer()
        {

        }

        public void DeleteCustomerFromId(int CustomerId)
        {

        }
    }
}
