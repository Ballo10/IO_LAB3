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
    public abstract class Command
    {

        #region Fields

        private TextServerAsync server;

        #endregion

        #region Constructors

        public Command(TextServerAsync server)
        {
            this.server = server;
        }

        #endregion

        #region Functions

#pragma warning disable IDE1006 // Style nazewnictwa
        public abstract void execute(string[] args, Session session);
#pragma warning restore IDE1006 // Style nazewnictwa

        #endregion

        public TextServerAsync Server { get => server; }

    }
}
