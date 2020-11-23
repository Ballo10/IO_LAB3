using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Klasa odpowiedzialna za wyswietlenie historii logowania
/// </summary>
namespace ServerLib
{
    public class LoginHistoryCommand : Command
    {
        public LoginHistoryCommand(TextServerAsync server) : base(server)
        {
        }

        public override void execute(string[] args, Session session)
        {
            if (args.Length > 0)
            {
                session.SendMessage("Bledne dane");
                return;
            }

            string data = File.ReadAllText("historia.txt");

            session.SendMessage(data);
        }
    }
}
