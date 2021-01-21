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

        private string login = null;

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

        public bool isLoggedIn()
        {
            return login != null;
        }
        public void notifyLogin(string login)
        {
            this.login = login;
        }
        public void notifyLogout()
        {
            login = null;
        }

        public NetworkStream NetStream { get => netStream; }
        public string Login { get => login; }

    }
}
