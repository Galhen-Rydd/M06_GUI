using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Datatypes.Scalar.Types;

namespace ProjectGUI
{
    internal class Game
    {
        public string key { get; set; }
        public string title { get; set; }
        public Platform platform { get; set; }
        public List<Genre> genres { get;  set; }
        public Company company { get; set; }
        public string description { get; set; }

        public Game() { }
        public Game(string key, string title, Platform plat, List<Genre> genres, Company comp, string desc) 
        {
            this.key = key;
            this.title = title;
            this.platform = plat;
            this.genres = genres;
            this.company = comp;
            this.description = desc;
        }

        public string GetGenres()
        {
            string genres = "";
            foreach (var item in this.genres)
            {
                genres += item.ToString() + ", ";
            }
            genres = genres.Remove(genres.Length - 2);
            return genres;
        }

        public override string ToString()
        {
            string genres = "";
            foreach (var item in this.genres)
            {
                genres += item.ToString() + ", ";
            }
            genres = genres.Remove(genres.Length - 2);
            return key + ": " + title + ": " + genres + ";";
        }
    }
}
