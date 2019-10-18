using System;
using Xunit;
using RatStore.Logic;
using RatStore.Data;
using System.Collections.Generic;

namespace RatStore.Test
{
    public class TestValidation
    {
        RatStore.Logic.RatStore test = new RatStore.Logic.RatStore();

        [Fact]
        public void InvalidLocation()
        {
            Location location = new Location
            {
                LocationId = 0
            };

            bool valid = test.ValidateLocation(location);

            Assert.False(valid);
        }

        [Fact]
        public void ValidLocation()
        {
            Location location = new Location
            {
                LocationId = 1,
                Address = "yes"
            };

            bool valid = test.ValidateLocation(location);

            Assert.True(valid);
        }

        [Fact]
        public void InvalidOrder_NoDetails()
        {
            Order order = new Order();

            bool valid = test.ValidateOrder(order);

            Assert.False(valid);
        }


        [Fact]
        public void InvalidOrder_NullOrder()
        {
            Order order = null;

            bool valid = test.ValidateOrder(order);

            Assert.False(valid);
        }

        [Fact]
        public void ValidOrder()
        {
            Order order = new Order();
            order.OrderDetails = new List<OrderDetails>();
            order.OrderDetails.Add(new OrderDetails());

            bool valid = test.ValidateOrder(order);

            Assert.True(valid);
        }

        [Fact]
        public void InvalidCustomer()
        {
            Customer customer = new Customer
            {
                FirstName = "",
                LastName = "",
                PhoneNumber = ""
            };

            bool valid = test.ValidateCustomer(customer);

            Assert.False(valid);
        }

        [Fact]
        public void ValidCustomer()
        {
            Customer customer = new Customer
            {
                FirstName = "Joe",
                LastName = "Joeson",
                PhoneNumber = "1111111111"
            };

            bool valid = test.ValidateCustomer(customer);

            Assert.True(valid);
        }

        [Fact]
        public void InvalidProductRequest_NoDetails()
        {
            List<OrderDetails> list = new List<OrderDetails>();

            bool valid = test.ValidateProductRequest(list);

            Assert.False(valid);
        }

        [Fact]
        public void InvalidProductRequest_TooManyProducts()
        {
            List<ProductComponent> pcl = new List<ProductComponent>();
            pcl.Add(new ProductComponent { Component = new Component { Name = "Name", Cost = (decimal)2.00 } });
            Product p = new Product("test", pcl);

            OrderDetails od = new OrderDetails
            {
                Product = p,
                Quantity = 1000
            };

            List<OrderDetails> list = new List<OrderDetails>();
            list.Add(od);

            bool valid = test.ValidateProductRequest(list);

            Assert.False(valid);
        }

        [Fact]
        public void ValidProductRequest()
        {
            List<ProductComponent> pcl = new List<ProductComponent>();
            pcl.Add(new ProductComponent { Component = new Component { Name = "Name", Cost = (decimal)2.00 } });
            Product p = new Product("test", pcl);

            OrderDetails od = new OrderDetails
            {
                Product = p,
                Quantity = 1
            };

            List<OrderDetails> list = new List<OrderDetails>();
            list.Add(od);

            bool valid = test.ValidateProductRequest(list);

            Assert.True(valid);
        }
    }
}
