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

namespace MyTask.WebUI.Controllers.API
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Customers
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
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Customer_Id_Pk)
            {
                return BadRequest();
            }

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
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.Customer_Id_Pk }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
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