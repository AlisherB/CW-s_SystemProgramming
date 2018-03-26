using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Console;
using System.Threading;

namespace SP_LessonFive_26._03._18
{
    class Program
    {
        static FileStream file, fileOut;
        static IAsyncResult ar = null;
        static void Main(string[] args)
        {
            string fileName = "Z:\\123.bin";
            try
            {
                file = new FileStream(fileName, FileMode.Open);
                long size = file.Length;
                WriteLine("file.Length = {0}", size);
                byte[] buf = new byte[size];

                ar = file.BeginRead(buf, 0, (int)size, null, null);
                //ar = file.BeginRead(buf, 0, (int)size, EndReadCallBack, file);

                WriteLine("ID основного потока: {0}", Thread.CurrentThread.ManagedThreadId);

                WriteLine("Старт чтения в асинхронном режиме...");

                int readSize = file.EndRead(ar);
                WriteLine("Прочтено {0} байт из файла.", readSize);
                fileOut = new FileStream("Z:\\1.bin", FileMode.Create);
                fileOut.BeginWrite(buf, 0, buf.Length, EndWriteCallBack, ar);
                WriteLine("Старт записи в асинхронном режиме...");
                ar.AsyncWaitHandle.WaitOne();
                WriteLine("Завершение записи в асинхронном режиме...");
                /*
                * Рабочий код
                ar.AsyncWaitHandle.WaitOne();
                WriteLine("Операция асинхронного чтения завершена!");
                */

                /*
                * Примерный код системного потока асинхронного чтения
                while (!ar.IsCompleted) 
                {
                    // Оперция чтения
                }
                ((ManualResetEvent)ar.AsyncWaitHandle).Set();
                EndReadCallbak(obj);
                */
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
                //return;
            }
            ReadLine();
        }

        private static void EndWriteCallBack(IAsyncResult ar)
        {
            fileOut.EndWrite(ar);
            WriteLine("Завершена асинхронная запись.");
            WriteLine("ID основного потока: {0}", Thread.CurrentThread.ManagedThreadId);
        }

        //private static void EndReadCallBack(object obj)
        //{
        //    IAsyncResult ar = (IAsyncResult)obj;
        //    int readSize = file.EndRead(ar);
        //    WriteLine("Прочтено {0} байт из файла!", readSize);
        //    //возможен код для сигнализации о завершении операции чтения
        //}
    }
}
