using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal Money { get; set; }

        public string Detail { get; set; }

        public bool IsPaied { get; set; }
    }
}