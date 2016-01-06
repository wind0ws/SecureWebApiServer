using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Threshold.WebApiHmacAuth.Web.Infrastructure.Utils
{
    //提示：Java计算出的MD5值(byte[])的Base64形式 = C#计算出的MD5值(byte[])的Bit形式
    public class MD5Helper
    {
        public static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            var content = await httpContent.ReadAsByteArrayAsync();
            return ComputeHash(content);
            //using (MD5 md5 = MD5.Create())
            //{
            //    var content = await httpContent.ReadAsByteArrayAsync();
            //    byte[] hash = md5.ComputeHash(content);
            //    return hash;
            //}
        }


        public static async Task<string> ComputeHashToBit(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            //var bytes=Encoding.GetEncoding("ISO-8859-1").GetBytes(str);
            //var content = Encoding.UTF8.GetString(bytes);
            //var encode= System.Web.HttpUtility.UrlEncode(content,Encoding.UTF8);
            return ComputeHashToBit(str);
            //using (MD5 md5 = MD5.Create())
            //{
            //    var content = await httpContent.ReadAsByteArrayAsync();
            //    byte[] hash = md5.ComputeHash(content);
            //    return hash;
            //}
        }

        public static async Task<string> ComputeHashToBase64(HttpContent httpContent)
        {
            var str = await httpContent.ReadAsStringAsync();
            return ComputeHashToBase64(str);
        }

        /// <summary>
        /// 根据byte[] Content计算出MD5 hash (byte[]形式）
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static  byte[] ComputeHash(byte[] content)
        {
            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(content); 
            }
        }

        /// <summary>
        /// 计算出string形式的content(默认编码UTF-8)的Base64形式的MD5值
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ComputeHashToBase64(string content)
        {
            return ComputeHashToBase64(content, Encoding.UTF8);
        }

        /// <summary>
        /// 计算出string形式的content(可设置string编码)的Base64形式的MD5值
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ComputeHashToBase64(string content,Encoding encoding)
        {
            var bytesContent = encoding.GetBytes(content);
            return ComputeHashToBase64(bytesContent);
        }

 
        public static string ComputeHashToBase64(byte[] content)
        {
            byte[] bytesHash = ComputeHash(content);
            return Convert.ToBase64String(bytesHash);
        }

        public static string ComputeHashToBit(string content)
        {
            return ComputeHashToBit(content, Encoding.UTF8);
        }

        public static string ComputeHashToBit(string content, Encoding encoding)
        {
            var contentBytes = encoding.GetBytes(content);
            return ComputeHashToBit(contentBytes);
        }

        public static string ComputeHashToBit(byte[] content)
        {
            var hash = ComputeHash(content);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}