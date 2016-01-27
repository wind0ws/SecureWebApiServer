using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApi.Controllers
{
    public class UsersController : ApiController
    {
        public IHttpActionResult Login(int id,string password)
        {
            if (id == 1 && password == "password")
            {
                return Ok();
            }
            return Unauthorized();
        }



    }
}
