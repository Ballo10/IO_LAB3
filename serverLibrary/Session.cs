using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class Session
    {
        private TcpClient tcpClient;
        private NetworkStream netStream;

        private bool active = false;
        private string login = "";

        public Session(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            netStream = tcpClient.GetStream();
        }

        public TcpClient TcpClient { get => tcpClient; }
        public NetworkStream NetStream { get => netStream; }
        public bool Active { get => active; set => active = value; }
        public string Login { get => login; set => login = value; }
    }
}
