using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RatStore.Data;
using RatStore.Logic;

namespace RatStore.UI
{
    static class ConsoleExtensions
    {
        #region RatStore
        public static void PrintStoreInformation(this Location location)
        {
            Console.WriteLine($"You are currently at store {location.LocationId}.");
            Console.WriteLine($"This store has {location.AvailableProducts.Count} different products you can choose from:");
            PrintAvailableProducts(location);
        }

        public static void PrintCustomerInformation(this Location location, int customerId)
        {
            try
            {
                Customer customer = location.DataStore.GetCustomerById(customerId);
                Console.WriteLine($"Records for customer {customer.CustomerId}:");

                string middle = (customer.MiddleName != null && customer.MiddleName != "") ? customer.MiddleName + " " : "";
                Console.WriteLine($"Name: {customer.FirstName} {middle}{customer.LastName}"); // Weird spacing on purpose
                Console.WriteLine($"Phone Number: {customer.PhoneNumber}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void PrintInventory(this Location location) //(of components)
        {
            Console.WriteLine($"The following ingredients and stocks are available at store {location.LocationId}:");
            foreach (Inventory inventoryItem in location.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                    Console.WriteLine($"   {inventoryItem.Component.Name} x {inventoryItem.Quantity} @ {String.Format("{0:C}", inventoryItem.Component.Cost)}");
            }
        }

        public static void PrintAvailableProducts(this Location location)
        {
            List<Product> availableProducts = location.GetAvailableProducts().Keys.ToList();
            if (availableProducts.Count == 0)
                Console.WriteLine("This store has no products to sell! Please type 'end' and try another store.");
            else
                Console.WriteLine($"The following products are available at store {location.LocationId}:");

            for (int i = 0; i < availableProducts.Count; ++i)
            {
                Console.WriteLine($"{i}: {availableProducts[i].Name} -- @ {string.Format("{0:C}", availableProducts[i].Cost)}");
                foreach (ProductComponent ingredient in availableProducts[i].Ingredients)
                {
                    Console.WriteLine($"    {ingredient.Component.Name} x {ingredient.Quantity} @ {String.Format("{0:C}", ingredient.Component.Cost)}");
                }
            }
        }

        public static void PrintAllLocations(this Location thisLocation)
        {
            Console.WriteLine($"The following locations exist within the company: ");
            List<Location> allLocations = thisLocation.DataStore.GetAllLocations();
            for (int i = 0; i < allLocations.Count; ++i)
            {
                if (allLocations[i].LocationId == thisLocation.LocationId)
                    Console.WriteLine($"{allLocations[i].LocationId}: location {allLocations[i].LocationId} at {allLocations[i].Address} (this store)");
                else 
                    Console.WriteLine($"{allLocations[i].LocationId}: location {allLocations[i].LocationId} at {allLocations[i].Address}");
            }
        }

        public static void PrintOrderAtId(this Location location, int id)
        {
            Order order = location.DataStore.GetOrderById(id);
            Console.WriteLine($"{order.OrderId}: {order.Count} {(order.Count > 1 ? "products" : "product")} ordered " +
                $"on {order.OrderDate.ToShortDateString()} for {String.Format("{0:C}", order.Cost)}.");
            foreach (OrderDetails orderDetails in order.OrderDetails)
            {
                Console.WriteLine($"  {orderDetails.Product.Name} x {orderDetails.Quantity} @ {orderDetails.Product.Cost}");
            }
        }

        public static void PrintCustomerOrderHistory(this Location location, int customerId)
        {
            List<Order> customerOrderHistory = location.DataStore.GetOrderHistory(customerId);

            if (customerOrderHistory.Count == 0)
                Console.WriteLine("Order history is empty for this customer.");
            else
                foreach (Order order in customerOrderHistory)
                {
                    PrintOrderAtId(location, order.OrderId);
                }
        }

        public static void PrintLocationOrderHistory(this Location thisLocation)
        {
            if (thisLocation.OrderHistory.Count == 0)
                Console.WriteLine("Order history is empty for this location.");
            else
                foreach (Order order in thisLocation.OrderHistory)
                {
                    Console.WriteLine($"{order.OrderId}: {order.Count} {(order.Count > 1 ? "products" : "product")} ordered " +
                        $"on {order.OrderDate.ToShortDateString()} for {String.Format("{0:C}", order.Cost)}.");
                }
        }
        #endregion

        #region Navigator
        public static void PrintCart(this Navigator navigator)
        {
            Console.WriteLine($"Total: {navigator.Subtotal.ToString("C0")}");
        }
        #endregion
    }
}
