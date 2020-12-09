using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Klasa odpowiedzialna za wyswietlenie historii logowania
/// </summary>
namespace ServerLib
{
    public class LoginHistoryCommand : Command
    {
        public LoginHistoryCommand(TextServerAsync server) : base(server)
        {
        }

        public override void execute(string[] args, Session session)
        {
            if (args.Length > 0)
            {
                //session.SendMessage("Incorrect data");
                session.SendMessage("Usage history");
                return;
            }

            //string data = File.ReadAllText("historia.txt");

            try
            {

                using (var sr = new StreamReader("historia.txt"))
                {



                    //string data = (sr, Encoding.Unicode);
                    // string[] separator = { " ", "\n", "\r", "\t" };
                    // string[] tab = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    // session.SendMessage(data);
                    foreach (var line in File.ReadLines("historia.txt"))
                    {
                        session.SendMessage(line);
                    }
                }
            }
            catch (IOException e)
            {
                session.SendMessage("The file could not be read");
                File.Create("historia.txt");
                //Console.WriteLine(e.Message);
            }
            //session.SendMessage(data);
        }
    }
}
