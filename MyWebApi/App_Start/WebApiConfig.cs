using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Threshold.WebApiHmacAuth.Web.Infrastructure;

namespace MyWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 constraints: null,
                 handler: new HmacAuthenticationHandler(new DummySecretRepository(),
                     new CanonicalRepresentationBuilder(), new HmacSignatureCalculator())
                 {
                     InnerHandler = new ResponseContentMd5Handler()
                     {
                         InnerHandler = new HttpControllerDispatcher(config)
                     }
                 },
                 defaults: new { id = RouteParameter.Optional }
             );
        }
    }
}
