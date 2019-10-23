using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RatStore.Logic;
using RatStore.Data;

namespace RatStore.WebApp.Controllers
{
    public class RatStoreController : Controller
    {
        IDataStore _dataStore;
        Models.IBaseViewModel _baseViewModel;

        public RatStoreController(IDataStore dataStore, Models.IBaseViewModel baseViewModel
            , [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            _dataStore = dataStore;
            _baseViewModel = baseViewModel;
        }

        // GET: RatStore
        public ActionResult Index()
        {
            return View(_baseViewModel);
        }

        // GET: RatStore/Cart/
        public ActionResult Cart()
        {
            return View(_baseViewModel);
        }

        // GET: RatStore/Profile/
        public ActionResult Profile()
        {
            return View(_baseViewModel);
        }

        // GET: RatStore/RemoveItem/5
        public ActionResult RemoveItem(int id)
        {
            try
            {
                Cart tempCart = _baseViewModel.Cart;
                OrderDetails target = tempCart.OrderDetails.Find(od => od.Product.ProductId == id);
                tempCart.OrderDetails.Remove(target);
                _baseViewModel.Cart = tempCart;

                return RedirectToAction(nameof(Cart));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: RatStore/SubmitOrder
        public ActionResult SubmitOrder()
        {
            Location tempLocation = _baseViewModel.CurrentLocation;
            tempLocation.SubmitOrder(_baseViewModel.CurrentCustomer, _baseViewModel.Cart.OrderDetails);

            _baseViewModel.Cart = new Cart();
            _baseViewModel.CurrentLocation = _dataStore.GetLocationById(_baseViewModel.CurrentLocation.LocationId);

            return RedirectToAction(nameof(Index));
        }

        // GET: RatStore/OrderHistory
        public ActionResult OrderHistory()
        {
            Models.OrderHistoryViewModel viewModel = new Models.OrderHistoryViewModel((Models.BaseViewModel)_baseViewModel);
            viewModel.OrderHistory = _dataStore.GetOrderHistory(_baseViewModel.CurrentCustomer.CustomerId);

            return View(viewModel);
        }

        #region AddToCart
        // GET: RatStore/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            try
            { 
                Models.AddToCartViewModel viewModel = new Models.AddToCartViewModel((Models.BaseViewModel)_baseViewModel);
                viewModel.Product = _dataStore.GetProductById(id);
                return View(viewModel);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RatStore/AddToCart/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(IFormCollection collection)
        {
            try
            {
                // Make a temp copy
                Cart tempCart = new Cart();
                foreach (OrderDetails orderDetails in _baseViewModel.Cart.OrderDetails)
                {
                    tempCart.OrderDetails.Add(orderDetails);
                }

                // Add the request to it
                tempCart.AddProductToCart(_baseViewModel.CurrentCustomer,
                    _baseViewModel.CurrentLocation,
                    _dataStore.GetProductById(int.Parse(collection["Product.ProductId"].ToString())),
                    int.Parse(collection["Quantity"].ToString())
                    );

                //Check that the temp copy could be fulfilled before accepting it.
                if (_baseViewModel.CurrentLocation.CanFulfillOrder(tempCart.OrderDetails))
                {
                    _baseViewModel.Cart = tempCart;
                    return RedirectToAction(nameof(Index));
                }

                // Error handling
                Models.AddToCartViewModel viewModel = new Models.AddToCartViewModel((Models.BaseViewModel)_baseViewModel);
                viewModel.Product = _dataStore.GetProductById(int.Parse(collection["Product.ProductId"].ToString()));
                ModelState.AddModelError("Quantity", "Store cannot fulfill this quantity.");
                return View(viewModel);
            }
            catch
            {
                return View(new Models.AddToCartViewModel((Models.BaseViewModel)_baseViewModel));
            }
        }
        #endregion

        #region SelectStore
        // GET: RatStore/SelectStore/5
        public ActionResult SelectStore()
        {
            Models.SelectStoreViewModel viewModel = new Models.SelectStoreViewModel((Models.BaseViewModel)_baseViewModel);
            viewModel.Locations = _dataStore.GetAllLocations();

            return View(viewModel);
        }
        
        public ActionResult ChooseStore(int id)
        {
            try
            {
                _baseViewModel.CurrentLocation = _dataStore.GetLocationById(id);
                _baseViewModel.CurrentCustomer.PreferredStoreId = id;

                // Clear the cart
                _baseViewModel.Cart = new Cart();

                return RedirectToAction(nameof(Profile));
            }
            catch
            {
                return RedirectToAction(nameof(Profile));
            }
        }
        #endregion

        #region CreateCustomer
        // GET: RatStore/CreateCustomer
        public ActionResult CreateCustomer()
        {
            return View(new Models.CreateCustomerViewModel((Models.BaseViewModel)_baseViewModel));
        }

        // POST: RatStore/CreateCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer(IFormCollection collection)
        {
            try
            {
                Customer newCustomer = new Customer
                {
                    Username = collection["Username"].ToString(),
                    Password = collection["Password"].ToString(),
                    FirstName = collection["FirstName"].ToString(),
                    MiddleName = collection["MiddleName"].ToString(),
                    LastName = collection["LastName"].ToString(),
                    PhoneNumber = collection["PhoneNumber"].ToString(),
                    PreferredStoreId = 1
                };

                if (_dataStore.GetAllCustomers().Exists(c => c.Username == newCustomer.Username))
                {
                    Models.CreateCustomerViewModel viewModel = new Models.CreateCustomerViewModel((Models.BaseViewModel)_baseViewModel);
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(viewModel);
                }

                _dataStore.AddCustomer(newCustomer);
                _dataStore.Save();

                return RedirectToAction(nameof(LogIn));
            }
            catch
            {
                return View(new Models.CreateCustomerViewModel((Models.BaseViewModel)_baseViewModel));
            }
        }
        #endregion

        #region LogIn
        // GET: RatStore/LogIn
        public ActionResult LogIn()
        {
            return View(new Models.LogInViewModel((Models.BaseViewModel)_baseViewModel));
        }

        // POST: RatStore/LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(IFormCollection collection)
        {
            try
            {
                _baseViewModel.CurrentCustomer = _dataStore.GetCustomerByUsernameAndPassword(collection["Username"], collection["Password"]);
                _baseViewModel.CurrentLocation = _dataStore.GetLocationById(_baseViewModel.CurrentCustomer.PreferredStoreId);

                // TODO: Handle non-existent/incorrect credentials w/ error message
                if (_baseViewModel.CurrentCustomer == null)
                    throw new NullReferenceException("Customer not found.");

                _baseViewModel.LoggedIn = true;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new Models.LogInViewModel((Models.BaseViewModel)_baseViewModel));
            }
        }
        #endregion

        #region Prefab
        #region Details
        // GET: RatStore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region Create
        // GET: RatStore/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RatStore/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Edit
        // GET: RatStore/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RatStore/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Delete
        // GET: RatStore/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RatStore/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #endregion
        #endregion
    }
}