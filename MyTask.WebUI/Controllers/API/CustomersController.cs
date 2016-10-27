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

            var customerNumberDetails = new List<string>();
            var customerNumberValues = new List<string>();

            foreach (var CustomerNumber in customer.CustomerNumbers)
            {
                customerNumberDetails.Add(CustomerNumber.Customer_Number_Details);
                customerNumberValues.Add(CustomerNumber.Customer_Number_Value);
            }

            customerViewModel.NumberDetails = customerNumberDetails;
            customerViewModel.NumberValues = customerNumberValues;

            return Ok(customerViewModel);
        }

        // PUT: api/Customers/5
        [HttpPut]
        [ResponseType(typeof(void))]
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
            if (!ModelState.IsValid)
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
                Created_By = User.Identity.GetUserName(),
                Created_On = DateTime.Now
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            //adding customer numbers here
            for (int i = 0; i < customerViewModel.NumberValues.Count; i++)
            {
                var customerNumber = new CustomerNumber()
                {
                    Customer_Id_Fk = customer.Customer_Id_Pk,
                    Customer_Number_Details = customerViewModel.NumberDetails[i],
                    Customer_Number_Value = customerViewModel.NumberValues[i],
                    Created_By = User.Identity.GetUserName(),
                    Created_On = DateTime.Now
                };

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