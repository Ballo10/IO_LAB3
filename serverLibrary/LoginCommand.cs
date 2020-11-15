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
            if(session.Active)
            {
                session.SendMessage("Jestes juz zalogowany");
            }
            lock(Server.Database)
            {
                if (success = Server.Database.ContainsKey(args[1]))
                {
                    success = Server.Database[args[1]].Equals(args[2]);
                }
            }


            DateTime thisDay = DateTime.UtcNow.AddHours(1);
                
            String time = thisDay.ToString("dd-MM-yyyy HH:mm:ss");
            string line = "";
            if (success)
            {
                session.SendMessage("Udalo sie zalogowac");
                session.Active = true;
                session.Login = args[1];
                line = "Udane logowanie przez uzytkownika: " + args[1] + " o godzinie: " + time+'\n';
               
            }
            else
            {
                session.SendMessage("Nie udalo sie zalogowac");
                session.Login = args[1];
                line = "Nieudane logowanie przez uzytkownika: " + args[1] + " o godzinie: " + time+'\n';
                
            }

            System.IO.File.AppendAllText("historia.txt", line);
        }
    }
}
