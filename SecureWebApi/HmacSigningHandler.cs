using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class HmacSigningHandler : HttpClientHandler
    {
        private readonly ISecretRepository _secretRepository;
        private readonly IBuildMessageRepresentation _representationBuilder;
        private readonly ICalculteSignature _signatureCalculator;


        public string AppKey { get; set; }  //  find the appsecret by appkey


        public HmacSigningHandler(ISecretRepository secretRepository,
                              IBuildMessageRepresentation representationBuilder,
                              ICalculteSignature signatureCalculator)
        {
            _secretRepository = secretRepository;
            _representationBuilder = representationBuilder;
            _signatureCalculator = signatureCalculator;
            TrySetAppKey();
        }

        private void TrySetAppKey()
        {
            var clientSecretRepo = _secretRepository as ClientSecretRepository;
            if (clientSecretRepo != null)
            {
                this.AppKey = clientSecretRepo.AppKey;
            }
        }

        protected  override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                System.Threading.CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Configuration.AppKey))
            {
                request.Headers.Add(Configuration.AppKey, AppKey);
            }
            request.Headers.Date = new DateTimeOffset(DateTime.Now,DateTime.Now-DateTime.UtcNow);
            var representation = _representationBuilder.BuildRequestRepresentation(request);
            var secret = _secretRepository.GetSecretForAppKey(AppKey);
            string signature = _signatureCalculator.Signature(secret,
                representation);
            var header = new AuthenticationHeaderValue(Configuration.AuthenticationScheme, signature);
            request.Headers.Authorization = header;
            return base.SendAsync(request, cancellationToken);
        }
    }
}