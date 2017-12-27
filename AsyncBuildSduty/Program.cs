using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBuildSduty
{
    class Program
    {
        static void Main(string[] args)
        {
            //NormalToDo();
            AsyncToDo();
            Console.ReadLine();
        }
        
        /// <summary>
        /// 顺序操作
        /// </summary>
        static void NormalToDo()
        {

            Console.WriteLine("Start");

            using (var fs = new FileStream("Data.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096))
            {
                fs.Write(new byte[100], 0, 100);
            }

            DoSomething();

            Console.WriteLine("END");
        }

        /// <summary>
        /// 异步操作
        /// </summary>
        static void AsyncToDo()
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
        /// 跨方法访问临时变量，形成闭包
        /// </summary>
        static void AsyncToDoBuffer()
        {
            string result = null;

            Thread writeThread = new Thread(new ThreadStart(WriteWapper));
            Thread doSomethingThread = new Thread(new ThreadStart(() =>
            {
                result = DoSomething();//跨方法访问临时变量，形成闭包
            }));

            writeThread.Start();
            doSomethingThread.Start();

            writeThread.Join();
            doSomethingThread.Join();
        }

        /// <summary>
        /// 使用线程池
        /// </summary>
        static void AsyncToDoWithPool()
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

        static void AsyncToDoWithInvoke()
        {
            string result = null;

            var doSomgthingDelegate = new Func<string>(DoSomething);
            var asyncResult = doSomgthingDelegate.BeginInvoke(new AsyncCallback(aresult =>
            {
                result = doSomgthingDelegate.EndInvoke(aresult);
            }), null);

            asyncResult.AsyncWaitHandle.WaitOne();

            Console.WriteLine(result);
        }


        //将方法包装成适于线程调用的签名
        private static void WriteWapper()
        {
            using (var fs = new FileStream("Data.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096))
            {
                fs.Write(new byte[100], 0, 100);
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
