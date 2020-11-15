﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class HelpCommand : CommandHandler
    {
        public HelpCommand(TextServerAsync server) : base(server)
        {

        }
        public override void execute(string[] args, Session session)
        {
            session.SendMessage("Lista komend:\n");
            for(int i=0;i<Server.Commands.Count;i++)
            {
                session.SendMessage(Server.Commands.ElementAt(i).Key);
            }
        }
    }
}
