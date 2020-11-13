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
            if(args.Length!=3)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            lock(Server.Database)
            {
                if (success = Server.Database.ContainsKey(args[1]))
                {
                    success = Server.Database[args[1]].Equals(args[2]);
                }
            }
            if (success)
            {
                session.SendMessage("Udalo sie zalogowac");
                session.Active = true;
                session.Login = args[1];
            }
            else session.SendMessage("Nie udalo sie zalogowac");
        }
    }
}
