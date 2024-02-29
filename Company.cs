using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySqlConnector;
using System.Threading.Tasks;

namespace ProjectGUI
{
    internal class Company
    {
        public int id { get; set; }
        public string name { get; set; }

        public Company() { }
        public Company(int id, string name) 
        { 
            this.id = id;
            this.name = name;
        }

        public static bool InsertCompany(Company company)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO company ( Name ) VALUES ( @companyName )", con);
                cmd.Parameters.AddWithValue("@companyName", company.name);
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

        public bool DeleteCompany()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"DELETE FROM company WHERE ID like @id", con);
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

        public bool UpdateCompany()
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"UPDATE company SET Name = @companyName WHERE ID like @id", con);
                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@companyName", this.name);
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

        public bool Equals(Company other)
        {
            if(other == null) return false;
            else if(other == this) return true;
            else if(other.id == this.id) return true;
            return false;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
