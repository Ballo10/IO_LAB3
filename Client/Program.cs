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
                    Console.WriteLine("Podaj adres ip");
                    ipAddress = Console.ReadLine();

                    Console.WriteLine("Podaj port");
                    port = Convert.ToUInt16(Console.ReadLine());

                    tcpClient = new TcpClient(ipAddress, port);
                    valid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Błąd: " + e.Message);
                    Console.WriteLine("Spróbuj ponownie.");
                }
            }

            Console.WriteLine("Połączono pomyślnie.");

            Client client = new Client(ipAddress, port, tcpClient);
            client.Start();
        }

    }
}
