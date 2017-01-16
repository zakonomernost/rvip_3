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

namespace WindowsFormsApplication1
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            this.MinimizeBox = false;
        }
        static string output = "";
        System.Threading.Thread t;
        public void createListener()
        {
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.textBox1.Text = "Start server on localhost" + Environment.NewLine;
                    this.textBox1.Text += "Listener created" + Environment.NewLine;
                    this.textBox1.Refresh();
                }));
            }
            // Create an instance of the TcpListener class.
            TcpListener tcpListener = null;
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            try
            {
                // Set the listener on the local IP address
                // and specify the port.
                tcpListener = new TcpListener(ipAddress, 13);
                tcpListener.Start();
                output = "Waiting for a connection...";
            }
            catch (Exception e)
            {
                output = "Error: " + e.ToString();
                MessageBox.Show(output);
            }
            while (true)
            {
                // Always use a Sleep call in a while(true) loop
                // to avoid locking up your CPU.
                Thread.Sleep(10);
                // Create a TCP socket.
                // If you ran this server on the desktop, you could use
                // Socket socket = tcpListener.AcceptSocket()
                // for greater flexibility.
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                // Read the data stream from the client.
                byte[] bytes = new byte[256];
                NetworkStream stream = tcpClient.GetStream();
                stream.Read(bytes, 0, bytes.Length);
                SocketHelper helper = new SocketHelper();
                helper.processMsg(tcpClient, stream, bytes, textBox1, this);
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.textBox1.Text += Environment.NewLine;
                        this.textBox1.Refresh();
                    }));
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            this.button1.Hide();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.createListener();
        }
    }
}
