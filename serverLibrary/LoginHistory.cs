using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    class LoginHistoryCommand : CommandHandler
    {
        public LoginHistoryCommand(TextServerAsync server) : base(server)
        {
        }

        public override void execute(string[] args, Session session)
        {
            if (args.Length > 1)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            //wyswietlenie pliku
            string data = File.ReadAllText("historia.txt");
           // string data = File.ReadAllText("test.txt");
           
            
            
                session.SendMessage(data);
            
        }
    }
}
