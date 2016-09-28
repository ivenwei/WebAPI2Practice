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
using WebAPI2WithFormAuth.ActionFilters;
using WebAPI2WithFormAuth.Models;

namespace WebAPI2WithFormAuth.Controllers
{

    //If you don't login first you will no access right of ClientsController
    //If want to test the webAPi please go to http://localhost:49169/Home/Login 
    //After Login you can go back to PostMan , and install interceptor of postman package
    //After install interceptor finish , the webapi can access by login person
    [Authorize]
    [ValidateModel]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Clients  orignal URL: http://localhost:49169/api/clients
        // Use route attribute to set the new URL of WebAPI
        // PostMan test new URL use get method : http://localhost:49169/clients
        [Route("clients")]
        public IQueryable<Client> GetClient()
        {
            return db.Client;
        }

        // GET: api/Clients/5  orignal URL: http://localhost:49169/api/clients/1
        // Use route attribute to set the new URL of WebAPI
        // PostMan test new URL use get method : http://localhost:49169/clients/1
        [ResponseType(typeof(Client))]
        [Route("clients/{id}")]
        public IHttpActionResult GetClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        //self define route attribute : Input client id to find order table data with this id
        //PostMan test URL use get method : http://localhost:49169/clients/1/orders
        [Route("clients/{id}/orders")]
        public IHttpActionResult GetClientOrders(int id)
        {
            var orders = db.Order.Where(d => d.ClientId == id);
            return Ok(orders);
        }

        //self define route attribute : Input client id to find all orders and selected by input order id
        //PostMan test URL use get method : http://localhost:49169/clients/1/orders/182
        [Route("clients/{ClientId}/orders/{OrderId}")]
        public IHttpActionResult GetClientOrders(int ClientId, int OrderId)
        {
            var order = db.Order.Where(d => d.ClientId == ClientId && d.OrderId == OrderId).FirstOrDefault();
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        //self define route attribute : Input client id to find all orders and selected where OrderStatus is pending
        //PostMan test URL use get method : http://localhost:49169/clients/1/orders/pending
        [Route("clients/{id}/orders/pending")]
        public IHttpActionResult GetClientOrdersPending(int id)
        {
            var orders = db.Order.Where(p => p.ClientId == id && p.OrderStatus == "P");
            return Ok(orders);
        }

        //self define route : Input client id to find all orders and selected by input date
        //PostMan test URL use get method : http://localhost:49169/clients/1/orders/2001/11/25
        [Route("clients/{id}/orders/{*date}")]
        public IHttpActionResult GetClientOrdersByDate(int id, DateTime date)
        {
            var orders = db.Order.Where(p => p.ClientId == id
                && p.OrderDate.Value.Year == date.Year
                && p.OrderDate.Value.Month == date.Month
                && p.OrderDate.Value.Day == date.Day);

            return Ok(orders);
        }


        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.ClientId }, client);
        }

        // DELETE: api/Clients/5
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}