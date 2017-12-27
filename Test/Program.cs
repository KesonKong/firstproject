using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Publishser pub = new Publishser();
            Subscriber1 sub1 = new Subscriber1();
            Subscriber2 sub2 = new Subscriber2();
            Subscriber3 sub3 = new Subscriber3();

            pub.NumberChanged += new GeneralEventHandler(sub1.OnNumberChanged);
            pub.NumberChanged += new GeneralEventHandler(sub2.OnNumberChanged);
            pub.NumberChanged += new GeneralEventHandler(sub3.OnNumberChanged);
            pub.NumberChanged += new GeneralEventHandler(GetStriing);
            pub.DoSomething();          // 触发事件

            GeneralEventHandler dd = new GeneralEventHandler(GetStriing2);
            string s = dd();
            Console.WriteLine(s);
            Console.ReadKey();
        }

        static string GetStriing()
        {
            return "ABC";
        }

        static string GetStriing2()
        {
            return "efg";
        }

    }

    
    // 定义委托
    public delegate string GeneralEventHandler();

    // 定义事件发布者
    public class Publishser
    {
        public event GeneralEventHandler NumberChanged; // 声明一个事件
        public void DoSomething()
        {
            if (NumberChanged != null)
            {    // 触发事件
                string rtn = NumberChanged();
                Console.WriteLine(rtn);     // 打印返回的字符串，输出为Subscriber3
            }
        }
    }


    // 定义事件订阅者
    public class Subscriber1
    {
        public string OnNumberChanged()
        {
            return "Subscriber1";
        }
    }
    public class Subscriber2
    {
        public string OnNumberChanged()
        {
            return "Subscriber2";
        }
    }
    public class Subscriber3
    {
        public string OnNumberChanged()
        {
            return "Subscriber3";
        }
    }
}
