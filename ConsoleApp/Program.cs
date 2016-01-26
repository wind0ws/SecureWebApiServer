using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApp.WebApi;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Utils.RandomUtil.Run();
            //AppKeySecret.Run();

            Client.Run();

           // Test.RFC1123.Run();

            Console.WriteLine("\r\n Console App运行结束，按回车键退出程序。");
            Console.ReadLine();
        }

    }
}
