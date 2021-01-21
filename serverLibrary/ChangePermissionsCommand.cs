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
            string targetUsername = args[0];
            string targetPerm = args[1];

            if (!session.isLoggedIn())
            {
                session.SendMessage("You are not logged in");
                return;
            }

            lock (Server.Database)
            {
                User currentUser = Server.Database[session.Login];

                if(args.Length==2 && currentUser.Permission.Equals("admin"))
                {

                    if(Server.Database.ContainsKey(targetUsername))
                    {
                        User targetUser = Server.Database[targetUsername];
                        targetUser.Permission = targetPerm;

                        try
                        {
                            foreach (var line in File.ReadLines("login.txt"))
                            {
                                string[] separators = { " ", "\n", "\r", "\t" };
                                string[] temp = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                if (temp[0].Equals(targetUsername))
                                {
                                    System.IO.File.AppendAllText("templogin.txt", targetUsername + ' ' + targetUser.Password + ' ' + targetUser.Permission + '\n');
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
                else if (!currentUser.Permission.Equals("admin"))
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
