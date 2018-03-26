using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleAppTimer
{
    class TimerParam
    {
        public string name;
        public int period;
        public int index;
        public int firstCall;
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Timer timer = new Timer(MyTimer, (object)"Timer 1", 2000, 1000);

            Console.WriteLine("Press <Enter> key for continue...");
            Console.ReadLine();
            TimerParam tparam = new TimerParam();
            tparam.index = 1;
            tparam.name = "Timer 1";
            tparam.period = 1000;   //1 сек
            tparam.firstCall = 2000;    //due time
            Timer timer = new Timer(MyTimer, tparam, tparam.firstCall, tparam.period);
            Console.ReadLine();
        }

        private static void MyTimer(object param)
        {
            TimerParam tp = (TimerParam)param;
            Console.WriteLine("{0}) Name: {1}; ThreadId = {2}", tp.index, tp.name, Thread.CurrentThread.ManagedThreadId);
            tp.timer.Change(0, 0xffffffff);
            Thread.Sleep(5000);
            Console.WriteLine("Timer is done!", Thread.CurrentThread.ManagedThreadId);
            tp.timer.Change(tp.period, tp.period);
        }
    }
}
