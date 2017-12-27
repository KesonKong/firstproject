using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateSduty
{
    //定义委托，它定义了可以代表的方法的类型
    public delegate void GreetingDelegate(string name);

    class Program
    {
        private static void EnglishGreeting(string name)
        {
            Console.WriteLine("Morning, " + name);
        }

        private static void ChineseGreeting(string name)
        {
            Console.WriteLine("早上好, " + name);
        }

        //注意此方法，它接受一个GreetingDelegate类型的方法作为参数
        private static void GreetPeople(string name, GreetingDelegate MakeGreeting)
        {
            MakeGreeting(name);
        }

        static void Main(string[] args)
        {
            //GreetPeople("Jimmy Zhang", EnglishGreeting);
            //GreetPeople("张子阳", ChineseGreeting);

            //GreetingDelegate delegate1, delegate2;
            //delegate1 = EnglishGreeting;
            //delegate2 = ChineseGreeting;
            //GreetPeople("Jimmy Zhang", delegate1);
            //GreetPeople("张子阳", delegate2);

            //GreetingDelegate delegate1 = new GreetingDelegate(EnglishGreeting);
            //delegate1 += ChineseGreeting;
            //// 将先后调用 EnglishGreeting 与 ChineseGreeting 方法
            //delegate1("Jimmy Zhang");   

            GreetingManager gm = new GreetingManager();
            gm.MakeGreet += EnglishGreeting;
            gm.MakeGreet += ChineseGreeting;
            gm.GreetPeople("Jimmy Zhang");

            Console.ReadKey();
        }
    }

    public class GreetingManager
    {
        //这一次我们在这里声明一个事件
        public event GreetingDelegate MakeGreet;
        public void GreetPeople(string name)
        {
            if (MakeGreet != null)
            {
                MakeGreet(name);
            }
        }
    }


    // 热水器
    public class Heater
    {
        private int temperature;
        public delegate void BoilHandler(int param);   //声明委托
        public event BoilHandler BoilEvent;        //声明事件

        // 烧水
        public void BoilWater()
        {
            for (int i = 0; i <= 100; i++)
            {
                temperature = i;

                if (temperature > 95)
                {
                    if (BoilEvent != null)
                    { //如果有对象注册
                        BoilEvent(temperature);  //调用所有注册对象的方法
                    }
                }
            }
        }
    }

    // 警报器
    public class Alarm
    {
        public void MakeAlert(int param)
        {
            Console.WriteLine("Alarm：嘀嘀嘀，水已经 {0} 度了：", param);
        }
    }

    // 显示器
    public class Display
    {
        public static void ShowMsg(int param)
        { //静态方法
            Console.WriteLine("Display：水快烧开了，当前温度：{0}度。", param);
        }
    }
}
