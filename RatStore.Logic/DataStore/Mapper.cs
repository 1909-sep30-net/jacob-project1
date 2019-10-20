using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RatStore.Data;

namespace RatStore.Logic
{
    public static class Mapper
    {
        #region Component
        /// <summary>
        /// Maps a db component to a business Component.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static Component MapComponent(Data.Entities.Component component)
        {
            return new Component
            {
                ComponentId = component.ComponentId,
                Name = component.Name,
                Cost = component.Cost ?? throw new ArgumentException("Argument cannot be null", nameof(component))
            };
        }
        /// <summary>
        /// Maps a business component to a db Component.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static Data.Entities.Component MapComponent(Component component)
        {
            return new Data.Entities.Component
            {
                ComponentId = component.ComponentId,
                Name = component.Name,
                Cost = component.Cost
            };
        }
        #endregion

        #region ProductComponent
        /// <summary>
        /// Maps a db ProductComponent to a business ProductComponent.
        /// </summary>
        /// <param name="productComponent"></param>
        /// <returns></returns>
        public static ProductComponent MapProductComponent(Data.Entities.ProductComponent productComponent)
        {
            return new ProductComponent
            {
                Component = MapComponent(productComponent.Component),
                Quantity = productComponent.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(productComponent))
            };
        }
        /// <summary>
        /// Maps a business ProductComponent to a db ProductComponent.
        /// </summary>
        /// <param name="productComponent"></param>
        /// <returns></returns>
        public static Data.Entities.ProductComponent MapProductComponent(ProductComponent productComponent)
        {
            return new Data.Entities.ProductComponent
            {
                ComponentId = productComponent.Component.ComponentId,
                Quantity = productComponent.Quantity 
            };
        }
        #endregion

        #region Product
        /// <summary>
        /// Maps a business Product to a db Product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static Product MapProduct(Data.Entities.Product product)
        {
            return new Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Ingredients = product.ProductComponent.Select(MapProductComponent).ToList()
            };
        }
        /// <summary>
        /// Maps a business Product to a db Product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static Data.Entities.Product MapProduct(Product product)
        {
            return new Data.Entities.Product
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ProductComponent = product.Ingredients.Select(MapProductComponent).ToList()
            };
        }
        #endregion

        #region Customer
        /// <summary>
        /// Maps a business Customer to a db Customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static Customer MapCustomer(Data.Entities.Customer customer)
        {
            return new Customer
            {
                CustomerId = customer.CustomerId,
                Username = customer.Username,
                Password = customer.Password,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
        /// <summary>
        /// Maps a business Customer to a db Customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static Data.Entities.Customer MapCustomer(Customer customer)
        {
            return new Data.Entities.Customer
            {
                CustomerId = customer.CustomerId,
                Username = customer.Username,
                Password = customer.Password,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber
            };
        }
        #endregion

        #region Inventory
        /// <summary>
        /// Maps a business Inventory to a db Inventory.
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public static Inventory MapInventory(Data.Entities.Inventory inventoryItem)
        {
            return new Inventory
            {
                Component = MapComponent(inventoryItem.Component),
                Quantity = inventoryItem.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(inventoryItem))
            };
        }
        /// <summary>
        /// Maps a business Inventory to a db Inventory.
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public static Data.Entities.Inventory MapInventory(Inventory inventoryItem)
        {
            return new Data.Entities.Inventory
            {
                ComponentId = inventoryItem.Component.ComponentId,
                Quantity = inventoryItem.Quantity
            };
        }
        #endregion

        #region Location
        /// <summary>
        /// Maps a business Location to a db Location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Location MapLocation(Data.Entities.Location location)
        {
            return new Location
            {
                LocationId = location.LocationId,
                Address = location.Address,
                Inventory = location.Inventory.Select(MapInventory).ToList()
            };
        }
        /// <summary>
        /// Maps a business Location to a db Location.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static Data.Entities.Location MapLocation(Location location)
        {
            return new Data.Entities.Location
            {
                LocationId = location.LocationId,
                Address = location.Address,
                Inventory = location.Inventory.Select(MapInventory).ToList()
            };
        }
        #endregion

        #region OrderDetails
        /// <summary>
        /// Maps a business OrderDetails to a db OrderDetails.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public static OrderDetails MapOrderDetails(Data.Entities.OrderDetails orderDetails)
        {
            return new OrderDetails
            {
                Product = MapProduct(orderDetails.Product),
                Quantity = orderDetails.Quantity ?? throw new ArgumentException("Argument cannot be null", nameof(orderDetails))
            };
        }
        /// <summary>
        /// Maps a business OrderDetails to a db OrderDetails.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public static Data.Entities.OrderDetails MapOrderDetails(OrderDetails orderDetails)
        {
            return new Data.Entities.OrderDetails
            {
                ProductId = orderDetails.Product.ProductId,
                Quantity = orderDetails.Quantity
            };
        }
        #endregion

        #region Order
        /// <summary>
        /// Maps a business Order to a db Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Order MapOrder(Data.Entities.Order order)
        {
            return new Order
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId ?? throw new ArgumentException("Argument cannot be null", nameof(order)),
                LocationId = order.LocationId ?? throw new ArgumentException("Argument cannot be null", nameof(order)),
                OrderDetails = order.OrderDetails.Select(MapOrderDetails).ToList(),
                OrderDate = order.OrderDate ?? throw new ArgumentException("Argument cannot be null", nameof(order))
            };
        }
        /// <summary>
        /// Maps a business Order to a db Order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Data.Entities.Order MapOrder(Order order)
        {
            return new Data.Entities.Order
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                LocationId = order.LocationId,
                OrderDetails = order.OrderDetails.Select(MapOrderDetails).ToList(),
                OrderDate = order.OrderDate
            };
        }
        #endregion
    }
}
