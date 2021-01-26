using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class MessageCommand : Command
    {
        public MessageCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length < 2)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage msg [login] [message]");
                return;
            }

            string login = args[0];
            string message = args[1];

            for (int i = 2; i < args.Length; ++i)
            {
                message += " " + args[i];
            }

            lock (Server.ActiveUsers)
            {
                if (!session.isLoggedIn())
                {
                    session.SendMessage("You are not logged in");
                    return;
                }

                if (Server.ActiveUsers.ContainsKey(login))
                {
                    Server.ActiveUsers[login].SendMessage("[" + session.Login + "]: " + message);
                }
                else
                {
                    session.SendMessage("Target user is not logged in");
                }
            }
        }
    }
}
