using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Data;

namespace RatStore.Logic
{
    public class TextStore : IDataStore
    {
        #region Properties
        string _path, _customersFile, _locationsFile, _productsFile, _componentsFile, _ordersFile;
        public List<Customer> Customers { get; private set; }
        public List<Location> Locations { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Component> Components { get; private set; }
        public List<Order> Orders { get; private set; }
        #endregion

        #region Startup and Shutdown
        public void Initialize()
        {
            _path = "C:\\Users\\Jacob Davis\\Revature\\jacob-project0\\";

            _customersFile = _path + "Customers.json";
            _locationsFile = _path + "Locations.json";
            _productsFile = _path + "Recipes.json";
            _componentsFile = _path + "Components.json";
            _ordersFile = _path + "Orders.json";

            Customers = new List<Customer>();
            Locations = new List<Location>();
            Products = new List<Product>();
            Components = new List<Component>();
            Orders = new List<Order>();

            LoadStores();
        }

        public bool Connected()
            => true;

        public void Save()
        {
            SaveStores();
        }

        public void Cleanup()
        {
            SaveStores();
        }

        public void PopulateWithTestData()
        {
            #region Components
            Components.Add(new Component { ComponentId = 1, Name = "Big Rat", Cost = (decimal)99.99 });
            Components.Add(new Component { ComponentId = 2, Name = "Small Rat", Cost = (decimal)49.99 });
            Components.Add(new Component { ComponentId = 3, Name = "Micro Rat", Cost = (decimal)19.99 });
            Components.Add(new Component { ComponentId = 4, Name = "Rat Food, 1lb", Cost = (decimal)4.99 });
            Components.Add(new Component { ComponentId = 5, Name = "Rat Cage, Small", Cost = (decimal)19.99 });
            #endregion

            #region Products
            ProductComponent pc;
            Product p;

            p = new Product { ProductId = 1, Name = "Big Rat" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 1), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            Products.Add(p);

            p = new Product { ProductId = 2, Name = "Small Rat" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 2), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            Products.Add(p);

            p = new Product { ProductId = 3, Name = "Micro Rat" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 3), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            Products.Add(p);

            p = new Product { ProductId = 4, Name = "Rat Food, 1lb" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 4), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            Products.Add(p);

            p = new Product { ProductId = 5, Name = "Rat Cage, Small" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 5), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            Products.Add(p);

            /*Rat Package Deal is a Micro Rat, two pounds of food, and a cage */
            p = new Product { ProductId = 6, Name = "Rat Package Deal" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 3), Quantity = 1 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 4), Quantity = 2 };
            p.Ingredients.Add(pc);
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 5), Quantity = 1 };
            p.Ingredients.Add(pc);
            Products.Add(p);

            /* Rat Soup is three Small Rats and a pound of food */
            p = new Product { ProductId = 7, Name = "Rat Soup" };
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 2), Quantity = 3 };
            p.Ingredients = new List<ProductComponent>();
            p.Ingredients.Add(pc);
            pc = new ProductComponent { Component = Components.Find(c => c.ComponentId == 4), Quantity = 1 };
            p.Ingredients.Add(pc);
            Products.Add(p);
            #endregion

            #region Location
            Locations.Add(new Location { LocationId = 1, Address = "1600 Pennsylvania Ave NW, Washington, DC 20500" }); /* White House */
            Locations.Add(new Location { LocationId = 1, Address = "12500 TI Boulevard Dallas, TX 75243" }); /* Texas Instruments */
            Locations.Add(new Location { LocationId = 1, Address = "410 Terry Ave N, Seattle, WA 98109" }); /* Amazon HQ */

            foreach (Location l in Locations)
            {
                l.Inventory = new List<Inventory>();
                foreach (Component c in Components)
                {
                    l.Inventory.Add(new Inventory { Component = c, Quantity = 10 });
                }
            }
            #endregion
        }
        #endregion

        #region Storage
        public void LoadStores()
        {
            try
            {
                if (File.Exists(_customersFile))
                    Customers = JsonConvert.DeserializeObject< List<Customer>>(File.ReadAllText(_customersFile));

                if (File.Exists(_ordersFile))
                    Orders = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(_ordersFile));

                if (!File.Exists(_locationsFile) || !File.Exists(_productsFile) || !File.Exists(_componentsFile))
                {
                    PopulateWithTestData();
                }
                else
                {
                    Locations = JsonConvert.DeserializeObject<List<Location>>(File.ReadAllText(_locationsFile));
                    Products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(_productsFile));
                    Components = JsonConvert.DeserializeObject<List<Component>>(File.ReadAllText(_componentsFile));
                }
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }

        public void SaveStores()
        {
            try
            {
                System.IO.File.WriteAllText(_customersFile, JsonConvert.SerializeObject(Customers));
                System.IO.File.WriteAllText(_locationsFile, JsonConvert.SerializeObject(Locations));
                System.IO.File.WriteAllText(_productsFile, JsonConvert.SerializeObject(Products));
                System.IO.File.WriteAllText(_componentsFile, JsonConvert.SerializeObject(Components));
                System.IO.File.WriteAllText(_ordersFile, JsonConvert.SerializeObject(Orders));
            }
            catch (Exception e)
            {
                // TODO: logging
            }
        }
        #endregion

        #region Customer
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }
        public void AddCustomer(string username, string password, string firstName, string middleName, string lastName, string phoneNumber)
        {
            Customer customer = new Customer()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                CustomerId = Customers.Count,
                PreferredStoreId = 0
            };

            Customers.Add(customer);
        }
        public Customer GetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
        {
            foreach (Customer c in Customers)
            {
                if (c.FirstName == firstName && c.LastName == lastName && c.PhoneNumber == phoneNumber)
                    return c;
            }

            throw new Exception($"No customer named {firstName} {lastName} with phone number {phoneNumber}");
        }
        public Customer GetCustomerById(int id)
        {
            foreach (Customer c in Customers)
            {
                if (c.CustomerId == id)
                    return c;
            }

            throw new Exception($"No customer with given id: {id}");
        }
        public Customer GetCustomerByUsernameAndPassword(string username, string password)
        {
            Customer customer = Customers.Find(c => c.Username == username && c.Password == password);

            if (customer == null)
                throw new Exception($"No customer with given username and password.");

            return customer;
        }
        public List<Customer> GetAllCustomers()
        {
            return new List<Customer>(Customers);
        }
        public int GetNextCustomerId()
        {
            return Customers.Count;
        }

        public void UpdateCustomer(Customer customer)
        {
            Customer real = Customers.Find(c => c.CustomerId == customer.CustomerId);
            real.CustomerId = customer.CustomerId;
            real.FirstName = customer.FirstName;
            real.MiddleName = customer.MiddleName;
            real.LastName = customer.LastName;
            real.PhoneNumber = customer.PhoneNumber;
            real.PreferredStoreId = customer.PreferredStoreId;
        }

        public void RemoveCustomer(int id)
        {
            Customer real = Customers.Find(c => c.CustomerId == id);
            Customers.Remove(real);
        }
        #endregion

        #region Location
        public void AddLocation(Location location)
        {
            Locations.Add(location);
        }
        public Location GetLocationById(int id)
        {
            foreach (Location l in Locations)
            {
                if (l.LocationId == id)
                    return l;
            }

            throw new Exception($"No location found with id: {id}");
        }
        public List<Location> GetAllLocations()
        {
            return new List<Location>(Locations);
        }
        public int GetNextLocationId()
        {
            return Locations.Count;
        }
        public void UpdateLocation(Location location)
        {
            Location real = Locations.Find(l => l.LocationId == location.LocationId);
            real.Address = location.Address;
            real.Inventory = location.Inventory;
            real.LocationId = location.LocationId;
            real.OrderHistory = location.OrderHistory;
        }
        public void RemoveLocation(int id)
        {
            Location real = Locations.Find(l => l.LocationId == id);
            Locations.Remove(real);
        }
        #endregion

        #region Product
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public Product GetProductByName(string name)
        {
            foreach (Product product in Products)
            {
                if (product.Name == name)
                    return product;
            }

            throw new Exception($"No recipe found with name: {name}");
        }
        public List<Product> GetAllProducts()
        {
            return new List<Product>(Products);
        }
        public void UpdateProduct(Product product)
        {
            Product real = Products.Find(p => p.ProductId == product.ProductId);
            real.Name = product.Name;
            real.Ingredients = product.Ingredients;
            real.ProductId = product.ProductId;
        }
        public void RemoveProduct(int id)
        {
            Product real = Products.Find(p => p.ProductId == id);
            Products.Remove(real);
        }
        #endregion

        #region Component
        public void AddComponent(Component component)
        {
            Components.Add(component);
        }
        public Component GetComponentByName(string name)
        {
            foreach (Component c in Components)
            {
                if (c.Name == name)
                    return c;
            }

            throw new Exception($"No component with name: {name}");
        }
        public Component GetComponentById(int id)
            => Components.Find(c => c.ComponentId == id);
        public List<Component> GetAllComponents()
        {
            return new List<Component>(Components);
        }
        public void UpdateComponent(Component component)
        {
            Component real = Components.Find(c => c.ComponentId == component.ComponentId);
            real.ComponentId = component.ComponentId;
            real.Name = component.Name;
        }
        public void RemoveComponent(int id)
        {
            Product real = Products.Find(p => p.ProductId == id);
            Products.Remove(real);
        }
        #endregion

        #region Order
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public Order GetOrderById(int id)
        {
            foreach (Order o in Orders)
            {
                if (o.OrderId == id)
                    return o;
            }

            throw new Exception($"No order with id: {id}");
        }
        public List<Order> GetOrderHistory(int customerId = 0)
        {
            if (customerId == 0)
            {
                return new List<Order>(Orders);
            }
            else return Orders.Where(o => o.CustomerId == customerId).ToList();
        }
        public int GetNextOrderId()
        {
            return Orders.Count;
        }
        public void UpdateOrder(Order order)
        {
            Order real = Orders.Find(o => o.OrderId == order.OrderId);
            real.LocationId = order.LocationId;
            real.OrderDate = order.OrderDate;
            real.OrderDetails = order.OrderDetails.ToList();
            real.OrderId = order.OrderId;
        }
        public void RemoveOrder(int id)
        {
            Order real = Orders.Find(o => o.OrderId == id);
            Orders.Remove(real);
        }
        #endregion
    }
}
