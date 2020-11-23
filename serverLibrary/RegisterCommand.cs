using System;
using System.Collections.Generic;
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
                session.SendMessage("Bledne dane");
                return;
            }

            lock (Server.Database)
            {
                if (!Server.Database.ContainsKey(args[0]))
                {
                    Server.Database.Add(args[0], args[1]);
                    System.IO.File.AppendAllText("login.txt", args[0] + " " + args[1] + '\n');
                    session.SendMessage("Pomyslnie zarejestrowano uzytkownika");
                }
                else
                {
                    session.SendMessage("Uzytkownik juz istnieje w bazie");
                }
            }
        }
    }
}
