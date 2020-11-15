using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerLib;

/// <summary>
/// Klasa abstrakcyjna, ktora sluzy implementacji kolejnych polecen
/// </summary>
namespace ServerLib
{
    public abstract class CommandHandler
    {

        #region Fields

        private TextServerAsync server;

        #endregion

        #region Constructors

        public CommandHandler(TextServerAsync server)
        {
            this.server = server;
        }

        #endregion

        #region Functions

        public abstract void execute(string[] args, Session session);

        #endregion

        public TextServerAsync Server { get => server; }

    }
}
