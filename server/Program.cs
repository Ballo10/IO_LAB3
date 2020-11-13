using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServerLib;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
        }
    }
}
