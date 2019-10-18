using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RatStore.Data;
using Serilog;
using Serilog.Sinks.SystemConsole;

namespace RatStore.Logic
{
    public class DatabaseStore : IDataStore
    {
        #region Properties
        private DbContextOptions<Data.Entities.jacobproject0Context> _options;
        private Data.Entities.jacobproject0Context _context;
        #endregion

        #region Startup and Shutdown
        public DatabaseStore()
        {
            _options = new DbContextOptionsBuilder<Data.Entities.jacobproject0Context>()
                .UseSqlServer(Data.Entities.SecretCode.Sauce)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new Data.Entities.jacobproject0Context(_options);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }

        ~DatabaseStore()
        {
            _context.Dispose();
        }

        public void Initialize()
        {
            
        }

        public bool Connected()
        {
            try
            {
                return _context.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Cleanup()
        {
            _context.SaveChanges();
        }
        #endregion

        #region Customer
        public void AddCustomer(Customer customer)
        {
            Data.Entities.Customer newCustomer = Mapper.MapCustomer(customer);
            _context.Customer.Add(newCustomer);
        }
        public void AddCustomer(string username, string password, string firstName, string middleName, string lastName, string phoneNumber)
        {
            Customer customer = new Customer
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            AddCustomer(customer);
        }
        public Customer GetCustomerByNameAndPhone(string firstName, string lastName, string phoneNumber)
            => _context.Customer.Select(Mapper.MapCustomer).Where(c => c.FirstName == firstName && c.LastName == lastName && c.PhoneNumber == phoneNumber).FirstOrDefault();
        public Customer GetCustomerById(int id)
            => Mapper.MapCustomer(_context.Customer.Find(id));
        public Customer GetCustomerByUsernameAndPassword(string username, string password)
            => Mapper.MapCustomer(_context.Customer.Where(c => c.Username == username && c.Password == password).FirstOrDefault()); 
        public List<Customer> GetAllCustomers()
        {
            IQueryable<Data.Entities.Customer> customers = _context.Customer
                .AsNoTracking();

            return customers.Select(Mapper.MapCustomer).ToList();
        }
        public void UpdateCustomer(Customer customer)
        {
            Data.Entities.Customer currentCustomer = _context.Customer.Find(customer.CustomerId);

            if (currentCustomer != null)
            {
                Data.Entities.Customer newCustomer = Mapper.MapCustomer(customer);

                _context.Entry(currentCustomer).CurrentValues.SetValues(newCustomer);

                Log.Information($"Customer with Id {customer.CustomerId} has been updated.");
            }
            else
            {
                Log.Error($"Customer with Id {customer.CustomerId} could not be updated because it does not exist.");
            }
        }
        public void RemoveCustomer(int id)
        {
            Data.Entities.Customer customer = _context.Customer.Find(id);

            if (customer != null)
            {
                _context.Customer.Remove(customer);

                Log.Information($"Customer with Id {customer.CustomerId} has been deleted.");
            }
            else
            {
                Log.Error($"Customer with Id {customer.CustomerId} could not be deleted because it does not exist.");
            }
        }
        #endregion

        #region Location
        public void AddLocation(Location location)
        {
            Data.Entities.Location newLocation = Mapper.MapLocation(location);
            _context.Location.Add(newLocation);
        }
        public Location GetLocationById(int id)
            => Mapper.MapLocation(_context.Location
                .Include(l => l.Inventory)
                .ThenInclude(i => i.Component)
                .Include(l => l.Order)
                .First(l => l.LocationId == id));
        public List<Location> GetAllLocations()
        {
            IQueryable<Data.Entities.Location> locations = _context.Location
                .Include(l => l.Inventory)
                .ThenInclude(i => i.Component)
                .Include(l => l.Order)
                .AsNoTracking();

            return locations.Select(Mapper.MapLocation).ToList();
        }
        public void UpdateLocation(Location location)
        {
            Data.Entities.Location currentLocation = _context.Location
                .Include(l => l.Inventory).FirstOrDefault(l => l.LocationId == location.LocationId);

            if (currentLocation != null)
            {
                foreach (Data.Entities.Inventory item in currentLocation.Inventory)
                {
                    item.Quantity = location.Inventory.Find(i => i.Component.ComponentId == item.ComponentId).Quantity;
                }

                Log.Information($"Location with Id {location.LocationId} has been updated.");
            }
            else
            {
                Log.Error($"Location with Id {location.LocationId} could not be updated because it does not exist.");
            }
        }
        public void RemoveLocation(int id)
        {
            Data.Entities.Location location = _context.Location.Find(id);

            if (location != null)
            {
                _context.Location.Remove(location);

                Log.Information($"Location with Id {location.LocationId} has been deleted.");
            }
            else
            {
                Log.Error($"Location with Id {location.LocationId} could not be removed because it does not exist.");
            }
        }
        #endregion

        #region Product
        public void AddProduct(Product product)
        {
            Data.Entities.Product newProduct = Mapper.MapProduct(product);
            _context.Product.Add(newProduct);
        }
        public Product GetProductByName(string name)
            => Mapper.MapProduct(_context.Product
                .Include(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .First(p => p.Name == name));
        public List<Product> GetAllProducts()
        {
            IQueryable<Data.Entities.Product> products = _context.Product
                .Include(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .AsNoTracking();

            return products.Select(Mapper.MapProduct).ToList();
        }
        public void UpdateProduct(Product product)
        {
            Data.Entities.Product currentProduct = _context.Product.Find(product.ProductId);

            if (currentProduct != null)
            {
                Data.Entities.Product newProduct = Mapper.MapProduct(product);

                _context.Entry(currentProduct).CurrentValues.SetValues(newProduct);

                Log.Information($"Product with Id {product.ProductId} has been updated.");
            }
            else
            {
                Log.Error($"Product with Id {product.ProductId} could not be updated because it does not exist.");
            }
        }
        public void RemoveProduct(int id)
        {
            Data.Entities.Product product = _context.Product.Find(id);

            if (product != null)
            {
                _context.Product.Remove(product);

                Log.Information($"Product with Id {product.ProductId} has been deleted.");
            }
            else
            {
                Log.Error($"Product with Id {id} could not be removed because it does not exist.");
            }
        }
        #endregion

        #region Component
        public void AddComponent(Component component)
        {
            Data.Entities.Component newComponent = Mapper.MapComponent(component);
            _context.Component.Add(newComponent);
        }
        public Component GetComponentByName(string name)
            => _context.Component.Select(Mapper.MapComponent).Where(c => c.Name == name).FirstOrDefault();
        public Component GetComponentById(int id)
            => Mapper.MapComponent(_context.Component.AsNoTracking().FirstOrDefault(c => c.ComponentId == id));
        public List<Component> GetAllComponents()
        {
            IQueryable<Data.Entities.Component> component = _context.Component
                .AsNoTracking();

            return component.Select(Mapper.MapComponent).ToList();
        }
        public void UpdateComponent(Component component)
        {
            Data.Entities.Component currentComponent = _context.Component.AsNoTracking().FirstOrDefault(c => c.ComponentId == component.ComponentId);

            if (currentComponent != null)
            {
                Data.Entities.Component newComponent = Mapper.MapComponent(component);

                _context.Entry(currentComponent).CurrentValues.SetValues(newComponent);

                Log.Information($"Component with Id {component.ComponentId} has been updated.");
            }
            else
            {
                Log.Error($"Component with Id {component.ComponentId} could not be updated because it does not exist.");
            }
        }
        public void RemoveComponent(int id)
        {
            Data.Entities.Component component = _context.Component.FirstOrDefault(c => c.ComponentId == id);

            if (component != null)
            {
                _context.Component.Remove(component);

                Log.Information($"Component with Id {component.ComponentId} has been removed.");
            }
            else
            {
                Log.Error($"Component with Id {component.ComponentId} could not be removed because it does not exist.");
            }
        }
        #endregion

        #region Order
        public void AddOrder(Order order)
            => _context.Order.Add(Mapper.MapOrder(order));
        public Order GetOrderById(int id)
            => Mapper.MapOrder(_context.Order
                .Include(o => o.Customer)
                .Include(o => o.Location)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ThenInclude(p => p.ProductComponent)
                .ThenInclude(pc => pc.Component)
                .First(o => o.OrderId == id));
        public List<Order> GetOrderHistory(int customerId = 0)
        {
            IQueryable<Data.Entities.Order> orders = _context.Order
                    .AsNoTracking();

            try
            {
                if (customerId == 0)
                {
                    return orders.Include(o => o.Location)
                        .ThenInclude(l => l.Inventory)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductComponent)
                        .ThenInclude(pc => pc.Component)
                        .Include(o => o.Customer)
                        .Select(Mapper.MapOrder)
                        .ToList();
                }
                else
                {
                    return orders.Include(o => o.Location)
                        .ThenInclude(l => l.Inventory)
                        .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.ProductComponent)
                        .ThenInclude(pc => pc.Component)
                        .Include(o => o.Customer)
                        .Select(Mapper.MapOrder)
                        .Where(o => o.CustomerId == customerId)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                if (customerId != 0)
                    Log.Error($"Order history for customer with Id {customerId} could not be retrieved.");
                else
                    Log.Error($"Order history could not be accessed.");
            }

            return new List<Order>();
        }

        public void UpdateOrder(Order order)
        {
            Data.Entities.Order currentOrder = _context.Order.Find(order.OrderId);

            if (order != null)
            {
                Data.Entities.Order newOrder = Mapper.MapOrder(order);

                _context.Entry(currentOrder).CurrentValues.SetValues(newOrder);

                Log.Information($"Order with Id {order.OrderId} has been updated.");
            }
            else
            {
                Log.Debug($"Order with Id {order.OrderId} could not be updated because it does not exist.");
            }
        }
        public void RemoveOrder(int id)
        {
            Data.Entities.Order customer = _context.Order.Find(id);

            if (customer != null)
            {
                _context.Order.Remove(customer);
                Log.Information($"Order with Id {id} has been removed.");
            }
            else
            {
                Log.Information($"Order with id {id} could not be removed because it does not exist.");
            }
        }
        #endregion
    }
}
