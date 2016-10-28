using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyTask.Models;
using MyTask.WebUI.Models;
using MyTask.WebUI.ViewModels;
using Microsoft.AspNet.Identity;

namespace MyTask.WebUI.Controllers.API
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<CustomerViewModel> GetCustomers()
        {
            var customers = db.Customers.ToList();

            var customersViewModels = new List<CustomerViewModel>();

            foreach (var customer in customers)
            {
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

                var customerNumberViewModels = new List<CustomerNumberViewModel>();

                var CustomerNumbers = db.CustomerNumbers.Where(c => c.Customer_Id_Fk == customer.Customer_Id_Pk).ToList();

                foreach (var CustomerNumber in CustomerNumbers)
                {
                    var customerNumberViewModel = new CustomerNumberViewModel()
                    {
                        ID = CustomerNumber.Customer_Number_Id_Pk,
                        NumberValue = CustomerNumber.Customer_Number_Value,
                        NumberDetail = CustomerNumber.Customer_Number_Details
                    };

                    customerNumberViewModels.Add(customerNumberViewModel);
                }

                customerViewModel.CustomerNumbers = customerNumberViewModels;

                customersViewModels.Add(customerViewModel);
            }

            return customersViewModels;
        }

        // GET: api/Customers/5
        [HttpGet]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
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

            var customerNumberViewModels = new List<CustomerNumberViewModel>();

            var CustomerNumbers = db.CustomerNumbers.Where(c => c.Customer_Id_Fk == customer.Customer_Id_Pk).ToList();

            foreach (var CustomerNumber in CustomerNumbers)
            {
                var customerNumberViewModel = new CustomerNumberViewModel()
                {
                    ID = CustomerNumber.Customer_Number_Id_Pk,
                    NumberValue = CustomerNumber.Customer_Number_Value,
                    NumberDetail = CustomerNumber.Customer_Number_Details
                };

                customerNumberViewModels.Add(customerNumberViewModel);
            }

            customerViewModel.CustomerNumbers = customerNumberViewModels;

            return Ok(customerViewModel);
        }

        // PUT: api/Customers/5
        [HttpPut]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult PutCustomer(int id, CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerViewModel.ID)
            {
                return BadRequest();
            }

            var customer = db.Customers.Find(customerViewModel.ID);
            customer.Customer_Name = customerViewModel.Name;
            customer.Customer_Details = customerViewModel.Details;
            customer.Customer_Address = customerViewModel.Address;
            customer.Customer_Contact = customerViewModel.Contact;
            customer.Is_Active = customerViewModel.IsActive;
            customer.Modified_By = User.Identity.GetUserName();
            customer.Modified_On = DateTime.Now;

            db.Entry(customer).State = EntityState.Modified;

            foreach (var customerNumberViewModel in customerViewModel.CustomerNumbers)
            {
                var customerNumber = db.CustomerNumbers.Find(customerNumberViewModel.ID);

                customerNumber.Customer_Number_Details = customerNumberViewModel.NumberDetail;
                customerNumber.Customer_Number_Value = customerNumberViewModel.NumberValue;
                customerNumber.Modified_By = User.Identity.GetUserName();
                customerNumber.Modified_On = DateTime.Now;

                db.Entry(customerNumber).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [HttpPost]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult PostCustomer(CustomerViewModel customerViewModel)
        {
            customerViewModel.CreatedOn = DateTime.Now;
            customerViewModel.CreatedBy = User.Identity.GetUserName();

            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            //can not apply modelstate.isvalid because it is looking for ID in model & giving error.

            if (string.IsNullOrEmpty(customerViewModel.Name))
            {
                return BadRequest(ModelState);
            }

            //i should do transactions here to make sure all recrrds insserts correctly
            var customer = new Customer()
            {
                Customer_Name = customerViewModel.Name,
                Customer_Details = customerViewModel.Details,
                Customer_Address = customerViewModel.Address,
                Customer_Contact = customerViewModel.Contact,
                Is_Active = customerViewModel.IsActive,
                Created_By = customerViewModel.CreatedBy,
                Created_On = customerViewModel.CreatedOn
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            //adding customer numbers here
            foreach (var customerNumberViewModel in customerViewModel.CustomerNumbers)
            {
                var customerNumber = new CustomerNumber();

                customerNumber.Customer_Number_Details = customerNumberViewModel.NumberDetail;
                customerNumber.Customer_Number_Value = customerNumberViewModel.NumberValue;
                customerNumber.Created_By = User.Identity.GetUserName();
                customerNumber.Created_On = DateTime.Now;

                db.CustomerNumbers.Add(customerNumber);
                db.SaveChanges();
            }

            return Created(new Uri(Request.RequestUri + "/" + customer.Customer_Id_Pk), customerViewModel);
        }

        // DELETE: api/Customers/5
        [HttpDelete]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Customer_Id_Pk == id) > 0;
        }
    }
}