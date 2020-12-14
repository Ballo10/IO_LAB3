using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Klasa przechowuje informacje o sesji i pozwala na latwe przesylanie komunikatow.
/// </summary>

namespace ServerLib
{
    public class Session
    {
        private TextServerAsync server;

        private TcpClient tcpClient;
        private NetworkStream netStream;

        private bool active = false;
        private string login = "";

        public Session(TextServerAsync ts, TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            server = ts;
            netStream = tcpClient.GetStream();
        }

        public void SendMessage(string message)
        {
            lock (this.netStream)
            {
                byte[] finalMessage = Encoding.UTF8.GetBytes(message + "\r\n");
                this.netStream.Write(finalMessage, 0, finalMessage.Length);
            }
        }

        public NetworkStream NetStream { get => netStream; }
        public bool Active { get => active; set => active = value; }
        public string Login { get => login; set => login = value; }
    }
}
