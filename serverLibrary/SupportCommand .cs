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
    public class SupportCommand : Command
    {
        public SupportCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            session.SendMessage("chpwd [old_password] [new_password] [new_password]\n" //kazdy
            +"chname [new_name] [new_name]\n" //kazdy
            +"login [login] [password]\n"
            +"history\n" 
            +"logout\n" 
            +"register [login] [password]\n" //kazdy
            + "delete [password] [password]\n" 
            + "activeusers\n" //kazdy
            + "Strong [number]");
            
           
        }
    }
}
