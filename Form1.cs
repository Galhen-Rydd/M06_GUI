using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace ProjectGUI
{
    public partial class Form1 : Form
    {
        private ArrayList ListOfGames = new ArrayList();
        private Dictionary<int, Company> ListOfCompanies = new Dictionary<int, Company>();
        private Dictionary<int, Platform> ListOfPlatforms = new Dictionary<int, Platform>();
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Genre g = (Genre)Enum.Parse(typeof(Genre), "RPG");
            Console.WriteLine(g);
            LoadSQL();
        }

        private void LoadSQL()
        {
            
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                
                //MessageBox.Show(Convert.ToString(con.State));
                con.Open();
                //MessageBox.Show(Convert.ToString(con.State));
                // Get Companies
                MySqlCommand cmd = new MySqlCommand("select * from company order by ID ASC", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read() != false)
                {
                    ListOfCompanies.Add(reader.GetInt32(0), new Company(reader.GetInt32(0), reader.GetString(1)));
                }

                reader.Close();
                cmd.Dispose();

                // Get Platforms
                cmd = new MySqlCommand("select * from platform order by ID ASC", con);
                reader = cmd.ExecuteReader();
                while (reader.Read() != false)
                {
                    ListOfPlatforms.Add(reader.GetInt32(0), new Platform(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                }

                reader.Close();
                cmd.Dispose();

                // Get Games
                cmd = new MySqlCommand("select * from games order by GameKey ASC", con);
                reader = cmd.ExecuteReader();
                ArrayList arrayGenres;

                while(reader.Read() != false) {
                    arrayGenres = new ArrayList();
                    string[] genres = reader.GetString(3).Split(';');
                    foreach(string s in genres)
                    {
                        try
                        {
                            Genre g = (Genre)Enum.Parse(typeof(Genre), "RPG");
                            arrayGenres.Add(g);
                        }
                        catch (Exception ex) { Console.WriteLine("Unrecognized Genre..."); }
                    }
                    ListOfGames.Add(new Game(reader.GetString(0), reader.GetString(1), ListOfPlatforms[reader.GetInt32(2)], 
                        arrayGenres, ListOfCompanies[reader.GetInt32(5)], reader.GetString(4)));
                }
                
                reader.Close();
                cmd.Dispose();

                foreach(Game g in ListOfGames) 
                {
                    Console.WriteLine(g.ToString());
                }
                //Extraiem les dades del reader
                //lbValors.Text = reader.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

    }
}
