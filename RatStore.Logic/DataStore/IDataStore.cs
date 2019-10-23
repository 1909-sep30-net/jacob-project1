using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    public interface IDataStore
    {
        #region Startup and Shutdown
        /// <summary>
        /// Do any startup actions, such as connection.
        /// </summary>
        void Initialize();
        /// <summary>
        /// Returns true if the database is connected.
        /// </summary>
        /// <returns></returns>
        bool Connected();
        /// <summary>
        /// Save changes to the data store.
        /// </summary>
        void Save();
        /// <summary>
        /// Do any required cleanup, such as disposal.
        /// </summary>
        void Cleanup();
        #endregion

        #region Customer
        /// <summary>
        /// Add a Customer to the data store.
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(Customer customer);
        /// <summary>
        /// Get a Customer from the data store by name and phone.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Customer GetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber);
        /// <summary>
        /// Get a Customer from the data store by a known Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Customer GetCustomerById(int id);
        /// <summary>
        /// Gets the Customer by their username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Customer GetCustomerByUsernameAndPassword(string username, string password = "");
        /// <summary>
        /// Gets a list in memory of all customers in the data store.
        /// </summary>
        /// <returns></returns>
        List<Customer> GetAllCustomers();
        /// <summary>
        /// Updates a Customer's information in the data store based on the Id of the given customer.
        /// </summary>
        /// <param name="customer"></param>
        void UpdateCustomer(Customer customer);
        /// <summary>
        /// Removes the Customer at id from the data store.
        /// </summary>
        /// <param name="id"></param>
        void RemoveCustomer(int id);
        #endregion

        #region Location
        /// <summary>
        /// Adds a new Location to the data store.
        /// </summary>
        /// <param name="location"></param>
        void AddLocation(Location location);
        /// <summary>
        /// Gets a Location from the data store by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Location GetLocationById(int id);
        /// <summary>
        /// Gets a list of all Locations in memory from the data store.
        /// </summary>
        /// <returns></returns>
        List<Location> GetAllLocations();
        /// <summary>
        /// Updates a Location's inventory in the data store according to the given Location's Id.
        /// </summary>
        /// <param name="location"></param>
        void UpdateLocation(Location location);
        /// <summary>
        /// Removes the Location with the given id from the data store.
        /// </summary>
        /// <param name="id"></param>
        void RemoveLocation(int id);
        #endregion

        #region Product
        /// <summary>
        /// Adds a new Product to the data store.
        /// </summary>
        /// <param name="product"></param>
        void AddProduct(Product product);
        /// <summary>
        /// Gets the Product with the given name from the data store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Product GetProductByName(string name);
        /// <summary>
        /// Gets the Product with the given id from the data store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Product GetProductById(int id);
        /// <summary>
        /// Gets a list of all Products in memory from the data store.
        /// </summary>
        /// <returns></returns>
        List<Product> GetAllProducts();
        /// <summary>
        /// Updates the Product with the given Id with the given Product.
        /// </summary>
        /// <param name="product"></param>
        void UpdateProduct(Product product);
        /// <summary>
        /// Removes the Product with the given id from the data store.
        /// </summary>
        /// <param name="id"></param>
        void RemoveProduct(int id);
        #endregion

        #region Component
        /// <summary>
        /// Adds a new Component to the data store.
        /// </summary>
        /// <param name="component"></param>
        void AddComponent(Component component);
        /// <summary>
        /// Gets the Component with the matching name from the data store.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Component GetComponentByName(string name);
        /// <summary>
        /// Gets the Component with the matching Id from the data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Component GetComponentById(int id);
        /// <summary>
        /// Gets a list of all Components in memory from the data store.
        /// </summary>
        /// <returns></returns>
        List<Component> GetAllComponents();
        /// <summary>
        /// Updates the Component in the data store with the matching ComponentId with values in component.
        /// </summary>
        /// <param name="component"></param>
        void UpdateComponent(Component component);
        /// <summary>
        /// Removes the Component from the data store with the matching ComponentId.
        /// </summary>
        /// <param name="id"></param>
        void RemoveComponent(int id);
        #endregion

        #region Order
        /// <summary>
        /// Adds a new Order to the data store.
        /// </summary>
        /// <param name="order"></param>
        void AddOrder(Order order);
        /// <summary>
        /// Gets the Order with the matching Id from the data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Order GetOrderById(int id);
        /// <summary>
        /// Gets a list of Orders from the data store. If a customerId is given, only that Customer's Orders will be retrieved.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        List<Order> GetOrderHistory(int customerId = 0);
        /// <summary>
        /// Updates the Order in the data store with the given Order.
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrder(Order order);
        /// <summary>
        /// Deletes the Order in the data store with the matching Id.
        /// </summary>
        /// <param name="id"></param>
        void RemoveOrder(int id);
        #endregion
    }
}
