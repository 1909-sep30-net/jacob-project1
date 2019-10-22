using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    public class Location
    {
        public IDataStore DataStore { get; set; }

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

        public Location(IDataStore dataStore)
        {
            Inventory = new List<Inventory>();
            AvailableProducts = new List<Product>();
            OrderHistory = new List<Order>();

            DataStore = dataStore;
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
        /// Constructs a dictionary of Products that the store can theoretically provide and the maximum quantity.
        /// </summary>
        /// <returns>List of Products</returns>
        public Dictionary<Product, int> GetAvailableProducts()
        {
            Dictionary<Product, int> availableProducts = new Dictionary<Product, int>();
            List<Product> allProducts = DataStore.GetAllProducts();

            foreach (Product p in allProducts)
            {
                availableProducts.Add(p, FindMaximumQty(p));
            }

            return availableProducts;
        }
        /// <summary>
        /// Constructs a dictionary of Products that the store can theoretically provide and the maximum quantity.
        /// This dictionary takes into account the potential order represented by cart.
        /// </summary>
        /// <returns>List of Products</returns>
        public Dictionary<Product, int> GetAvailableProducts(List<OrderDetails> cart)
        {
            Dictionary<Product, int> availableProducts = new Dictionary<Product, int>();
            List<Product> allProducts = DataStore.GetAllProducts();

            foreach (Product p in allProducts)
            {
                if (cart.Exists(od => od.Product.ProductId == p.ProductId))
                    availableProducts.Add(p, FindMaximumQty(p) - cart.Find(od => od.Product.ProductId == p.ProductId).Quantity);
                else availableProducts.Add(p, FindMaximumQty(p));
            }

            return availableProducts;
        }
        /// <summary>
        /// Uses binary search to find the highest quantity the current inventory can fulfill.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        int FindMaximumQty(Product product)
        {
            int low = 1, high = 100;
            int qty;

            foreach (ProductComponent pc in product.Ingredients)
            {
                Inventory item = Inventory.Find(i => i.Component.ComponentId == pc.Component.ComponentId);
                if (item == null || item.Quantity < pc.Quantity)
                    return 0;
            }

            do
            {
                qty = low + ((high - low + 1) / 2);

                if (CanFulfillProductQty(new OrderDetails { Product = product, Quantity = qty }))
                {
                    low = qty;
                }
                else
                {
                    high = qty;
                }
            } while (high - low > 1);

            return qty;
        }
        /// <summary>
        /// Checks whether the store's inventory can provide a single product at a given quantitiy, stored in an OrderDetail.
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
        /// Checks that a store can fulfill an entire order as a whole.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>True ifthe store has required ingredients, false otherwise.</returns>
        public bool CanFulfillOrder(List<OrderDetails> orderDetails)
        {
            Dictionary<Component, int> inventoryOrder = new Dictionary<Component, int>();

            // Get the total number of each component required.
            foreach (OrderDetails od in orderDetails)
            {
                foreach (ProductComponent c in od.Product.Ingredients)
                {
                    if (inventoryOrder.ContainsKey(c.Component))
                    {
                        inventoryOrder[c.Component] += c.Quantity*od.Quantity;
                    }
                    else
                    {
                        inventoryOrder.Add(c.Component, c.Quantity*od.Quantity);
                    }
                }
            }

            // CHeck if the inventory matches or exceeds the needed number
            foreach (Inventory i in Inventory)
            {
                if (inventoryOrder.ContainsKey(i.Component) && i.Quantity < inventoryOrder[i.Component])
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

            if (!CanFulfillOrder(o.OrderDetails))
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
        /// <summary>
        /// Checks for invalid Id and invalid Address.
        /// </summary>
        /// <param name="location"></param>
        /// <returns>True if valid, false otherwise.</returns>
        public bool ValidateLocation(Location location)
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
        public bool ValidateCustomer(Customer customer)
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
        public bool ValidateProductRequest(List<OrderDetails> orderDetails)
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
        public bool ValidateOrder(Order order)
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
