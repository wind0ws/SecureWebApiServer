using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
   public class ClientCanonicalRepresentationBuilder : CanonicalRepresentationBuilder
    {
        public string AppKey{ get; set; }

        public override string GetAppKey(HttpRequestMessage requestMessage)
        {
            return AppKey;
        }

    }
}
