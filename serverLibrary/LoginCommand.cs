using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class LoginCommand : CommandHandler
    {

        public LoginCommand(TextServerAsync server) : base(server)
        {

        }

        public override void execute(string[] args, Session session)
        {
            bool success = false;
            lock(Server.Database)
            {
                
            }
        }
    }
}
