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
using WebAPI2WithFormAuth.Models;
using WebAPI2WithFormAuth.Models.ViewModels;

namespace WebAPI2WithFormAuth.Controllers
{
    public class ProductsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ProductsController()
        {
            //avoid Reference Loop
            db.Configuration.LazyLoadingEnabled = false;
        }

        // GET: api/Products
        //PostMan test URL get : http://localhost:49169/api/Products
        public IQueryable<Product> GetProduct()
        {
            return db.Product;
        }

        // GET: api/Products/5
        //PostMan test URL get : http://localhost:49169/api/Products/1
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // First. use Postman Get : http://localhost:49169/api/Products/1 
        // Second. use Postman Put modify data : http://localhost:49169/api/Products/1 
        // Body : 
        // {
        //  "ProductId": 1,
        //  "ProductName": "block fleece fabric",
        //  "Price": 6,
        //  "Active": true,
        //  "Stock": 1500
        // }
        // Third. use use Postman Get again : http://localhost:49169/api/Products/1 
        // You will see the change
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // PostMan test URL Post : http://localhost:49169/api/Products
        // PostMan Body : 
        // {
        //  "OrderLine": [],
        //  "ProductName": "Test12345",
        //  "Price": 999,
        //  "Active": true,
        //  "Stock": 9999
        // }
        // After Post you can get product data by : http://localhost:49169/api/Products/1555
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        // PostMan test URL Delete : http://localhost:49169/api/Products/1555
        // After DELETE you can use get product data by : http://localhost:49169/api/Products/1555
        // But find nothing
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        // self defined Patch action and use viewmodel to modify the specific property you want
        // First. use Postman Get : http://localhost:49169/api/Products/1 
        // Second. use Postman Patch modify data : http://localhost:49169/api/Products/1 
        // Body : 
        // {
        //  "ProductId": 1,
        //  "ProductName": "fleece fabric",
        //  "Price": 55555,
        //  "Active": true,
        //  "Stock": 66666
        // }
        // Third. use use Postman Get again : http://localhost:49169/api/Products/1 
        // And you will see the change
        [ResponseType(typeof(Product))]
        public IHttpActionResult PatchProduct(int id, ProductsPatchViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ProductItem = db.Product.Where(d => d.ProductId == id).FirstOrDefault();
            if (ProductItem == null)
            {
                return NotFound();
            }

            ProductItem.Price = product.Price;
            ProductItem.Stock = product.Stock;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}