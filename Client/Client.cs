using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {

        private string ipAddress;
        private UInt16 port;
        private TcpClient client;

        private NetworkStream netStream;

        public Client(string ipAddress, UInt16 port, TcpClient client)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.client = client;

            netStream = client.GetStream();
        }

        public void Start()
        {
            Task task = Task.Run(() => {
                this.StartReadThread();
            });

            Task keepAliveThread = Task.Run(() => {
                this.StartKeepAliveThread();
            });

            for (; ; )
            {
                string message = Console.ReadLine();

                if (!message.Equals("quit"))
                {
                    lock (this.netStream)
                    {
                        this.SendMessage(message);
                    }
                }
                else break;
            }

            netStream.Close();
            client.Close();
        }

        private void StartReadThread()
        {
            int BUFFER_SIZE = 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            string stringBuffer = "";

            for (; ; )
            {
                int readBytes = netStream.Read(buffer, 0, BUFFER_SIZE);
                stringBuffer += Encoding.UTF8.GetString(buffer, 0, readBytes);

                int newLineIndex;
                while ((newLineIndex = stringBuffer.IndexOf("\r\n")) != -1)
                {
                    string message = stringBuffer.Substring(0, newLineIndex);
                    stringBuffer = stringBuffer.Substring(newLineIndex + 2);

                    Console.WriteLine(message);
                }
            }
        }

        private void StartKeepAliveThread()
        {
            for (; ; )
            {
                lock (this.netStream)
                {
                    this.SendMessage("KA");
                }
                Thread.Sleep(5000);
            }
        }

        public void SendMessage(string message)
        {
            byte[] finalMessage = Encoding.UTF8.GetBytes(message + "\r\n");
            this.netStream.Write(finalMessage, 0, finalMessage.Length);
        }

    }
}
