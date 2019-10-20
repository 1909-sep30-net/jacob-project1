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
        static Models.BaseViewModel _baseViewModel = new Models.BaseViewModel();

        public RatStoreController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        // GET: RatStore
        public ActionResult Index()
        {
            return View(_baseViewModel);
        }

        // GET: RatStore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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

        #region CreateCustomer
        // GET: RatStore/CreateCustomer
        public ActionResult CreateCustomer()
        {
            return View(new Models.CreateCustomerViewModel(_baseViewModel));
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
                    PhoneNumber = collection["PhoneNumber"].ToString()
                };

                // Validate customer, throw exception for bad entries

                _dataStore.AddCustomer(newCustomer);
                _dataStore.Save();

                return RedirectToAction(nameof(LogIn));
            }
            catch
            {
                return View(new Models.CreateCustomerViewModel(_baseViewModel));
            }
        }
        #endregion

        #region LogIn
        // GET: RatStore/LogIn
        public ActionResult LogIn()
        {
            return View(new Models.LogInViewModel(_baseViewModel));
        }

        // POST: RatStore/LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(IFormCollection collection)
        {
            try
            {
                _baseViewModel.CurrentCustomer = _dataStore.GetCustomerByUsernameAndPassword(collection["Username"], collection["Password"]);

                // TODO: Handle non-existent/incorrect credentials w/ error message
                if (_baseViewModel.CurrentCustomer == null)
                    throw new NullReferenceException("Customer not found.");

                _baseViewModel.LoggedIn = true;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(new Models.LogInViewModel(_baseViewModel));
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
    }
}