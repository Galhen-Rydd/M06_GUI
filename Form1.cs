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
        private List<Game> ListOfGames = new List<Game>();
        private List<Platform> platforms = new List<Platform>();
        private List<Company> companies = new List<Company>();
        private Dictionary<int, Company> ListOfCompanies = new Dictionary<int, Company>();
        private Dictionary<int, Platform> ListOfPlatforms = new Dictionary<int, Platform>();
        private List<User> users = new List<User>();
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            //Genre g = (Genre)Enum.Parse(typeof(Genre), "RPG");
            //Console.WriteLine(g);
            LoadSQL();
            dataGridView1.DataSource = ListOfGames;
            dataGridView1.Columns.Add("Genre", "Genre");
            //dataGridView1.Columns["key"].Visible = false;
            btnSendData.Enabled = false;
            btnLogOut.Enabled = false;
            PopulateGenreColumn();
        }

        private void PopulateGenreColumn()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Game game = (Game)row.DataBoundItem; // Get the corresponding Game object
                row.Cells["Genre"].Value = game.GetGenres(); // Populate the genre column
            }
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
                MySqlCommand cmd;
                MySqlDataReader reader;

                // Get Users
                cmd = new MySqlCommand("select * from user order by ID ASC", con);
                reader = cmd.ExecuteReader();
                while (reader.Read() != false)
                {
                    users.Add(new User(reader.GetString(1), reader.GetString(2)));
                }
                reader.Close();
                cmd.Dispose();
                
                // Get Companies
                cmd = new MySqlCommand("select * from company order by ID ASC", con);
                reader = cmd.ExecuteReader();
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
                cmd = new MySqlCommand("select * from game order by GameKey ASC", con);
                reader = cmd.ExecuteReader();
                List<Genre> listGenres;

                while(reader.Read() != false) {
                    listGenres = new List<Genre>();
                    string[] genres = reader.GetString(3).Split(';');
                    foreach(string s in genres)
                    {
                        //Console.WriteLine(s);
                        try
                        {
                            if(s == "")
                            {
                                continue;
                            }
                            string genreString = s.Trim();
                            Genre g = (Genre)Enum.Parse(typeof(Genre), genreString);
                            //Console.WriteLine(g.ToString());
                            listGenres.Add(g);
                        }
                        catch (Exception ex) {
                            Console.WriteLine(s);
                            Console.WriteLine("Unrecognized Genre..."); 
                        }
                    }
                    ListOfGames.Add(new Game(reader.GetString(0), reader.GetString(1), ListOfPlatforms[reader.GetInt32(2)],
                        listGenres, ListOfCompanies[reader.GetInt32(5)], reader.GetString(4)));
                }
                reader.Close();
                cmd.Dispose();

                /*foreach(Game g in ListOfGames) 
                {
                    Console.WriteLine(g.ToString());
                }*/
                //Extraiem les dades del reader
                //lbValors.Text = reader.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                ListOfGames = new List<Game>();
                MySqlCommand cmd = new MySqlCommand("select * from game order by GameKey ASC", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                List<Genre> listGenres;

                while (reader.Read() != false)
                {
                    listGenres = new List<Genre>();
                    string[] genres = reader.GetString(3).Split(';');
                    foreach (string s in genres)
                    {
                        try
                        {
                            if (s == "")
                            {
                                continue;
                            }
                            string genreString = s.Trim();
                            Genre g = (Genre)Enum.Parse(typeof(Genre), genreString);
                            listGenres.Add(g);
                        }
                        catch
                        {
                            Console.WriteLine(s);
                            Console.WriteLine("Unrecognized Genre...");
                        }
                    }
                    ListOfGames.Add(new Game(reader.GetString(0), reader.GetString(1), ListOfPlatforms[reader.GetInt32(2)],
                        listGenres, ListOfCompanies[reader.GetInt32(5)], reader.GetString(4)));
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
                dataGridView1.DataSource = ListOfGames;
                dataGridView1.Columns.Add("Genre", "Genre");
                //dataGridView1.Columns["key"].Visible = false;
                PopulateGenreColumn();
            }
        }

        private void btnPlatforms_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from platform order by ID ASC", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                ListOfPlatforms = new Dictionary<int, Platform>();
                while (reader.Read() != false)
                {
                    ListOfPlatforms.Add(reader.GetInt32(0), new Platform(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                }
                platforms = ListOfPlatforms.Values.ToList();
                dataGridView1.DataSource = platforms;
                if (dataGridView1.Columns.Contains("Genre"))
                {
                    dataGridView1.Columns.Remove("Genre");
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from company order by ID ASC", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                ListOfCompanies = new Dictionary<int, Company>();
                while (reader.Read() != false)
                {
                    ListOfCompanies.Add(reader.GetInt32(0), new Company(reader.GetInt32(0), reader.GetString(1)));
                }
                companies = ListOfCompanies.Values.ToList();
                dataGridView1.DataSource = companies;
                if(dataGridView1.Columns.Contains("Genre"))
                {
                    dataGridView1.Columns.Remove("Genre");
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = checkedListBox1.SelectedIndex;
            int count = checkedListBox1.Items.Count;
            for(int i = 0; i < count; i++)
            {
                if(index != i)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = checkedListBox2.SelectedIndex;
            int count = checkedListBox2.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (index != i)
                {
                    checkedListBox2.SetItemCheckState(i, CheckState.Unchecked);
                }
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            string mysqlquery = "";
            string table;
            string action;
            
            switch (checkedListBox1.SelectedIndex)
            {
                case 0:
                    table = "game";
                    break;
                case 1:
                    table = "platform";
                    break;
                case 2:
                    table = "company";
                    break;
                default:
                    return;
            }
            switch (checkedListBox2.SelectedIndex)
            {
                case 0:
                    action = "insert into";
                    mysqlquery = action + " " + table + " values ()";
                    break;
                case 1:
                    action = "delete from";
                    mysqlquery = action + " " + table + " where ";
                    break;
                case 2:
                    action = "update";
                    mysqlquery = action + " " + table + " set "; 
                    break;
                default:
                    return;
            }
            using (MySqlConnection con = new MySqlConnection("server=localhost;" +
                                                                "user=root;" +
                                                                "database=dam_m06;" +
                                                                "port=3306;" +
                                                                "password=1234"))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(mysqlquery, con);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("SQL error...");
                }
                con.Close();
            }
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            User user = new User(tbxUser.Text, tbxPass.Text);
            foreach(var u in users)
            {
                if (user.Equals(u))
                {
                    lblLog.Text = "Loged In!";
                    btnSendData.Enabled = true;
                    btnLogOut.Enabled = true;
                    return;
                }
            }
            lblLog.Text = "Invalid Credentials...";
            btnSendData.Enabled = false;
            btnLogOut.Enabled = false;
            return;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if(btnSendData.Enabled == true)
            {
                lblLog.Text = "Loged Out, see you!";
                btnSendData.Enabled = false;
                btnLogOut.Enabled = false;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }
    }
}
