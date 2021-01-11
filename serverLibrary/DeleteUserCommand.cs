using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class DeleteUserCommand : Command
    {
        public DeleteUserCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length != 2)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage delete [password] [password]");
                return;
            }

            bool success = false;

            lock (Server.Database)
            {
                if (!Server.Database.ContainsKey(args[0]))
                {
                    success = args[0].Equals(args[1]);
                    if (success)
                    {
                        string tempPasswd = Server.Database[session.Login];
                        Server.Database.Remove(session.Login);
                    }
                    else
                    {
                        session.SendMessage("Usage delete [password] [password]");
                    }
                }

                if (success)
                {
                    try
                    {
                        foreach (var line in File.ReadLines("login.txt"))
                        {
                            string[] separators = { " ", "\n", "\r", "\t" };
                            string[] temp = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            if (temp[0].Equals(session.Login))
                            {
                                session.Login = "";
                                session.Active = false;
                                 System.IO.File.AppendAllText("templogin.txt", session.Login);
                            }
                            else
                            {
                                System.IO.File.AppendAllText("templogin.txt", line + '\n');
                            }
                        }

                        File.Delete("login.txt");
                        File.Move("templogin.txt", "login.txt");
                        session.SendMessage("User deleted successfully");
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
