using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    public class User {
        private string login;

        private string password;

        private string permission;

        public User(string login, string password, string permission)
        {
            this.login = login;
            this.password = password;
            this.permission = permission;
        }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public string Permission { get => permission; set => permission = value; }
    }
}
