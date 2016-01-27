using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MyWebApi.Infrastructure.Attribute
{
    public class PrivilegeActionFilterAttribute : ActionFilterAttribute
    {
        private const string NoPrivilege = "You don't have privilege of this action!Check your permissons.";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (VerifyPrivilege(actionContext))
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
               var response= actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Forbidden, NoPrivilege);
                actionContext.Response = response;
            }
        }

        private bool VerifyPrivilege(HttpActionContext actionContext)
        {
            var anoymous = actionContext.ActionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>();
            if (anoymous != null && anoymous.Count >= 1)
            {
                return true;
            }
            else
            { 
                var controllerName= actionContext.ControllerContext.ControllerDescriptor.ControllerName; //Controller Name eg："Products"
                var actionName= actionContext.ActionDescriptor.ActionName;//Http Method eg:"Get"
                 
            }
            return false;
        }
    }
}