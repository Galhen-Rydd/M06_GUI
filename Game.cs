using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class Game
    {
        private string key;
        private string title;
        private Platform platform;
        private ArrayList genres = new ArrayList();
        private Company company;
        private string description;

        public Game() { }
        public Game(string key, string title, Platform plat, ArrayList genres, Company comp, string desc) 
        {
            this.key = key;
            this.title = title;
            this.platform = plat;
            this.genres = genres;
            this.company = comp;
            this.description = desc;
        }

        public override string ToString()
        {
            return key + ": " + title + ";";
        }
    }
}
