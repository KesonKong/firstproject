using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TreadStudy
{

    //声明指向含两个int型参数、返回值为int型的函数的委托
    public delegate int AddOp(int x, int y);
    public delegate string MyDelegate(object data);

    class Program
    {
        static void Main(string[] args)
        {
            //Demo01();
            //Demo02();
            //Demo03();
            //Demo04();
            Demo05();
            Console.ReadKey();
        }

        /// <summary>
        /// 多线程演示
        /// </summary>
        static void Demo01()
        {
            Console.WriteLine("************** 显示当前线程的相关信息 *************");

            //声明线程变量并赋值为当前线程
            Thread primaryThread = Thread.CurrentThread;
            //赋值线程的名称
            primaryThread.Name = "主线程";

            //显示线程的相关信息
            Console.WriteLine("线程的名字：{0}", primaryThread.Name);
            Console.WriteLine("线程是否启动？ {0}", primaryThread.IsAlive);
            Console.WriteLine("线程的优先级： {0}", primaryThread.Priority);
            Console.WriteLine("线程的状态： {0}", primaryThread.ThreadState);

            User u = new User()
            {
                UserName = "张三",
                Sex = "男"
            };
            Thread SecondThread = new Thread(new ParameterizedThreadStart(PrintNumbers));
            SecondThread.Name = "次线程";
            //次线程开始执行指向的方法
            SecondThread.Start(u);


            for (int i = 99; i > 0; i--)
            {
                Console.WriteLine("{0}{1}, ", i, Thread.CurrentThread.Name);
                //Sleep()方法使当前线程挂等待指定的时长在执行，这里主要是模仿打印任务
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 多线程并发问题,需要用锁来解决 lock 或者Monitor
        /// </summary>
        static void Demo02()
        {
            Console.WriteLine("********* 并发问题演示 ***************");
            //创建一个打印对象实例
            Printer printer = new Printer();
            //声明一含5个线程对象的数组
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++)
            {
                //将每一个线程都指向printer的PrintNumbers()方法
                threads[i] = new Thread(new ThreadStart(printer.MonitorPrintNumbers));
                //给每一个线程编号
                threads[i].Name = i.ToString() + "号线程";

            }

            //开始执行所有线程

            foreach (Thread t in threads)
            {
                t.Start();
            }

            Console.ReadLine();
        }



        /// <summary>
        /// 线程异步 委托异步线程 两个线程“同时”工作
        /// </summary>
        static void Demo03()
        {
            Console.WriteLine("******* 委托异步线程 两个线程“同时”工作 *********");
            //显示主线程的唯一标示
            Console.WriteLine("调用Main()的主线程的线程ID是：{0}.", Thread.CurrentThread.ManagedThreadId);
            //将委托实例指向Add()方法
            AddOp pAddOp = new AddOp(Add);

            //开始委托次线程调用。委托BeginInvoke()方法返回的类型是IAsyncResult，
            //包含这委托指向方法结束返回的值，同时也是EndInvoke()方法参数
            IAsyncResult iftAR = pAddOp.BeginInvoke(10, 10, null, null);
            //int i = 0;
            //while (!iftAR.IsCompleted)
            //{
            //    Console.WriteLine("Main()方法中执行其他任务........线程ID是：{0}，执行第{1}次", Thread.CurrentThread.ManagedThreadId, i.ToString());
            //    i++;
            //}
            
            int sum = pAddOp.EndInvoke(iftAR);
            Console.WriteLine("10 + 10 = {0}.", sum);
            Console.ReadLine();
        }

        /// <summary>
        /// 线程同步，“阻塞”调用，两个线程工作
        /// </summary>
        static void Demo04()
        {
            Console.WriteLine("******* 线程同步，“阻塞”调用，两个线程工作 *********");
            Console.WriteLine("Main() invokee on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            //将委托实例指向Add()方法
            AddOp pAddOp = new AddOp(Add);
            IAsyncResult iftAR = pAddOp.BeginInvoke(10, 10, null, null);
            //判断委托线程是否执行完任务，
            //没有完成的话，主线程就做其他的事
            while (!iftAR.IsCompleted)
            {
                Console.WriteLine("Main()方法工作中.......线程ID是：{0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
            }
            //获得返回值
            int answer = pAddOp.EndInvoke(iftAR);
            Console.WriteLine("10 + 10 = {0}.", answer);
            Console.ReadLine();
        }

        static void Demo05()
        {
            User u = new User()
            {
                UserName = "李四",
                Sex = "男"
            };
            MyDelegate mydelegate = new MyDelegate(TestMethod);
            IAsyncResult result = mydelegate.BeginInvoke("Thread Param", TestCallback, u);

            //异步执行完成
            string resultstr = mydelegate.EndInvoke(result);
            Console.WriteLine(resultstr);
        }

        //线程函数
        public static string TestMethod(object data)
        {
            string datastr = data as string;

            return datastr;
        }

        //异步回调函数
        public static void TestCallback(IAsyncResult data)
        {
            User u = data.AsyncState as User;
            Console.WriteLine(u.UserName);
        }

        static void PrintNumbers(object obj)
        {
            User u = obj as User;
            Console.WriteLine("-> {0} 在执行打印数字函数 PrintNumber()，操作人：{1}，性别{2}", Thread.CurrentThread.Name, u.UserName, u.Sex);
            Console.WriteLine("打印数字： ");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0},{1} ", i, Thread.CurrentThread.Name);
                //Sleep()方法使当前线程挂等待指定的时长在执行，这里主要是模仿打印任务
                Thread.Sleep(2000);
            }
            Console.WriteLine();

        }

        public class Printer
        {
            //打印数字的方法
            public void PrintNumbers()
            {
                //使用lock关键字，锁定d的代码是线程安全的
                lock (this)
                {
                    Console.WriteLine("-> {0} 正在执行打印任务，开始打印数字：", Thread.CurrentThread.Name);
                    for (int i = 0; i < 10; i++)
                    {
                        Random r = new Random();
                        //为了增加冲突的几率及，使各线程各自等待随机的时长
                        Thread.Sleep(2000 * r.Next(5));
                        //打印数字
                        Console.Write("{0} ", i);
                    }
                    Console.WriteLine();

                }
            }

            public void MonitorPrintNumbers()
            {
                Monitor.Enter(this);
                try
                {
                    Console.WriteLine("-> {0} 正在执行打印任务，开始打印数字：", Thread.CurrentThread.Name);
                    for (int i = 0; i < 10; i++)
                    {
                        Random r = new Random();
                        //为了增加冲突的几率及，使各线程各自等待随机的时长
                        Thread.Sleep(2000 * r.Next(5));
                        //打印数字
                        Console.Write("{0} ", i);
                    }
                    Console.WriteLine();
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }

        }

        static int Add(int x, int y)
        {

            //指示调用该方法的线程ID，ManagedThreadId是线程的唯一标示

            Console.WriteLine("调用求和方法 Add()的线程ID是： {0}.", Thread.CurrentThread.ManagedThreadId);

            //模拟一个过程，停留5秒

            Thread.Sleep(5000);

            int sum = x + y;

            return sum;

        }
    }

    class User
    {
        public string UserName { get; set; }
        public string Sex { get; set; }
    }
}
