using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public User() { }
        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public bool Equals(User obj)
        {
            if(this.Name.Equals(obj.Name) && this.Password.Equals(obj.Password)) return true;
            return false;
        }

    }
}
