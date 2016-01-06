using System;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class MessageRepresentation
    {
        public string Representation { get; set; }
        public string AppKey { get; set; }
        public DateTime Date { get; set; }
    }
}