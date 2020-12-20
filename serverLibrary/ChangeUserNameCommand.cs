using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class ChangeUserNameCommand : Command
    {
        public ChangeUserNameCommand(TextServerAsync server) : base(server)
        {

        }

        public override void execute(string[] args, Session session)
        {
            if(args.Length!=2)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage chname [new_name] [new_name]");
                return;
            }

            bool success = false;

            lock(Server.Database)
            {
                if(!Server.Database.ContainsKey(args[0]))
                {
                    success = args[0].Equals(args[1]);
                    if (success)
                    {
                        string tempPasswd = Server.Database[session.Login];
                        Server.Database.Remove(session.Login);
                        Server.Database.Add(args[0], tempPasswd);
                        args[1] = tempPasswd;
                        //session.Login = args[0];
                    }
                    else
                    {
                        session.SendMessage("Usage chname [new_name] [new_name]");
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
                                session.Login = args[0];
                                System.IO.File.AppendAllText("templogin.txt", session.Login + ' ' + args[1] + '\n');
                            }
                            else
                            {
                                System.IO.File.AppendAllText("templogin.txt", line + '\n');
                            }
                        }

                        File.Delete("login.txt");
                        File.Move("templogin.txt", "login.txt");
                        session.SendMessage("User name changed successfully");
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
