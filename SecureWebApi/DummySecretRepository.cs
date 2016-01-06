using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure
{
    public class DummySecretRepository : ISecretRepository
    {
        private readonly IDictionary<string, string> _appKeySecret
            = new Dictionary<string, string>()
                  {
                      {"BD1DDf6db09C43Ff","68fE733070Ca4DBe9ec5A17e112e1c85"}
                  };

      //  private HashAlgorithm hashAlgorithm;

        public DummySecretRepository()
        {
            //hashAlgorithm = new SHA1CryptoServiceProvider();
        }

        /// <summary>
        /// 用户的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>返回密码的hash</returns>
        public  string GetSecretForAppKey(string username)
        {
            if (!_appKeySecret.ContainsKey(username))
            {
                return null;
            }
            var secret = _appKeySecret[username];//实际生产当中密码应该从数据库中取
                                                 // var hashed = ComputeHash(userPassword, hashAlgorithm);
                                                 // return hashed;
            return secret;
        }

        private string ComputeHash(string inputData, HashAlgorithm algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            byte[] hashed = algorithm.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashed);
        }
    }
}