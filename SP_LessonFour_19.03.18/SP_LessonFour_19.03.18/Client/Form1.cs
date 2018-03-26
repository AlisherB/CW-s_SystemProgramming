using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        Socket srvSocket;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                srvSocket.Connect("127.0.0.1", 1234);
                this.Text = "Connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(txtMessage.Text);
                srvSocket.Send(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            srvSocket.Shutdown(SocketShutdown.Both);
            //srvSocket.Disconnect(false);
            //srvSocket.Dispose();
            srvSocket = null;
        }
    }
}
