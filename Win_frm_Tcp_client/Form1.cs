using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Win_frm_Tcp_client
{
    public partial class Client : Form
    {
        Socket sock;
        public Client()
        {
            InitializeComponent();
            sock = socket();
            FormClosing += new FormClosingEventHandler(Client_FormClosing);
        }

        void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            sock.Close();
        }
        Socket socket()
        {

            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            try
            {
                sock.Connect(new IPEndPoint(IPAddress.Parse(textBox1.Text), 3));
                new Thread(() =>
                    {
                        read();
                    }).Start();
            }
            catch
            {
                MessageBox.Show("connection failed");
            }
        }

        void read()
        {
            while (true)
            {
                try
                {
                byte[] buffer = new byte[255];
                int rec = sock.Receive(buffer ,0,buffer.Length,0);
                if (rec <= 0)
                {
                    throw new SocketException();
                }
                Array.Resize(ref buffer,rec);
                
                    Invoke((MethodInvoker)delegate
                {
                    listBox1.Items.Add(Encoding.Default.GetString(buffer));
                });
                }
                catch
                {
                    MessageBox.Show("disconnection");
                    sock.Close();
                    break;

                }
            
            }
            Application.Exit();
        }
        private void Send_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(textBox2 .Text);
            sock.Send(data,0,data.Length ,0);
        }
    }
}
