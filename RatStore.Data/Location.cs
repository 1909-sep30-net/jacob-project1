using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Location
    {
        public IDataStore DataStore { get; protected set; }

        public string Address { get; set; }

        public int LocationId { get; set; }

        virtual public List<Inventory> Inventory { get; set; }

        virtual public List<Product> AvailableProducts { get; protected set; }

        virtual public List<Order> OrderHistory { get; set; }

        public Location()
        {
            Inventory = new List<Inventory>();
            AvailableProducts = new List<Product>();
            OrderHistory = new List<Order>();
        }

        /// <summary>
        /// Changes the current store to the one indicated by the given id. Throws an exception if the id is not found.
        /// </summary>
        /// <param name="targetStoreId"></param>
        public void ChangeLocation(int targetStoreId)
        {
            Location temp = DataStore.GetLocationById(targetStoreId);

            Address = temp.Address;
            LocationId = temp.LocationId;
            Inventory = temp.Inventory;
            AvailableProducts = temp.AvailableProducts;
            OrderHistory = temp.OrderHistory;
        }

        #region Inventory Management
        /// <summary>
        /// Aquires a list of products the store can provide at least one of based on its stock of components.
        /// </summary>
        /// <returns></returns>
        virtual public List<Product> GetAvailableProducts()
        {
            throw new Exception("No data for Location class!");
        }
        /// <summary>
        /// Checks whether the store's inventory can provide a certain product at a given quantitiy, stored in an OrderDetail.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        virtual public bool CanFulfillProductQty(OrderDetails orderDetails)
        {
            foreach (ProductComponent comp in orderDetails.Product.Ingredients)
            {
                Inventory inventoryItem = Inventory.Find(i => i.Component.ComponentId == comp.Component.ComponentId);
                if (inventoryItem == null || comp.Quantity * orderDetails.Quantity > inventoryItem.Quantity)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Checks that a store can fulfill an entire order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>True ifthe store has required ingredients, false otherwise.</returns>
        public bool CanFulfillOrder(Order order)
        {
            foreach (OrderDetails orderDetails in order.OrderDetails)
            {
                if (!CanFulfillProductQty(orderDetails))
                    return false;
            }

            return true;
        }
        #endregion

        #region Order Manipulation
        /// <summary>
        /// Takes a Customer and a list of OrderDetails, validates them, and submits the constructed Order to the database.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="orderDetails"></param>
        public void SubmitOrder(Customer customer, List<OrderDetails> orderDetails)
        {
            if (!ValidateCustomer(customer))
                throw new Exception("Order build failed: invalid customer");
            else if (!ValidateProductRequest(orderDetails))
                throw new Exception("Order build failed: invalid products dictionary");

            Order o = new Order()
            {
                CustomerId = customer.CustomerId,
                LocationId = LocationId,
                OrderDetails = orderDetails,
            };

            if (!ValidateOrder(o))
                throw new Exception("Order must have at least one item");

            if (!CanFulfillOrder(o))
                throw new Exception("Inventory has insufficient components");

            foreach (OrderDetails detail in orderDetails)
            {
                foreach (ProductComponent component in detail.Product.Ingredients)
                {
                    Inventory inventoryItem = Inventory.Find(item => item.Component.ComponentId == component.Component.ComponentId);
                    inventoryItem.Quantity -= component.Quantity * detail.Quantity;
                    DataStore.UpdateLocation(this);
                }
            }

            DataStore.AddOrder(o);
            DataStore.Save();
        }
        #endregion

        #region Validation
        virtual public bool ValidateLocation(Location location)
        {
            return true;
        }
        virtual public bool ValidateCustomer(Customer customer)
        {
            return true;
        }
        virtual public bool ValidateProductRequest(List<OrderDetails> products)
        {
            return true;
        }
        virtual public bool ValidateOrder(Order order)
        {
            return true;
        }
        #endregion
    }
}
