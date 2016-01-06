using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class ResponseContentMd5Handler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                byte[] content = await response.Content.ReadAsByteArrayAsync();
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(content);
                    response.Content.Headers.ContentMD5 = hash;
                }
                var utcNow = DateTime.UtcNow;
                response.Headers.Add(Configuration.XDateHeader, utcNow.ToString(Configuration.XDateFormat));
                //MD5 md5 = MD5.Create();
                //byte[] hash = md5.ComputeHash(content);
                //response.Content.Headers.ContentMD5 = hash;
            }
            return response;
        }
    }
}