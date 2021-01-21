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

            string login = args[0];
            string password = args[1];

            DateTime thisDay = DateTime.UtcNow.AddHours(1);

            String time = thisDay.ToString("dd-MM-yyyy HH:mm:ss");
            string line = "";

            lock (Server.Database)
            {
                if (Server.Database.ContainsKey(login))
                {
                    bool passwordValid = Server.Database[login].Password.Equals(password);

                    if (passwordValid)
                    {
                        lock (Server.ActiveUsers)
                        {
                            if (Server.ActiveUsers.ContainsKey(login))
                            {
                                session.SendMessage("You are already logged in ");
                                return;
                            }
                            else
                            {
                                session.SendMessage("Successful login");

                                lock (session)
                                {
                                    session.notifyLogin(login);
                                }
                                
                                Server.ActiveUsers[login] = session;

                                line = "Successful user login: " + login + " at: " + time + '\n';
                            }
                        }
                    }
                    else
                    {
                        session.SendMessage("Login failed");
                        line = "Failed user login: " + login + " at: " + time + '\n';
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
    }
}
