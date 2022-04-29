using System;
using System.Collections.Generic;

namespace ErpSystemOpgave.Data;

public enum OrderState
{
    None,
    Created,
    Confirmed,
    Packed,
    Done,
}

public class SalesOrderHeader
{
    public SalesOrderHeader(int orderNumber, int customerId, OrderState state, decimal price, DateTime creationTime)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        State = state;
        Price = price;
        CreationTime = creationTime;
        Customer = DataBase.Instance.GetCustomerFromId(customerId)!;
        //CreationTime = DateTime.Now;
    }

    public int OrderNumber { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string CustomerName
    {
        get { return Customer.FullName;} }
    public OrderState State { get; set; }
    public decimal Price { get; set; }
}
