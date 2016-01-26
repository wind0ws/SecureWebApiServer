using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Test
{
   public static class RFC1123
    {
        public static void Run()
        {
            var timeStr= TestToRFC1123String();
            var dateTime= TestRFC123ToDateTime(timeStr);
            Console.WriteLine(dateTime.ToString("yyyyMMdd HH:mm:ss"));
            

        }

        private static string TestToRFC1123String()
        {
            var timeStr= DateTime.UtcNow.ToString("r");
            Console.WriteLine(timeStr);
            return timeStr;
        }

        private static DateTime TestRFC123ToDateTime(string timeStr)
        {
            return Convert.ToDateTime(timeStr);
        }
        
    }
}
