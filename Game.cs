using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
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

        public string GetGenresSQL()
        {
            try
            {
                string gen = "";
                foreach (var item in this.genres)
                {
                    gen += item.ToString() + ";";
                }
                return gen;
            } catch { return ""; }

        }

        public string GetGenres()
        {
            string gen = "";
            try
            {
                foreach (var item in this.genres)
                {
                    gen += item.ToString() + ", ";
                }
                gen = gen.Remove(gen.Length - 2);
                return gen;
            } catch { return ""; }
        }

        public bool Equals(Game other)
        {
            if(other == null) return false;
            else if(other == this) return true;
            else if(other.key == this.key) return true;
            return false;
        }

        public static bool InsertGame(Game game)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO game ( GameKey, Title, Platform, Genre, Description, BrandID ) VALUES ( @GameKey, @Title, @PlatformID, @Genre, @Description, @BrandID )",
                    con);
                Console.WriteLine(game.GetGenresSQL());
                cmd.Parameters.AddWithValue("@GameKey", game.key);
                cmd.Parameters.AddWithValue("@Title", game.title);
                cmd.Parameters.AddWithValue("@PlatformID", game.platform.id);
                cmd.Parameters.AddWithValue("@Genre", game.GetGenresSQL());
                cmd.Parameters.AddWithValue("@Description", game.description);
                cmd.Parameters.AddWithValue("@BrandID", game.company.id);
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("SQL error...");
                    return false;
                }
                con.Close();
                return true;
            }
        }

        public bool DeleteGame()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM game WHERE GameKey like @GameKey",
                    con);
                cmd.Parameters.AddWithValue("@GameKey", this.key);
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("SQL error...");
                    return false;
                }
                con.Close();
                return true;
            }
        }

        public bool UpdateGame()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"UPDATE game SET Title = @Title, Platform = @PlatformID, Genre = @Genre, Description = @Description, BrandID = @BrandID" +
                    $" WHERE GameKey like @GameKey", con);
                cmd.Parameters.AddWithValue("@GameKey", this.key);
                cmd.Parameters.AddWithValue("@Title", this.title);
                cmd.Parameters.AddWithValue("@PlatformID", this.platform.id);
                cmd.Parameters.AddWithValue("@Genre", this.GetGenresSQL());
                cmd.Parameters.AddWithValue("@Description", this.description);
                cmd.Parameters.AddWithValue("@BrandID", this.company.id);
                try
                {
                    int rows = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("SQL error...");
                    return false;
                }
                con.Close();
                return true;
            }
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
