using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreadTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<Int32> t = new Task<Int32>(n => Sum((Int32)n), 1000);
            t.Start();
            t.Wait();
            Task cwt = t.ContinueWith(task => Console.WriteLine("The result is {0}", t.Result));
            //Console.WriteLine(t.Result);
            Console.ReadKey();
        }

        private static Int32 Sum(Int32 n)
        {
            Int32 sum = 0;
            for (; n > 0; --n)
            {
                checked { sum += n; } //结果太大，抛出异常
            }
            return sum;
        }
        private static string GetString(Int32 n)
        {
            return "success";
        } 
    }
}
