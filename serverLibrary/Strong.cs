using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{

    public class StrongCommand : CommandHandler
    {
        public StrongCommand(TextServerAsync server) : base(server)
        {
        }

        public override void execute(string[] args, Session session)
        {
            if (args.Length != 2)
            {
                session.SendMessage("Bledne dane");
                return;
            }
            uint value = 0;
            uint result = 1;
            if (UInt32.TryParse(args[1], out value))
            {
                for (; value > 1; value--)
                {
                    result = result * value;
                }
                session.SendMessage("Wynik: "+result);
            }
            else
            {
                session.SendMessage("Bledne dane");
                return;
            }
        }
    }
}
