using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    /// <summary>
    /// 规范化的待签名字符串
    /// </summary>
    public class CanonicalRepresentationBuilder : IBuildMessageRepresentation
    {
        private static DateTimeFormatInfo xDateFormat;
        static CanonicalRepresentationBuilder()
        {
            xDateFormat = new DateTimeFormatInfo();
            xDateFormat.ShortDatePattern = Configuration.XDateFormat;
        }

        /// <summary>
        /// Builds message representation as follows:
        /// HTTP METHOD\n +
        /// Content-MD5\n +  
        /// Content-Type\n +
        /// Timestamp\n +
        /// AppKey\n +
        /// Request URI
        /// </summary>
        /// <returns></returns>
        public string BuildRequestRepresentation(HttpRequestMessage requestMessage)
        {
            bool valid = IsRequestValid(requestMessage);
            if (!valid)
            {
                return null;
            }
            //if (!requestMessage.Headers.Date.HasValue)
            //{
            //    return null;
            //}
            DateTime date;
            if (requestMessage.Headers.Date.HasValue)
            {
                date = requestMessage.Headers.Date.Value.UtcDateTime;
            }
            else  {
                //DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                //dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                date = Convert.ToDateTime(requestMessage.Headers.GetValues(Configuration.XDateHeader).First(), xDateFormat);
            }
            
            string md5 = requestMessage.Content == null ||
                requestMessage.Content.Headers.ContentMD5 == null ?  "" 
                : Convert.ToBase64String(requestMessage.Content.Headers.ContentMD5);

            string httpMethod = requestMessage.Method.Method;
            string contentType = string.Empty;
            if (requestMessage.Content != null&&requestMessage.Content.Headers.ContentLength.GetValueOrDefault()>0)
            {
                contentType = requestMessage.Content.Headers.ContentType.ToString()/*.Replace(" ","")*/;
                //Content-Type:"application/json; utf-8"  注意分号后面有个空格。
                //提示Java、Android编写Http请求的Header时要格外注意。
            }
            //string contentType = requestMessage.Content.Headers.ContentType.MediaType;
            //if (!requestMessage.Headers.Contains(Configuration.UsernameHeader))
            //{
            //    return null;
            //}
            string username = requestMessage.Headers
                .GetValues(Configuration.AppKey).First();
            string uri = requestMessage.RequestUri.AbsolutePath.ToLower();
            // you may need to add more headers if thats required for security reasons
            string representation = string.Join("\n", httpMethod,
                md5,contentType, date.ToString(Configuration.XDateFormat),
                username, uri);
            
            return representation;
        }

        private bool IsRequestValid(HttpRequestMessage requestMessage)
        {
            //for simplicity I am omitting headers check (all required headers should be present)
            var headers=requestMessage.Headers;
            return headers.Contains(Configuration.AppKey) && (headers.Date.HasValue||headers.Contains(Configuration.XDateHeader));
            //return true;
        }
    }
}