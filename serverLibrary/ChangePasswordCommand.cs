using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class ChangePasswordCommand : Command
    {
        public ChangePasswordCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if(args.Length != 3)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage chpwd [old_password] [new_password] [new_password]");
                return;
            }

            bool success = false;

            lock (Server.Database)
            {
                if (Server.Database[session.Login].Equals(args[0]))
                {
                    success = args[1].Equals(args[2]);
                    if(success) Server.Database[session.Login] = args[1];
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
                                System.IO.File.AppendAllText("templogin.txt", session.Login + ' ' + args[1] + '\n');
                            }
                            else
                            {
                                System.IO.File.AppendAllText("templogin.txt", line + '\n');
                            }
                        }

                        File.Delete("login.txt");
                        File.Move("templogin.txt", "login.txt");
                        session.SendMessage("Password changed successfully");
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
