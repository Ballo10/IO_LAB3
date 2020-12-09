using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Klasa, ktora pozwala na rejestacje klienta
/// </summary>

namespace ServerLib
{
    public class RegisterCommand : Command
    {
        public RegisterCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length != 2)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage [login] [password]");
                return;
            }

            lock (Server.Database)
            {
                if (!Server.Database.ContainsKey(args[0]))
                {
                    Server.Database.Add(args[0], args[1]);
                    try
                    {
                        System.IO.File.AppendAllText("login.txt", args[0] + " " + args[1] + '\n');
                        session.SendMessage("User successfully registered");
                    }
                    catch (IOException e)
                    {
                        session.SendMessage("The file could not be read");
                        File.Create("login.txt");
                        //Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    session.SendMessage("The user already exists in the database");
                }
            }
        }
    }
}
