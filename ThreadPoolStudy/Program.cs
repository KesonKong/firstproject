using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            //将工作项加入到线程池队列中，这里可以传递一个线程参数
            ThreadPool.QueueUserWorkItem(TestMethod, "Hello");
            ThreadPool.QueueUserWorkItem(TestMethodTwo, "World");
            Console.ReadKey();
        }

        public static void TestMethod(object data)
        {
            string strData = data as string;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{2}执行第{0}次，线程ID：{1}", i.ToString(), Thread.CurrentThread.ManagedThreadId, strData);
                Thread.Sleep(300);
            }
        }


        public static void TestMethodTwo(object data)
        {
            string strData = data as string;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{2}执行第{0}次，线程ID：{1}", i.ToString(), Thread.CurrentThread.ManagedThreadId, strData);
                Thread.Sleep(300);
            }
        }

    }
}
