using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    public class ProductsController : ApiController
    {
        private static List<Product> products = new List<Product>()
        {
            new Product { Id=1,Name="Apple",Category="Fruit" ,Price=11.51m },
            new Product { Id=2,Name="Computer",Category="Electronic",Price=15.32m },
            new Product { Id=3,Name="Orange",Category="Fruit",Price=60.54m }
        };

        // GET: api/Products
        public IEnumerable<Product> Get()
        {
            return products;
        }

        // GET: api/Products/5
        public IHttpActionResult Get(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Products
        public IHttpActionResult Post([FromBody]Product product)
        {
            products.Add(product);
            return Created<Product>("/api/products/" + product.Id, product);
        }

        // PUT: api/Products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {

        }
    }
}
