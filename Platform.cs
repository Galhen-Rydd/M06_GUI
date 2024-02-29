using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
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

        public static bool InsertPlatform(Platform platform)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO platform ( Name, BrandID ) VALUES ( @Name, @companyId )", con);
                cmd.Parameters.AddWithValue("@Name", platform.name);
                cmd.Parameters.AddWithValue("@companyId", platform.brandId);
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

        public bool DeletePlatform()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM platform WHERE ID like @id", con);
                cmd.Parameters.AddWithValue("@id", this.id);
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

        public bool UpdatePlatform()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"UPDATE platform SET Name = @Name, BrandID = @companyId WHERE ID like @id", con);
                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@Name", this.name);
                cmd.Parameters.AddWithValue("@companyId", this.brandId);
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
            return name;
        }

        public bool Equals(Platform other)
        {
            if (other == null) return false;
            else if (other.id == this.id) return true;
            else if (other.name.ToLower() == this.name.ToLower()) return true;
            return false;
        }
    }
}
