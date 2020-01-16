using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace TCPServerClientApplication
{
    public partial class ServerSide : Form
    {
        delegate void SetTextCallback(string text);

        private string ipAddress = "";
        private int port = 13000;

        private int portForClient = 0;

        public ServerSide()
        {
            InitializeComponent();
        }
        
        private void PortButton_Click(object sender, EventArgs e)
        {
            ipAddress = textBox1.Text;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            informationBox.Text += "Destination IP: " + ipAddress + " Port: " + port + Environment.NewLine;
            portForClient = GetAvailablePort();
            try
            {
                s.Connect(ep);
                s.Send(Encoding.ASCII.GetBytes(portForClient.ToString()), 0, portForClient.ToString().Length, SocketFlags.None);
                s.Close();

                Console.WriteLine("Server sent available port.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }

        private int GetAvailablePort()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();

            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            informationBox.Text += $"Current assigned port is: {port}{Environment.NewLine}";

            return port;
        }
    }
}
