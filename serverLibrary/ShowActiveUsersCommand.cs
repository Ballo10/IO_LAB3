using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class ShowActiveUsersCommand : Command
    {
        public ShowActiveUsersCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length >0)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage ShowActiveUsersCommand");
                return;
            }

           string message = "";

            foreach (var user in Server.ActiveUsers)
            {
                message += (user+"\n");
            }
            session.SendMessage(message);

        }
    }
}

