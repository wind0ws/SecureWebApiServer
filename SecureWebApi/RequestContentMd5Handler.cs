using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class RequestContentMd5Handler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (request.Content == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            byte[] content = await request.Content.ReadAsByteArrayAsync();
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash=md5.ComputeHash(content);
                request.Content.Headers.ContentMD5 = hash;
            }
                //MD5 md5 = MD5.Create();
                //byte[] hash = md5.ComputeHash(content);
                //request.Content.Headers.ContentMD5 = hash;
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}