﻿using System;
using System.Collections.Generic;
using ErpSystemOpgave;
using ErpSystemOpgave.Data;
using Xunit;

namespace UnitTestsErp
{
    public class DataBaseTests2
    {
        [Fact]
        public void GetSalesOrderById_Returns_Correct_Order()
        {
            //arrange
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            DataBase sut = new DataBase();
            sut.salesOrderHeaders = salesOrderHeaders;
            
            //act
            var order = sut.GetSalesOrderById(3);
            
            //assert
            Assert.Equal(24, order.CustomerId);
        }
        
        [Fact]
        public void GetAllSalesOrders_Returns_All_Orders()
        {
            //arrange
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            DataBase sut = new DataBase();
            sut.salesOrderHeaders = salesOrderHeaders;
            
            //act
            var listLength = salesOrderHeaders.Count;

            //assert
            Assert.True(sut.salesOrderHeaders.Count == listLength);
        }

        [Fact]
        public void CreateSalesOrder_Can_Create_Order()
        {
            //arrange
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            DataBase sut = new DataBase();
            sut.salesOrderHeaders = salesOrderHeaders;
            
            //act
            salesOrderHeaders.Add(new SalesOrderHeader(6, 65,OrderState.Created,70, new List<SalesOrderLine>()));
            
            //assert
            Assert.True(sut.salesOrderHeaders.Count == 4);
        }

        [Fact]
        public void UpdateSalesOrder_Can_Update_Order()
        {
            //arrange
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            DataBase sut = new DataBase();
            sut.salesOrderHeaders = salesOrderHeaders;
            
            //act
            salesOrderHeaders[2].State = OrderState.Done;
            
            Assert.True(sut.salesOrderHeaders[2].State == OrderState.Done);
        }

        [Fact]
        public void DeleteSalesOrder_Can_Delete_Order()
        {
            //arrange
            List<SalesOrderHeader> salesOrderHeaders = new List<SalesOrderHeader>();
            salesOrderHeaders.Add(new SalesOrderHeader(3, 24,OrderState.Created,22, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(2, 35,OrderState.Created,20, new List<SalesOrderLine>()));
            salesOrderHeaders.Add(new SalesOrderHeader(5, 89,OrderState.Created,30, new List<SalesOrderLine>()));
            
            DataBase sut = new DataBase();
            
            //act
            salesOrderHeaders.RemoveAll(s => s.OrderNumber == 2);
            
            //assert
            Assert.True(salesOrderHeaders.Count == 2);
        }
    }
}