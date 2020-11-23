using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Klasa odpowiedzialna za logowanie uzytkownikow i zapisywanie informacji o zalogowaniu do pliku historia.txt
/// </summary>

namespace ServerLib
{
    public class LoginCommand : Command
    {

        public LoginCommand(TextServerAsync server) : base(server)
        {

        }

        public override void execute(string[] args, Session session)
        {
            if (args.Length != 2)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            if (session.Active)
            {
                session.SendMessage("Jestes juz zalogowany");
                return;
            }

            bool success = false;

            lock (Server.Database)
            {
                if (Server.Database.ContainsKey(args[0]))
                {
                    success = Server.Database[args[0]].Equals(args[1]);
                }
            }


            DateTime thisDay = DateTime.UtcNow.AddHours(1);

            String time = thisDay.ToString("dd-MM-yyyy HH:mm:ss");
            string line = "";
            if (success)
            {
                session.SendMessage("Udalo sie zalogowac");
                session.Active = true;
                session.Login = args[0];
                line = "Udane logowanie przez uzytkownika: " + args[0] + " o godzinie: " + time + '\n';

            }
            else
            {
                session.SendMessage("Nie udalo sie zalogowac");
                session.Login = args[0];
                line = "Nieudane logowanie przez uzytkownika: " + args[0] + " o godzinie: " + time + '\n';

            }

            System.IO.File.AppendAllText("historia.txt", line);
        }
    }
}
