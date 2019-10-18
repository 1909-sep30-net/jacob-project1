using System;
using System.Collections.Generic;
using System.Text;
using RatStore.Data;

namespace RatStore.Logic
{
    public class RatStore : Location
    {
        public RatStore(IDataStore newDataStore)
        {
            DataStore = newDataStore;

            DataStore.Initialize();

            try
            {
                ChangeLocation(1);
            }
            catch
            {
                // Add test location if none available
                Address = "123 Test St, Everett, WA 98203";
                LocationId = 1;
                DataStore.AddLocation(this);
                DataStore.Save();

                ChangeLocation(1);
            }
        }

        /// <summary>
        /// Constructs a list of Products that the store can provide at least one of.
        /// </summary>
        /// <returns>List of Products</returns>
        public override List<Product> GetAvailableProducts()
        {
            List<Product> availableProducts = new List<Product>();
            List<Product> allProducts = DataStore.GetAllProducts();
            for (int i = 0; i < allProducts.Count; ++i)
            {
                OrderDetails tempDetail = new OrderDetails
                {
                    Product = allProducts[i],
                    Quantity = 1
                };

                if (CanFulfillProductQty(tempDetail))
                {
                    availableProducts.Add(allProducts[i]);
                }
            }

            return availableProducts;
        }

        #region Validation
        /// <summary>
        /// Checks for invalid Id and invalid Address.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool ValidateLocation(Location location)
        {
            if (location.LocationId == 0
                || location.Address == "")
                return false;

            return true;
        }
        /// <summary>
        /// Checks that the Customer has a first name, last name, and phone number.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool ValidateCustomer(Customer customer)
        {
            if (customer.FirstName == ""
                || customer.LastName == ""
                || customer.PhoneNumber == "")
                return false;

            return true;
        }
        /// <summary>
        /// Checks that the list of OrderDetails has a valid product quantity (1 <= q <= 100).
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool ValidateProductRequest(List<OrderDetails> orderDetails)
        {
            if (orderDetails.Count == 0)
                return false;

            foreach (OrderDetails detail in orderDetails)
            {
                if (detail.Quantity > 100
                    || detail.Quantity < 1)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Checks that the Order has at least one OrderDetail.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>True if valid, false otherwise.</returns>
        public override bool ValidateOrder(Order order)
        {
            if (order == null
                || order.OrderDetails == null
                || order.OrderDetails.Count == 0)
                return false;

            return true;
        }
        #endregion
    }
}
