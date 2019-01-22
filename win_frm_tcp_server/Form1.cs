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

namespace win_frm_tcp_server
{
    public partial class Tcp_seerver : Form
    {

        Socket sock;
        Socket acc;
        public Tcp_seerver()
        {
            InitializeComponent();
        }

        Socket socket()
        {

            return new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sock = socket();
            sock.Bind(new IPEndPoint(0,3));
            sock.Listen(0);

            new Thread(delegate()
                {
                    acc = sock.Accept();
                    MessageBox.Show("connection accepted");
                    sock.Close();

                    while (true)
                    {
                        try
                        {

                            byte[] buffer = new byte[255];
                            int rec = acc.Receive(buffer, 0, buffer.Length, 0);

                            if (rec <= 0)
                            {
                                throw new SocketException();
                            }
                            Array.Resize(ref buffer, rec);

                            Invoke((MethodInvoker)delegate
                            {
                                listBox1.Items.Add(Encoding.Default.GetString(buffer));
                            });
                        }
                        catch
                        {
                            MessageBox.Show("disconnection");
                            break;

                        }
                    }
                    Application.Exit();

                }).Start();

        }

        private void Send_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Default.GetBytes(textBox1 .Text);
            acc.Send(data ,0,data.Length,0);

        }
    }
}
