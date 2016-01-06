using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Threshold.LogHelper;
using Threshold.WebApiHmacAuth.Web.Infrastructure.Utils;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class HmacAuthenticationHandler : DelegatingHandler
    {

        private static DateTimeFormatInfo xDateFormat;

        static HmacAuthenticationHandler()
        {
            xDateFormat = new DateTimeFormatInfo();
            xDateFormat.ShortDatePattern = Configuration.XDateFormat;
        }

        //private const string UnauthorizedMessage = "Unauthorized request";

        private readonly ISecretRepository _secretRepository;
        private readonly IBuildMessageRepresentation _representationBuilder;
        private readonly ICalculteSignature _signatureCalculator;


        public HmacAuthenticationHandler(ISecretRepository secretRepository,
            IBuildMessageRepresentation representationBuilder,
            ICalculteSignature signatureCalculator)
        {
            _secretRepository = secretRepository;
            _representationBuilder = representationBuilder;
            _signatureCalculator = signatureCalculator;
        }

      

        protected async Task<Tuple<bool,string>> IsAuthenticated(HttpRequestMessage requestMessage)
        {
            if (!requestMessage.Headers.Contains(Configuration.AppKey))
            {
                return Tuple.Create(false,UnauthorizedReasons.NoAppKeyHeader);
            }

            var isDateValid = IsDateValid(requestMessage);
            if (!isDateValid.Item1)
            {
                return isDateValid;
            }

            if (requestMessage.Headers.Authorization == null 
                || requestMessage.Headers.Authorization.Scheme != Configuration.AuthenticationScheme)
            {
                return Tuple.Create(false,UnauthorizedReasons.NoAuthorizationHeaderOrScheme);
            }
            string appKey = requestMessage.Headers.GetValues(Configuration.AppKey).First();
            var secret = _secretRepository.GetSecretForAppKey(appKey);
            if (secret == null)
            {
                return Tuple.Create(false, UnauthorizedReasons.AppKeyIsNotExists);
            }

            var representation = _representationBuilder.BuildRequestRepresentation(requestMessage);
            if (string.IsNullOrWhiteSpace(representation))
            {
                return Tuple.Create(false,UnauthorizedReasons.CantBuildRepresentation);
            }
            //mLogger.Debug(representation);
            // FileUtil.WriteStringToFile(@"D:\Log.txt", representation + "\r\n", false, true);
             Log.D(representation + "\r\n");
            if (requestMessage.Content.Headers.ContentMD5 != null 
                && !await IsMd5Valid(requestMessage))
            {
                return Tuple.Create(false,UnauthorizedReasons.MD5NotMatch);
            }

            var signature = _signatureCalculator.Signature(secret, representation);

            // mLogger.Debug("Signature:"+signature);
            // FileUtil.WriteStringToFile(@"D:\Log.txt", "Signature:" + signature, false, true);
            Log.D("Signature:" + signature);

            if (MemoryCache.Default.Contains(signature))
            {
                return Tuple.Create(false,UnauthorizedReasons.ReplayAttack);
            }

            var result = requestMessage.Headers.Authorization.Parameter == signature;
            if (result)
            {
                MemoryCache.Default.Add(signature, appKey,
                                        DateTimeOffset.UtcNow.AddMinutes(Configuration.ValidityPeriodInMinutes));
                return Tuple.Create(true,string.Empty);
            }
          

            return Tuple.Create(false, UnauthorizedReasons.WrongSignature);
        }

        private async Task<bool> IsMd5Valid(HttpRequestMessage requestMessage)
        {
            var hashHeader = requestMessage.Content.Headers.ContentMD5;
            //Console.WriteLine("Content-MD5:"+Convert.ToBase64String(hashHeader));
            if (requestMessage.Content == null)
            {
                return hashHeader == null || hashHeader.Length == 0;
            }
            var headerMd5Bytes = requestMessage.Content.Headers.ContentMD5;
            var bytesContent = await requestMessage.Content.ReadAsByteArrayAsync();
            var contentMd5Bytes= MD5Helper.ComputeHash(bytesContent);
            if (!headerMd5Bytes.SequenceEqual(contentMd5Bytes)) 
            {
                //md5二次判断是为了兼容Java语言生成的MD5值，下面的代码用于比较java生成的Md5与C#进行比对。
                //对于C# 直接比较MD5的md5 byte[]即可，无需比较C# md5的Bit形式
                var headerMd5 = Convert.ToBase64String(headerMd5Bytes);//这个用于还原在java中放入Content-MD5中的String
                var md5 = BitConverter.ToString(contentMd5Bytes).Replace("-", "");
                // mLogger.Debug(string.Format("Md5 Compare(header and server calculate):\r\n{0}\r\n{1}",headerMd5,md5));
              //  Log.D(string.Format("Md5 Compare(header and server calculate):\r\n{0}\r\n{1}", headerMd5, md5));    
                return string.Compare(md5, headerMd5, true) == 0;//不区分大小写进行比较
            }
            return true;

            //var hash = await MD5Helper.ComputeHash(requestMessage.Content); 
            //return hash.SequenceEqual(hashHeader);
        }

        private Tuple<bool,string> IsDateValid(HttpRequestMessage requestMessage)
        {
            DateTime date;
            var utcNow = DateTime.UtcNow;
            var headerDate= requestMessage.Headers.Date;

            if (headerDate.HasValue)
            {
                date = requestMessage.Headers.Date.Value.UtcDateTime;
            }
            else if (requestMessage.Headers.Contains(Configuration.XDateHeader))
            {
                //DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                //dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                date = Convert.ToDateTime(requestMessage.Headers.GetValues(Configuration.XDateHeader).First(),xDateFormat);
            } else
            {
                return Tuple.Create(false,UnauthorizedReasons.NoDateHeaderValue);
            }

            if (date >= utcNow.AddMinutes(Configuration.ValidityPeriodInMinutes)
                || date <= utcNow.AddMinutes(-Configuration.ValidityPeriodInMinutes))
            {
                return Tuple.Create(false,UnauthorizedReasons.RequestIsExpire);
            }
            return Tuple.Create(true,string.Empty);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var isAuthenticated = await IsAuthenticated(request);
            
            if (!isAuthenticated.Item1)
            {
                var response = request
                    .CreateErrorResponse(HttpStatusCode.Unauthorized, isAuthenticated.Item2);
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(
                    Configuration.AuthenticationScheme));
                return response;
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}