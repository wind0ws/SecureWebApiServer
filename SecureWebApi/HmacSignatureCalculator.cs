using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class HmacSignatureCalculator : ICalculteSignature
    {
        public string Signature(string secret, string value)
        {
            if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var valueBytes = Encoding.UTF8.GetBytes(value);
            string signature;

            using (var hmac = new HMACSHA256(secretBytes))
            {
                var hash = hmac.ComputeHash(valueBytes);
                signature = Convert.ToBase64String(hash);
            }

            return signature;
        }
    }
}