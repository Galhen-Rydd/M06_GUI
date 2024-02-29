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
using MySqlX.XDevAPI.Relational;

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
            lblTable.Text = "Table: Games";
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

        private void UpdateGames()
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
                //dataGridView1.Columns["key"].Visible = false;
            }
            dataGridView1.DataSource = ListOfGames;
            if (!dataGridView1.Columns.Contains("Genre"))
            {
                dataGridView1.Columns.Add("Genre", "Genre");
            }
            PopulateGenreColumn();
            lblTable.Text = "Table: Games";
        }

        private void UpdatePlatforms()
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
            lblTable.Text = "Table: Platforms";
        }

        private void UpdateCompanies()
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
                if (dataGridView1.Columns.Contains("Genre"))
                {
                    dataGridView1.Columns.Remove("Genre");
                }
                reader.Close();
                cmd.Dispose();
                con.Close();
            }
            lblTable.Text = "Table: Companies";
        }

        private void btnGames_Click(object sender, EventArgs e)
        {
            UpdateGames();
        }

        private void btnPlatforms_Click(object sender, EventArgs e)
        {
            UpdatePlatforms();
        }

        private void btnCompany_Click(object sender, EventArgs e)
        {
            UpdateCompanies();
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
                    checkedListBox1.SetItemCheckState(index, CheckState.Checked);
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
                    checkedListBox2.SetItemCheckState(index, CheckState.Checked);
                }
            }
        }

        private void btnSendData_Click(object sender, EventArgs e)
        {
            string mysqlquery = "";
            string table;
            string action;
            string identification;
            string insertText = "";
            
            switch (checkedListBox2.SelectedIndex)
            {
                case 0:
                    //action = "insert into";
                    switch (checkedListBox1.SelectedIndex)
                    {
                        case 0:
                            List<Genre> gen = GetGenresFromInput();
                            try
                            {
                                Game g1 = new Game(tbxKey.Text, tbxTitle.Text, ListOfPlatforms[Int32.Parse(tbxPlatformID.Text)], 
                                    gen, ListOfCompanies[Int32.Parse(tbxCompanyID.Text)], tbxDescription.Text);
                                if (Game.InsertGame(g1))
                                {
                                    lblResultQuery.Text = "Game Inserted";
                                    UpdateGames();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Insert Game";
                                }
                            } catch { lblResultQuery.Text = "Failed to Insert Game"; }
                            
                            break;
                        case 1:
                            try
                            {
                                Platform p1 = new Platform(0, tbxPlatform.Text, Int32.Parse(tbxCompanyID.Text));
                                if (Platform.InsertPlatform(p1))
                                {
                                    lblResultQuery.Text = "Platform Inserted";
                                    UpdatePlatforms();
                                    }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Insert the Platform";
                                }
                            } catch { lblResultQuery.Text = "Failed to Insert the Platform"; }
                            break;
                        case 2:
                            try
                            {
                                Company c1 = new Company(0, tbxCompany.Text);
                                if (Company.InsertCompany(c1))
                                {
                                    lblResultQuery.Text = "Company Inserted";
                                    UpdateCompanies();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Insert the Company";
                                }
                            } catch { lblResultQuery.Text = "Failed to Insert the Company"; }
                            break;
                        default:
                            return;
                    }
                    break;
                case 1:
                    //action = "delete from";
                    switch (checkedListBox1.SelectedIndex)
                    {
                        case 0:
                            List<Genre> gen = GetGenresFromInput();
                            try
                            {
                                Game g1 = new Game(tbxKey.Text, tbxTitle.Text, ListOfPlatforms[Int32.Parse(tbxPlatformID.Text)],
                                gen, ListOfCompanies[Int32.Parse(tbxCompanyID.Text)], tbxDescription.Text);
                                if (g1.DeleteGame())
                                {
                                    lblResultQuery.Text = "Game with key " + g1.key + " deleted";
                                    UpdateGames();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Delete Game";
                                }
                            } catch { lblResultQuery.Text = "Failed to Delete Game"; }                            
                            break;
                        case 1:
                            try
                            {
                                Platform p1 = new Platform(Int32.Parse(tbxPlatformID.Text), tbxPlatform.Text, Int32.Parse(tbxCompanyID.Text));
                                if (p1.DeletePlatform())
                                {
                                    lblResultQuery.Text = "Platform with ID " + p1.id + " deleted";
                                    UpdatePlatforms();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Delete Platform";
                                }
                            } catch { lblResultQuery.Text = "Failed to Delete Platform"; }
                            break;
                        case 2:
                            try
                            {
                                Company c1 = new Company(Int32.Parse(tbxCompanyID.Text), tbxCompany.Text);
                                if (c1.DeleteCompany())
                                {
                                    lblResultQuery.Text = "Company with ID " + c1.id + " deleted";
                                    UpdateCompanies();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Delete Company";
                                }
                            } catch { lblResultQuery.Text = "Failed to Delete Company"; }
                            break;
                        default:
                            return;
                    }
                    break;
                case 2:
                    //action = "update";
                    switch (checkedListBox1.SelectedIndex)
                    {
                        case 0:
                            List<Genre> gen = GetGenresFromInput();
                            try
                            {
                                Game g1 = new Game(tbxKey.Text, tbxTitle.Text, ListOfPlatforms[Int32.Parse(tbxPlatformID.Text)],
                                gen, ListOfCompanies[Int32.Parse(tbxCompanyID.Text)], tbxDescription.Text);
                                if (g1.UpdateGame())
                                {
                                    lblResultQuery.Text = "Game with key " + g1.key + " updated";
                                    UpdateGames();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Update Game";
                                }
                            } catch { lblResultQuery.Text = "Failed to Update Game"; }
                            break;
                        case 1:
                            try
                            {
                                Platform p1 = new Platform(Int32.Parse(tbxPlatformID.Text), tbxPlatform.Text, Int32.Parse(tbxCompanyID.Text));
                                if (p1.UpdatePlatform())
                                {
                                    lblResultQuery.Text = "Platform with ID " + p1.id + " updated";
                                    UpdatePlatforms();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Update Platform";
                                }
                            } catch { lblResultQuery.Text = "Failed to Update Platform"; }
                            break;
                        case 2:
                            try
                            {
                                Company c1 = new Company(Int32.Parse(tbxCompanyID.Text), tbxCompany.Text);
                                if (c1.UpdateCompany())
                                {
                                    lblResultQuery.Text = "Company with ID " + c1.id + " updated";
                                    UpdateCompanies();
                                }
                                else
                                {
                                    lblResultQuery.Text = "Failed to Update Company";
                                }
                            } catch { lblResultQuery.Text = "Failed to Update Company"; }
                            break;
                        default:
                            return;
                    }
                    break;
                default:
                    return;
            }
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int id;
            switch (checkedListBox1.SelectedIndex)
            {
                case 0:
                    List<Game> searchGames = new List<Game>();
                    foreach(var g in ListOfGames)
                    {
                        if (g.key.ToLower().Contains(tbxKey.Text.ToLower()))
                        {
                            Console.WriteLine(tbxKey.Text);
                            searchGames.Add(g);
                        }
                    }
                    dataGridView1.DataSource = searchGames;
                    if (!dataGridView1.Columns.Contains("Genre"))
                    {
                        dataGridView1.Columns.Add("Genre", "Genre");
                    }
                    PopulateGenreColumn();
                    lblResultQuery.Text = "Displaying found games";
                    if(searchGames.Count == 1)
                    {
                        Game game = searchGames[0];
                        tbxKey.Text = game.key;
                        tbxTitle.Text = game.title;
                        tbxPlatform.Text = game.platform.name;
                        tbxGenres.Text = game.GetGenres();
                        tbxDescription.Text = game.description;
                        tbxCompanyID.Text = game.company.id.ToString();
                        tbxPlatformID.Text = game.platform.id.ToString();
                        tbxCompany.Text = game.company.name;
                    }
                    break;
                case 1:
                    List<Platform> searchPlatform = new List<Platform>();
                    
                    Int32.TryParse(tbxPlatformID.Text, out id);
                    if (ListOfPlatforms.ContainsKey(id))
                    {
                        searchPlatform.Add(ListOfPlatforms[id]);
                    }
                    dataGridView1.DataSource = searchPlatform;
                    if (dataGridView1.Columns.Contains("Genre"))
                    {
                        dataGridView1.Columns.Remove("Genre");
                    }
                    if(searchPlatform.Count == 1)
                    {
                        Platform platform = searchPlatform[0];
                        tbxPlatformID.Text = platform.id.ToString();
                        tbxPlatform.Text = platform.name;
                        tbxCompanyID.Text = platform.brandId.ToString();
                        tbxCompany.Text = ListOfCompanies[platform.brandId].name;
                    }
                    break;
                case 2:
                    List<Company> searchCompany = new List<Company>();
                    Int32.TryParse(tbxCompanyID.Text, out id);
                    if (ListOfCompanies.ContainsKey(id))
                    {
                        searchCompany.Add(ListOfCompanies[id]);
                    }
                    dataGridView1.DataSource = searchCompany;
                    if (dataGridView1.Columns.Contains("Genre"))
                    {
                        dataGridView1.Columns.Remove("Genre");
                    }
                    if(searchCompany.Count == 1)
                    {
                        Company company = searchCompany[0];
                        tbxCompanyID.Text = company.id.ToString();
                        tbxCompany.Text = company.name;
                    }
                    break;
                default:
                    lblResultQuery.Text = "Select Game, Platform or Company";
                    return;
            }
        }

        private List<Genre> GetGenresFromInput()
        {
            List<Genre> gen = new List<Genre>();
            string[] arrayGenres = tbxGenres.Text.Split(',');
            foreach (var s in arrayGenres)
            {
                try
                {
                    if (s == "")
                    {
                        continue;
                    }
                    string genreString = s.Trim();
                    Genre g = (Genre)Enum.Parse(typeof(Genre), genreString);
                    gen.Add(g);
                }
                catch
                {
                    //Console.WriteLine(s);
                    Console.WriteLine("Unrecognized Genre...");
                }
            }
            return gen;
        }
    }
}
