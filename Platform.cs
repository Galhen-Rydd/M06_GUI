using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class Platform
    {
        public int id { get; set; }
        public string name { get; set; }
        public int brandId { get; set; }

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
