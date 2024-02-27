using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class Platform
    {
        private int id;
        private string name;
        private int brandId;

        public Platform() { }
        public Platform(int id, string name, int brandId) 
        {
            this.id = id;
            this.name = name;
            this.brandId = brandId;
        }

        public int getId() { return id; }
        public string getName() { return  name; }

        public override string ToString()
        {
            return name;
        }
    }
}
