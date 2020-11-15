using ServerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ServerLib
{
    public class TextServerAsync : TextServer
    {
        private Dictionary<string, string> database = new Dictionary<string,string>();

        private Dictionary<string, CommandHandler> commands = new Dictionary<string, CommandHandler>();

        public delegate void ProcessClientDelegate(Session session);

        public TextServerAsync(IPAddress IP, int port) : base(IP, port)
        {
            ///Wczytanie bazy danych z pliku
            string data = File.ReadAllText("login.txt");
            string[] separator = { " ", "\n", "\r", "\t" };
            string[] tab = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tab.Length; i+=2)
            {
                database.Add(tab[i], tab[i + 1]);
            }

            commands.Add("login", new LoginCommand(this));
            commands.Add("silnia", new StrongCommand(this));
            commands.Add("historia", new LoginHistoryCommand(this));
            commands.Add("register", new RegisterCommand(this));
            commands.Add("help", new HelpCommand(this));


            Console.WriteLine("Started");
        }
        /// <summary>
        ///     Funkcja sprawdzająca dane do "logowania". Dane zapisane w pliku porównywane z danymi przesłanymi przez użytkownika
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="passwd">Hasło</param>
        /// <returns>
        /// True - udane logowanie
        /// False - nieudane logowanie, niepoprawne dane
        /// </returns>
        private bool checkLogin(string login, string passwd)
        {
            bool result = false;
            string temp1 = login.Replace("\0", string.Empty);
            string temp2 = passwd.Replace("\0", string.Empty);
            if (result = database.ContainsKey(login.Replace("\0", string.Empty)))
            {
                result = database[temp1].Equals(temp2);
            }
            return result;
        }

 

        private void ProcessClient(Session session)
        {
            byte[] buffer = new byte[BufferSize];

            string stringBuffer = "";

            for (;;)
            {
                int readBytes = session.NetStream.Read(buffer, 0, BufferSize);
                stringBuffer += Encoding.UTF8.GetString(buffer, 0, readBytes);

                int newLineIndex;
                while ((newLineIndex = stringBuffer.IndexOf("\r\n")) != -1)
                {
                    string cmd = stringBuffer.Substring(0, newLineIndex);
                    stringBuffer = stringBuffer.Substring(newLineIndex + 2);

                    string[] args = cmd.Split(' ');

                    if (args.Length >= 1 && commands.ContainsKey(args[0]))
                    {
                        commands[args[0]].execute(args, session);
                    }
                    else
                    {
                        session.SendMessage("Zla komenda!");
                    }
                }
            }
        }
        private void ProcessClientCallback(IAsyncResult ar)
        {
            Session session = (Session)ar.AsyncState;
            session.NetStream.Close();
            session.TcpClient.Close();
        }

        public override void Start()
        {
            TcpListener = new TcpListener(IPAddress, Port);
            TcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();

                ProcessClientDelegate transmissionDelegate = new ProcessClientDelegate(ProcessClient);

                Session session = new Session(this,tcpClient);

                transmissionDelegate.BeginInvoke(session, ProcessClientCallback, session);

                //var task = Task.Run(() => transmissionDelegate.Invoke(netStream));
            }
        }

        public Dictionary<string, string> Database { get => database; }
        public Dictionary<string, CommandHandler> Commands { get => commands;  }

    }
}

