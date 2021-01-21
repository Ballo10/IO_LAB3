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

            string oldPassword = args[0];
            string newPassword = args[1];
            string newPasswordConfirm = args[2];

            if (!session.isLoggedIn())
            {
                session.SendMessage("You are not logged in");
                return;
            }

            bool success = false;

            lock (Server.Database)
            {
                User currentUser = Server.Database[session.Login];

                if (currentUser.Password.Equals(oldPassword))
                {
                    if (newPassword.Equals(newPasswordConfirm))
                    {
                        currentUser.Password = newPassword;
                        success = true;
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
                                System.IO.File.AppendAllText("templogin.txt", session.Login + ' ' + newPassword +' '+ currentUser.Permission +'\n');
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
                else
                {
                    session.SendMessage("Password is invalid or new password confirmation doesn't match new password");
                }
            }
        }
    }
}
