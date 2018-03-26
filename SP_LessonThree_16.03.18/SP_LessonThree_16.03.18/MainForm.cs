using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_LessonThree_16._03._18
{
    public partial class MainForm : Form
    {
        Mutex mutex;
        public MainForm()
        {
            mutex = new Mutex(true, "MyMutex", out bool IsCreate);
            if(!IsCreate)
                mutex.WaitOne();

            InitializeComponent();

            buttonStart.Tag = false;

            //if (!IsCreate)
            //{
            //    MessageBox.Show("Программа уже запущена");
            //    Close();
            //}
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mutex.ReleaseMutex();
        }


        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if ((bool)buttonStart.Tag == true)
            {//Сервер запущен - Отключение сервера
                if (StopServer())
                    buttonStart.Text = "Start";
            }
            else
            {//Сервер не запущен - Запуск сервера
                if (StartServer())
                    buttonStart.Text = "Stop";
            }
            buttonStart.Tag = !(bool)buttonStart.Tag;
        }

        private bool StopServer()
        {
            if (srvSocket != null) return false;
            srvSocket.Shutdown(SocketShutdown.Both);
            return true;
        }

        int port;
        Socket srvSocket;
        Thread srvThread;
        
        private bool StartServer()
        {
            port = 1234;
            if (srvSocket != null) return false;
            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint srvEndPoint = new IPEndPoint(0, port);
            srvSocket.Bind(srvEndPoint);
            srvSocket.Listen(10);

            ThreadPool.SetMaxThreads(16, 32);

            srvThread = new Thread(SrvRoutine);
            srvThread.Start();
            srvThread.IsBackground = true;
            return true;
        }

        private void SrvRoutine()
        {
            Socket clientSocket;
            while (true)
            {
                clientSocket = srvSocket.Accept();
                if (clientSocket == null)
                    break;
                ThreadPool.QueueUserWorkItem(ClientThread, clientSocket);
            }
        }
        private void ClientThread(object state)
        {
            Socket clientSocket = (Socket)state;
            textBox1.Text += "Client connected!\n";
            textBox1.Text += clientSocket.RemoteEndPoint.ToString() + "\n";
            string str = "Hello!";
            byte[] bstr = Encoding.ASCII.GetBytes(str);
            clientSocket.Send(bstr);
            bstr = new byte[1024];
            int size = clientSocket.Receive(bstr);
            Encoding.ASCII.GetString(bstr, 0, size);
            textBox1.Text += str;
            clientSocket.Shutdown(SocketShutdown.Both);
        }
    }
}
