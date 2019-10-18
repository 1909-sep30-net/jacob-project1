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
        Data.Customer _currentCustomer;
        
        public bool LoggedIn { get; set; }

        public RatStoreController()
        {
            _dataStore = RatStoreConfiguration.GetDataStore();
        }

        // GET: RatStore
        public ActionResult Index()
        {
            return View();
        }

        // GET: RatStore/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

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

        // GET: RatStore/Create
        public ActionResult LogIn()
        {
            return View();
        }

        // POST: RatStore/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(IFormCollection collection)
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
    }
}