using System;
using System.Collections.Generic;
using System.IO;
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
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage login [login] [password]");
                return;
            }
            if (session.Active)
            {
                session.SendMessage("You are already logged in ");
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
                session.SendMessage("Successful login");
                session.Active = true;
                session.Login = args[0];
                line = "Successful user login: " + args[0] + " at: " + time + '\n';

            }
            else
            {
                session.SendMessage("Login failed");
                session.Login = args[0];
                line = "Failed user login: " + args[0] + " at: " + time + '\n';

            }

            try
            {
                System.IO.File.AppendAllText("historia.txt", line);

            }
            catch (IOException e)
            {
                session.SendMessage("The file could not be read");
                File.Create("login.txt");
                //Console.WriteLine(e.Message);
            }
        }
    }
}
