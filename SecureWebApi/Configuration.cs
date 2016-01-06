namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class Configuration
    {
        /// <summary>
        /// X-ApiAuth的AppKey Header名称
        /// </summary>
        public const string AppKey = "X-ApiAuth-AppKey";
        /// <summary>
        /// XDate Header
        /// </summary>
        public const string XDateHeader = "X-Date";
        /// <summary>
        /// XDate 时间格式
        /// .Net平台直接将Date放入Header中即可
        /// </summary>
        public const string XDateFormat = "yyyy/MM/dd HH:mm:ss";
        /// <summary>
        /// 认证Scheme名称
        /// </summary>
        public const string AuthenticationScheme = "ApiAuth";
        /// <summary>
        /// 允许服务器与客户端的时间差
        /// </summary>
        public const int ValidityPeriodInMinutes = 5;
    }
}