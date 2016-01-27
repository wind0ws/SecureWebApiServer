using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    [RoutePrefix("api/users/{userId}/orders")]
    public class OrdersController : ApiController
    {
        private static List<Order> sOrders=new List<Order>()
        {
            new Order() { Id=1,Detail="红酒",Money=12.22m,IsPaied=false },
            new Order() { Id=2,Detail="零食",Money=55.12m,IsPaied=true }
        };

        [Route("")]
        public IHttpActionResult GetOrders(int userId)
        {
            if (userId == 1)
            {
                return Ok<List<Order>>(sOrders);
            }
            return NotFound();
        }

        [Route("order/{orderId}")]
        public IHttpActionResult GetOrders(int userId,int orderId)
        {
            if (userId == 1)
            {
                var order = sOrders.Find((p) => { return p.Id == orderId; });
                if (order != null)
                {
                    return Ok(order);
                }
            }
            return NotFound();

        }
        [Route("~/api/orders/{orderId}")]
        public IHttpActionResult GetOrdersDirect(int orderId)
        {
            var order = sOrders.Find((p) => { return p.Id == orderId; });
            if (order != null)
            {
                return Ok(order);
            }
            return NotFound();
        }

    }
}
