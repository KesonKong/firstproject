using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {

        static void Main(string[] args)
        {
            //WriteWapper();
            //Console.WriteLine( DoSomething());

            //Demo01();
            //Demo02();
            Demo03();
            
            Console.ReadKey();
        }

        /// <summary>
        /// 异步
        /// </summary>
        static void Demo01()
        {
            Thread writeThread = new Thread(new ThreadStart(WriteWapper));
            Thread doSomethingThread = new Thread(new ParameterizedThreadStart(DoSomethingWapper));

            ClosureClass closure = new ClosureClass();
            writeThread.Start();
            doSomethingThread.Start(closure);//闭包对象，用于变量穿越

            writeThread.Join();
            doSomethingThread.Join();

            Console.WriteLine(closure.Result);
        }

        /// <summary>
        /// 通过闭包的方式访问临时变量，从而减少大量的代码，并提高程序可读性。
        /// </summary>
        static void Demo02()
        {
            string result = null;

            Thread writeThread = new Thread(new ThreadStart(WriteWapper));
            Thread doSomethingThread = new Thread(new ThreadStart(() =>
            {
                result = DoSomething(); ////跨方法访问临时变量，形成闭包
            }));

            writeThread.Start();
            doSomethingThread.Start();

            writeThread.Join();
            doSomethingThread.Join();

            Console.WriteLine(result);
        }

        /// <summary>
        /// 开启一个新线程将带来可观的开销，因此我们希望能够重用线程，在.NET中，可以采用线程池达到这一目的，同时简化线程的操作。
        /// </summary>
        static void Demo03()
        {
            string result = null;

            AutoResetEvent resetEvent = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                result = DoSomething();
                resetEvent.Set();
            }));

            resetEvent.WaitOne();
            Console.WriteLine(result);
            
        }

        //将方法包装成适于线程调用的签名
        private static void WriteWapper()
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(string.Format("{0}-Hello World!", i.ToString()));
            }
        }


        //将方法包装成适于线程调用的签名
        static void DoSomethingWapper(object state)
        {
            ClosureClass closure = state as ClosureClass;
            var result = DoSomething();
            if (closure != null)
            {
                closure.Result = result;
            }
        }


        static string DoSomething()
        {
            Thread.Sleep(2000);
            
            return "Finished";
        }
    }

    //闭包辅助类，用于存储在方法间传递内部变量与参数
    class ClosureClass
    {
        //存储方法返回值
        public string Result { get; set; }
    }



}
