using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;


namespace Server
{
    public partial class FrmServer : Form
    {
        Socket srvSocket;
        bool IsStop;
        Mutex mxServer, mxTxtLog;
        public FrmServer()
        {
            bool IsCreate = true;
            mxServer = new Mutex(true, "MyServerMutex", out IsCreate);
            mxTxtLog = new Mutex(false);
            IsStop = false;
            srvSocket = null;
            thServer = null;
            InitializeComponent();
            if (IsCreate == false)
            {
                MessageBox.Show("Программа Серевер уже запущена!");
                Application.Exit();
            }
            txtLog.Text = "This host IP addresses\r\n";
            IPAddress []addr = Dns.GetHostAddresses(Dns.GetHostName());

            foreach(IPAddress a in addr)
            {
                txtLog.Text += a.ToString();
                txtLog.Text += "\r\n";
            }
            txtLog.Text += "\r\n";
            txtLog.Select(txtLog.TextLength, 0);
        }

        Thread thServer;
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (thServer != null) return;
            thServer = new Thread(ServerThread);
            thServer.IsBackground = true;
            thServer.Start();
        }
        void ServerThread()
        {
            if (srvSocket != null)
            {
                txtLog.Text += "Поток уже создан!\r\n";
                return;
            }
            txtLog.Text += "Создан поток сервера\r\n";
            txtLog.Text += "IsThreadPoolThread=" + Thread.CurrentThread.IsThreadPoolThread.ToString() + "\r\n";

            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipSrv = IPAddress.Parse("0.0.0.0");
            IPEndPoint srvEndPoint = new IPEndPoint(ipSrv, 1234);

            srvSocket.Bind(srvEndPoint);
            srvSocket.Listen(100);

            while (!IsStop)
            {
                Socket client = srvSocket.Accept();
                txtLog.Text += "Client conected\r\n";
                ThreadPool.SetMinThreads(16, 16);
                ThreadPool.SetMaxThreads(32, 32);
                ThreadPool.QueueUserWorkItem(ClientProc, client);
            }
        }
        void ClientProc(object param)
        {
            Socket client = (Socket)param;
            txtLog.Invoke(new Action<string>((x) => txtLog.Text += x), "Client conected\r\n");
            txtLog.Invoke(new Action<string>((x) => txtLog.Text += x), "IsThreadPoolThread id = " + Thread.CurrentThread.ManagedThreadId.ToString() + "\r\n");
           
            byte[] readBuf = new byte[1024];
   
            try
            {
                //mxTxtLog.WaitOne();

                //Monitor.Enter( txtLog );
                //lock(txtLog)
              //{
                while (client.Connected)
                {
                    if (client.Receive(readBuf) <= 0)
                    break;
                    string msg = Encoding.ASCII.GetString(readBuf);
                    // тут запись в разделяемый ресурс - txtLog.Text,
                    // который следует синхронизировать между клиентскими потоками

                    //mxTxtLog.WaitOne();
                    //txtLog.Text += "Client MSG: " + msg;
                    txtLog.Invoke(new Action<string>((x) => txtLog.Text += x), "Client MSG: " + msg);
                        
                    if (msg.Length == 0) break;
                }
                //}
                //Monitor.Exit(txtLog);
            }
            catch (Exception ex)
            {
                //Monitor.Exit(txtLog);
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                mxTxtLog.ReleaseMutex();
                client.Shutdown(SocketShutdown.Both);
            }
        }
    }
}
