using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {

        static void Main(string[] args)
        {
            string ipAddress = null;
            UInt16 port = 0;
            TcpClient tcpClient = null;

            bool valid = false;
            while (!valid)
            {
                try
                {
                    Console.WriteLine("Enter the ip address");
                    ipAddress = Console.ReadLine();

                    Console.WriteLine("Enter the port number");
                    port = Convert.ToUInt16(Console.ReadLine());

                    tcpClient = new TcpClient(ipAddress, port);
                    valid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Try again.");
                }
            }

            Console.WriteLine("Connected successfully.");

            Client client = new Client(ipAddress, port, tcpClient);
            client.Start();
        }

    }
}
