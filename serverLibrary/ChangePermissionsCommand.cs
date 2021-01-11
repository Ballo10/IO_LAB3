using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    class ChangePermissionsCommand : Command
    {
        public ChangePermissionsCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            lock(Server.Permissions)
            {
                if(args.Length==2 && String.Compare(Server.Permissions[session.Login],"admin")==0)
                {
                    if(Server.Permissions.ContainsKey(args[0]))
                    {
                        Server.Permissions[args[0]] = args[1];
                        try
                        {
                            foreach (var line in File.ReadLines("login.txt"))
                            {
                                string[] separators = { " ", "\n", "\r", "\t" };
                                string[] temp = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                if (temp[0].Equals(args[0]))
                                {
                                    System.IO.File.AppendAllText("templogin.txt", args[0] + ' ' + Server.Database[args[0]] + ' ' + Server.Permissions[args[0]] + '\n');
                                }
                                else
                                {
                                    System.IO.File.AppendAllText("templogin.txt", line + '\n');
                                }
                            }

                            File.Delete("login.txt");
                            File.Move("templogin.txt", "login.txt");
                            session.SendMessage("User permissions changed successfully");
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
                        session.SendMessage("Nie ma takiego użytkownika");
                    }
                }
                else if(String.Compare(Server.Permissions[session.Login], "admin") != 0)
                {
                    session.SendMessage("Access denied!");
                }
                else
                {
                    session.SendMessage("chperm [name] [permissions]");
                }
            }
        }
    }
}
