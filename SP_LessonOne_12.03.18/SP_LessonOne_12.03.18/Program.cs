using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SP_LessonOne_12._03._18
{
    class Program
    {
        static void Main(string[] args)
        {
            //Process proc = new Process();
            //proc.StartInfo = new ProcessStartInfo("notepad.exe", "Z:\\Системное программирование\\1.txt");
            ////установка приоритета процессу
            ////proc.PriorityClass = ProcessPriorityClass.High;
            //proc.Start();
            //proc.PriorityClass = ProcessPriorityClass.AboveNormal;
            //Console.WriteLine("Process ID: {0} ", proc.Id);
            //Console.WriteLine("MachineName: {0}", proc.MachineName);
            ////ожидание завершения дочернего процесса
            //proc.WaitForExit();
            //Console.WriteLine("Процесс запущен");
            //Console.ReadLine();
            //return;

            Process proc = new Process();
            Process [] procList = Process.GetProcesses();
            Console.WriteLine("Кол-во процессов: {0}", procList.Count());
            Console.WriteLine("Имя процесса\tID\tHandles\tПриоритет\tВремя запуска");
            foreach (var i in procList)
            {
                int BasePriority = -1;
                DateTime dt = DateTime.MinValue;
                try
                {
                    BasePriority = i.BasePriority;
                    dt = i.StartTime;
                }
                catch (Exception) { }

                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", i.ProcessName, i.Id, i.HandleCount, BasePriority, dt);
            }
            Console.ReadLine();
        }
    }
}
