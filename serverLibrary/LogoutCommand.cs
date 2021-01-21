using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class LogoutCommand : Command
    {
        public LogoutCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length > 0)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage logout");
                return;
            }

            lock (Server.ActiveUsers)
            {
                if (session.isLoggedIn())
                {
                    Server.ActiveUsers.Remove(session.Login);

                    lock (session)
                    {
                        session.notifyLogout();
                    }

                    session.SendMessage("Successfully logged out ");
                }
                else
                {
                    session.SendMessage("Unable to log out. User not logged in.");
                }
            }
        }
    }
}
