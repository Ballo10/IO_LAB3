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
        private Dictionary<string, User> database = new Dictionary<string, User>();

        private Dictionary<string, Command> commands = new Dictionary<string, Command>();

        private Dictionary<string, Session> activeUsers = new Dictionary<string, Session>();

        public delegate void ProcessClientDelegate(Session session);

        public TextServerAsync(IPAddress IP, int port) : base(IP, port)
        {
            ///Wczytanie bazy danych z pliku
            string data = File.ReadAllText("login.txt");
            string[] separator = { " ", "\n", "\r", "\t" };
            string[] tab = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tab.Length; i += 3)
            {
                User user = new User(tab[i], tab[i + 1], tab[i + 2]);
                database.Add(tab[i], user);
            }

            commands.Add("login", new LoginCommand(this));
            commands.Add("strong", new StrongCommand(this));
            commands.Add("history", new LoginHistoryCommand(this));
            commands.Add("register", new RegisterCommand(this));
            commands.Add("help", new HelpCommand(this));
            commands.Add("support", new SupportCommand(this));
            commands.Add("logout", new LogoutCommand(this));
            commands.Add("chpwd", new ChangePasswordCommand(this));
            commands.Add("delete", new DeleteUserCommand(this));
            commands.Add("activeusers", new ShowActiveUsersCommand(this));
            commands.Add("chperm", new ChangePermissionsCommand(this));
            commands.Add("msg", new MessageCommand(this));

            Console.WriteLine("Started");
        }

        private void ProcessClient(Session session)
        {
            byte[] buffer = new byte[BufferSize];

            string stringBuffer = "";

            for (; ; )
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
                        string[] trimmedArgs = new string[args.Length - 1];
                        Array.Copy(args, 1, trimmedArgs, 0, trimmedArgs.Length);

                        commands[args[0]].execute(trimmedArgs, session);
                    }
                    else if (cmd.Equals("KA")) {
                        
                    }
                    else
                    {
                        session.SendMessage("Invalid command. Type support for more information");
                    }
                }
            }
        }

        public override void Start()
        {
            TcpListener = new TcpListener(IPAddress, Port);
            TcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = TcpListener.AcceptTcpClient();
                tcpClient.ReceiveTimeout = 10000;

                Session session = new Session(this, tcpClient);

                Task.Run(() =>
                {
                    try
                    {
                        this.ProcessClient(session);
                    }
                    catch (SystemException) {
                        lock (activeUsers)
                        {
                            if (session.isLoggedIn())
                            {
                                activeUsers.Remove(session.Login);
                                lock (session)
                                {
                                    session.notifyLogout();
                                }
                            }
                        }
                    }
                });
            }
        }

        public Dictionary<string, User> Database { get => database; }
        public Dictionary<string, Command> Commands { get => commands; }
        public Dictionary<string, Session> ActiveUsers { get => activeUsers; }

    }
}

