using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SP_LessonTwo_14._03._18
{
    public partial class Form1 : Form
    {
        //static int idx = 0;
        private bool isStarted;
        Thread th1;
        Mutex mutex1;
        public Form1()
        {
            InitializeComponent();
            isStarted = false;
            button1.Text = "Start";
            mutex1 = new Mutex(false, "MyMutex", out bool IsCreated);
            if (IsCreated)
                Text = "MyMutex is created";
            else
                Text = "MyMutex is opened";
            //Text = idx.ToString() + ": " + Thread.CurrentThread.ManagedThreadId;
            //idx++;
        }
        
        private void Button1_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(10, 10, 300, 300);
            if (!isStarted)
            {
                isStarted = !isStarted;
                th1 = new Thread(ThreadFunction);
                th1.IsBackground = true;
                th1.Start(rect);
                button1.Text = "Pause";
            }
            else
            {
                int a = (int)th1.ThreadState;
                int ss = (int)ThreadState.Suspended;
                if ((a & ss) == 0)
                {
                    mutex1.WaitOne();
                    //th1.Suspend();
                    button1.Text = "Resume";
                }
                else
                {
                    mutex1.ReleaseMutex();
                    //th1.Resume();
                    button1.Text = "Pause";
                }   
            }
        }
        private void ThreadFunction(object obj)
        {
            //подготовка к синхронизации потока
            Rectangle rect = (Rectangle)obj;
            DrawLines(rect);
            //завершение потока
        }
        private void DrawLines(Rectangle rect)
        {
            while (true)
            {
                Graphics gr = CreateGraphics();
                Random rand = new Random();
                Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                float width = rand.Next(1, 4);
                Pen pen = new Pen(randomColor, width);
                float x1, x2, x3, x4, y1, y2, y3, y4;
            
                x1 = rect.X;
                y1 = rand.Next(rect.Top, rect.Bottom);

                x2 = rand.Next(rect.X, rect.X + rect.Width);
                y2 = rect.Y;

                x3 = rect.X + rect.Width;
                y3 = rand.Next(rect.Top, rect.Bottom);

                x4 = rand.Next(rect.X, rect.X + rect.Width);
                y4 = rect.Bottom;

                gr.DrawLine(pen, x1, y1, x2, y2);
                gr.DrawLine(pen, x2, y2, x3, y3);
                gr.DrawLine(pen, x3, y3, x4, y4);
                gr.DrawLine(pen, x4, y4, x1, y1);

                mutex1.ReleaseMutex();
                Thread.Sleep(100);
            }
        }

        //private void MyRoutine()
        //{
        //    Application.Run(new Form1());
        //}
    }
}
