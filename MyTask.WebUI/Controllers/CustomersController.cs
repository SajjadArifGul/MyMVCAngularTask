using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTask.WebUI.Models;
using MyTask.WebUI.ViewModels;
using MyTask.Models;
using Microsoft.AspNet.Identity;

namespace MyTask.WebUI.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.ToList();

            var customersViewModels = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
                var customerViewModel = new CustomerViewModel()
                {
                    ID = customer.Customer_Id_Pk,
                    Name = customer.Customer_Name,
                    Details= customer.Customer_Details,
                    Address= customer.Customer_Address,
                    Contact= customer.Customer_Contact,
                    IsActive= customer.Is_Active,
                    CreatedBy=customer.Created_By,
                    CreatedOn=customer.Created_On,
                    ModifiedBy=customer.Modified_By,
                    ModifiedOn=customer.Modified_On
                };

                var customerNumberDetails = new List<string>();
                var customerNumberValues = new List<string>();


                foreach (var CustomerNumber in customer.CustomerNumbers)
                {
                    customerNumberDetails.Add(CustomerNumber.Customer_Number_Details);
                    customerNumberValues.Add(CustomerNumber.Customer_Number_Value);
                }

                customerViewModel.NumberDetails = customerNumberDetails;
                customerViewModel.NumberValues = customerNumberValues;

                customersViewModels.Add(customerViewModel);
            }

            return View(customersViewModels);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var customerViewModel = new CustomerViewModel()
            {
                ID = customer.Customer_Id_Pk,
                Name = customer.Customer_Name,
                Details = customer.Customer_Details,
                Address = customer.Customer_Address,
                Contact = customer.Customer_Contact,
                IsActive = customer.Is_Active,
                CreatedBy = customer.Created_By,
                CreatedOn = customer.Created_On,
                ModifiedBy = customer.Modified_By,
                ModifiedOn = customer.Modified_On
            };

            var customerNumberDetails = new List<string>();
            var customerNumberValues = new List<string>();


            foreach (var CustomerNumber in customer.CustomerNumbers)
            {
                customerNumberDetails.Add(CustomerNumber.Customer_Number_Details);
                customerNumberValues.Add(CustomerNumber.Customer_Number_Value);
            }

            customerViewModel.NumberDetails = customerNumberDetails;
            customerViewModel.NumberValues = customerNumberValues;
            
            return View(customerViewModel);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Details,Address,Contact,IsActive")] CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer()
                {
                    Customer_Name = customerViewModel.Name,
                    Customer_Details = customerViewModel.Details,
                    Customer_Address = customerViewModel.Address,
                    Customer_Contact = customerViewModel.Contact,
                    Is_Active = customerViewModel.IsActive,
                    Created_By = User.Identity.GetUserName(),
                    Created_On = DateTime.Now
                };

                db.Customers.Add(customer);
                db.SaveChanges();
                
                //add customer numbers here

                return RedirectToAction("Index");
            }

            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var customerViewModel = new CustomerViewModel()
            {
                ID = customer.Customer_Id_Pk,
                Name = customer.Customer_Name,
                Details = customer.Customer_Details,
                Address = customer.Customer_Address,
                Contact = customer.Customer_Contact,
                IsActive = customer.Is_Active,
                CreatedBy = customer.Created_By,
                CreatedOn = customer.Created_On,
                ModifiedBy = customer.Modified_By,
                ModifiedOn = customer.Modified_On
            };

            var customerNumberDetails = new List<string>();
            var customerNumberValues = new List<string>();


            foreach (var CustomerNumber in customer.CustomerNumbers)
            {
                customerNumberDetails.Add(CustomerNumber.Customer_Number_Details);
                customerNumberValues.Add(CustomerNumber.Customer_Number_Value);
            }

            customerViewModel.NumberDetails = customerNumberDetails;
            customerViewModel.NumberValues = customerNumberValues;

            return View(customerViewModel);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Details,Address,Contact,IsActive,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn")] CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = db.Customers.Find(customerViewModel.ID);
                customer.Customer_Name = customerViewModel.Name;
                customer.Customer_Details = customerViewModel.Details;
                customer.Customer_Address = customerViewModel.Address;
                customer.Customer_Contact = customerViewModel.Contact;
                customer.Is_Active = customerViewModel.IsActive;
                customer.Modified_By = User.Identity.GetUserName();
                customer.Modified_On = DateTime.Now;

                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerViewModel);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var customerViewModel = new CustomerViewModel()
            {
                ID = customer.Customer_Id_Pk,
                Name = customer.Customer_Name,
                Details = customer.Customer_Details,
                Address = customer.Customer_Address,
                Contact = customer.Customer_Contact,
                IsActive = customer.Is_Active,
                CreatedBy = customer.Created_By,
                CreatedOn = customer.Created_On,
                ModifiedBy = customer.Modified_By,
                ModifiedOn = customer.Modified_On
            };

            var customerNumberDetails = new List<string>();
            var customerNumberValues = new List<string>();


            foreach (var CustomerNumber in customer.CustomerNumbers)
            {
                customerNumberDetails.Add(CustomerNumber.Customer_Number_Details);
                customerNumberValues.Add(CustomerNumber.Customer_Number_Value);
            }

            customerViewModel.NumberDetails = customerNumberDetails;
            customerViewModel.NumberValues = customerNumberValues;

            return View(customerViewModel);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);

            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
