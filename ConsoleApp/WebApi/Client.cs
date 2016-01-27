using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.WebApi.Models;
using Threshold.WebApiHmacAuth.Web.Infrastructure;

namespace ConsoleApp.WebApi
{
    class Client
    {
        public static void Run()
        {
            Console.WriteLine("WebApi Client Test开始.....\r\n");
            var signingHandler = new HmacSigningHandler(new ClientSecretRepository(Constant.APP_KEY, Constant.APP_SECRET), new ClientCanonicalRepresentationBuilder(),
                                                   new HmacSignatureCalculator());
            //signingHandler.AppKey = Constant.APP_KEY;

            // GetProducts(signingHandler);
            //GetProduct(signingHandler);
            // AddProduct(signingHandler);

            //GetSelfDefineRouteResult(signingHandler);
            //GetOrder(signingHandler);
            GetOrderById(signingHandler);

            Console.WriteLine("\r\nWebApi Client Test结束.....\r\n");

        }


        public static void GetOrderById(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("\r\n\r\nGet http://localhost:4779/api/orders/2");
            var getClient = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });

            var getResponse = getClient.GetStringAsync("http://localhost:4779/api/orders/2");
            var getResult = getResponse.Result;
            Console.WriteLine("Server response: " + getResult);
        }

        public static void GetOrder(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("\r\n\r\nGet http://localhost:4779/api/users/1/orders/order/2");
            var getClient = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });

            var getResponse = getClient.GetStringAsync("http://localhost:4779/api/users/1/orders/order/2");
            var getResult = getResponse.Result;
            Console.WriteLine("Server response: " + getResult);
        }

        public static void GetSelfDefineRouteResult(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("\r\n\r\nGet http://localhost:4779/api/users/Admin/products");
            var getClient = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });

            var getResponse = getClient.GetStringAsync("http://localhost:4779/api/users/Admin/products");
            var getResult = getResponse.Result;
            Console.WriteLine("Server response: " + getResult);
        }

        public static void GetProducts(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("\r\n\r\nGet http://localhost:4779/api/products/");
            var getClient = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });

            var getResponse = getClient.GetStringAsync("http://localhost:4779/api/products");
            var getResult = getResponse.Result;
            Console.WriteLine("Server response: " + getResult);
        }

        private static void GetProduct(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("\r\n\r\nGet The Product Of Location 1 http://localhost:4779/api/products/1");
            var getClient = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });

            var getResponse = getClient.GetStringAsync("http://localhost:4779/api/products/1");
            var getResult = getResponse.Result;
            Console.WriteLine("Server response: " + getResult);
        }

        private static void AddProduct(HmacSigningHandler signingHandler)
        {
            Console.WriteLine("Post Add Product http://localhost:4779/api/products");

            var client = new HttpClient(new RequestContentMd5Handler()
            {
                InnerHandler = signingHandler
            });
            Product product = new Product() { Id = 5, Name = "Apple", Category = "Fruit" ,Price=12.11m};
            //var productJson = JsonConvert.SerializeObject(product);
            var response = client.PostAsJsonAsync("http://localhost:4779/api/products", product).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            // var result = response.Content.ReadAsAsync<string>().Result;

            Console.WriteLine("Server response: " + result);
        }

    }
}
