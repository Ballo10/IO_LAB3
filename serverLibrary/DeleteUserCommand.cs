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

            string password = args[0];
            string passwordConfirm = args[1];

            if (!session.isLoggedIn())
            {
                session.SendMessage("You are not logged in");
                return;
            }

            if (!password.Equals(passwordConfirm))
            {
                session.SendMessage("Incorrect password confirmation");
                return;
            }

            lock (Server.Database)
            {
                User currentUser = Server.Database[session.Login];

                if (currentUser.Password.Equals(password))
                {
                    string deletedLogin = session.Login;
                    lock (Server.ActiveUsers)
                    {
                        Server.ActiveUsers.Remove(session.Login);
                        lock (session)
                        {
                            session.notifyLogout();
                        }
                    }

                    Server.Database.Remove(deletedLogin);

                    try
                    {
                        foreach (var line in File.ReadLines("login.txt"))
                        {
                            string[] separators = { " ", "\n", "\r", "\t" };
                            string[] temp = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            if (!temp[0].Equals(deletedLogin))
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
                else
                {
                    session.SendMessage("Incorrent password");
                }
            }

        }
    }
}
