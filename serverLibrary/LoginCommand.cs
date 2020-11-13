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
                DateTime thisDay = DateTime.Today;
                String time = thisDay.ToString();
            string line = "";
            if (success)
            {
                session.SendMessage("Udalo sie zalogowac");
                session.Active = true;
                session.Login = args[1];
                line = "Udane logowanie przez uzytkownika: " + args[1] + " o godzinie: " + time;
                //zrobic zeby nadpisywalo plik
            }
            else
            {
                session.SendMessage("Nie udalo sie zalogowac");
                session.Login = args[1];
                line = "Nieudane logowanie przez uzytkownika: " + args[1] + " o godzinie: " + time;
                //zrobic zeby nadpisywalo plik
            }

            System.IO.File.WriteAllText("historia.txt", line);
        }
    }
}
