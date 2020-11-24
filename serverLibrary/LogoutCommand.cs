using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class LogoutCommand : Command
    {
        public LogoutCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            if (args.Length > 0)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            if(session.Active)
            {
                session.Active = false;
                session.Login = "";
                session.SendMessage("Wylogowano pomyslnie");
            }
            else
            {
                session.SendMessage("Nie mozna sie wylogowac. Uzytkownik niezalogowany.");
            }
        }
    }
}
