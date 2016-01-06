using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class ClientSecretRepository : ISecretRepository
    {
        private IDictionary<string, string> _appKeySecretDict;
        // private HashAlgorithm _hashAlgorithm;

        public ClientSecretRepository(string appkey, string appSecret)
        {
            var dict = new Dictionary<string, string>();
            dict.Add(appkey, appSecret);
            this._appKeySecretDict = dict;
            this.AppKey = appkey;
        }

        private ClientSecretRepository(IDictionary<string, string> appKeySecret)
        {
            this._appKeySecretDict = appKeySecret;
            //this._hashAlgorithm = new SHA1CryptoServiceProvider();
        }

        public string AppKey { get; private set; }

        public string GetSecretForAppKey(string appKey)
        {
            KeyValuePair<string,string> result = _appKeySecretDict.FirstOrDefault((dict) => dict.Key == appKey);
            if (!string.IsNullOrWhiteSpace(result.Value))
            {
                return result.Value;
                //var hash= _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(result.Value));
                //return Convert.ToBase64String(hash);
            }
            return string.Empty;
        }
    }
}
