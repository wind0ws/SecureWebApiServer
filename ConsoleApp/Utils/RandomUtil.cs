using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp.Utils
{
   public class RandomUtil
    {
        public static Random mRandom = new Random();

        public static void Run()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(IsUppercase());
            }
        }

        public static int Get(int min ,int max)
        {
            if (max < min)
            {
                throw new Exception("Max Must Bigger than Min");
            }
           return mRandom.Next(min, max);
        }

        public static bool IsUppercase()
        {
            return Get(0, 2) == 0;
        }
    }
}
