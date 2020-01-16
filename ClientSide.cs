using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace TCPClientApplication
{
    public partial class ClientSide : Form
    {
        //IP Address is local.
        private string ipAddress;

        public ClientSide()
        {
            InitializeComponent();
        }
        
        int port = 13000;
        Socket requestPortListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void RequestPortButton_Click(object sender, EventArgs e)
        {
            ipAddress = textBox1.Text;
            RequestForPort();
        }

        private void RequestForPort()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            requestPortListener.Bind(ep);
            requestPortListener.Listen(100);

            Console.WriteLine("Client requested an available port.");
            Socket serverPortReply = default(Socket);
            serverPortReply = requestPortListener.Accept();

            Thread clientThread = new Thread(new ThreadStart(() => ConnectedClient(serverPortReply, requestPortListener)));
            clientThread.Start();
        }
        string assignedPort;
        private delegate void setTextDel();

        private void ConnectedClient(Socket serverPortReply, Socket requestPortListener)
        {
            byte[] messageFromServer = new byte[1024];
            int size = serverPortReply.Receive(messageFromServer);
            assignedPort = Encoding.ASCII.GetString(messageFromServer, 0, size);


            DataToSendTextBox.Invoke(new setTextDel(SetText));
            Console.WriteLine(assignedPort + " is assigned as new port");

            serverPortReply.Close();
            requestPortListener.Close();
        }

        private void SetText()
        {
            DataToSendTextBox.Text = assignedPort;
        }
    }
}
