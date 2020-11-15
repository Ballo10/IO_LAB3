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
    public class RegisterCommand : CommandHandler
    {
        public RegisterCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            bool success = false;
            if(args.Length!=3)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            lock (Server.Database)
            {
                if (!Server.Database.ContainsKey(args[1]))
                {
                    Server.Database.Add(args[1], args[2]);
                    System.IO.File.AppendAllText("login.txt", args[1]+" "+args[2]+'\n');
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
