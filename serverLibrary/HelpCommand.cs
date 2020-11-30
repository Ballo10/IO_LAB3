using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Klasa odpowiedzialna za wyswietlanie okna pomocy - jakie komendy wystepuja
/// </summary>
namespace ServerLib
{
    public class HelpCommand : Command
    {
        public HelpCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            session.SendMessage("Command list:\n");
            for(int i=0;i<Server.Commands.Count;i++)
            {
                session.SendMessage(Server.Commands.ElementAt(i).Key);
            }
        }
    }
}
