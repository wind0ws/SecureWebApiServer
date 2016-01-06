using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class AppKeySecret 
    {
        public  static void Run()
        {
            Console.WriteLine("生成APPKey与APPSecret：\r\n");

            var appKey = GenerateAppKey();
            var appSecret = GenerateAppSecret();

            ToFile("\r\n---------------------------------------------------");
            ToFile(appKey);
            ToFile(appSecret);
            ToFile("---------------------" + DateTime.Now.ToString() + "---------------------\r\n");

            Console.WriteLine("生成结束，您也可以查看软件目录下生成的文件\r\n");
        }

        public static string GenerateAppKey()
        {
           var originKey= NewGuidWithoutSeparator().Substring(0,16);
            var key = GetRandomUpperLowerString(originKey);
            DebugWriteLine("AppKey:    "+key);
            return key;
        }

        public static string GenerateAppSecret()
        {
            var originSecret= NewGuidWithoutSeparator();
            var secret = GetRandomUpperLowerString(originSecret);
            DebugWriteLine("AppSecret: " + secret);
            return secret;
        }

        private static string NewGuidWithoutSeparator()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        private static void DebugWriteLine(string content)
        {
            Console.WriteLine(content);
            System.Diagnostics.Debug.WriteLine(content);
        }

        private static void ToFile(string content)
        {
            Utils.FileUtil.WriteStringToFile(AppDomain.CurrentDomain.BaseDirectory+@"\AppKeySecret.txt",content);
        }

        private static string GetRandomUpperLowerString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                var s = str[i];
                sb.Append(Utils.RandomUtil.IsUppercase() ? s.ToString().ToUpperInvariant() : s.ToString().ToLowerInvariant());
            }
            return sb.ToString();
        }
        

    }
}
