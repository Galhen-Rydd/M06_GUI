﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class Company
    {
        int id;
        string name;

        public Company() { }
        public Company(int id, string name) 
        { 
            this.id = id;
            this.name = name;
        }

        public int getId() { return id; }
        public string getName() { return name; }

        public override string ToString()
        {
            return name;
        }
    }
}
