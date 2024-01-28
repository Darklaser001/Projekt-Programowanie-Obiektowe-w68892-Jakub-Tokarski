using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;


namespace Projekt_Programowanie_Obiektowe_w68892_Jakub_Tokarski
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            runQuery(textBox1.Text, 0);
        }

        private void runQuery(string query, int todo)
        {
            if (query == "")
            {
                MessageBox.Show("Proszę wpisać zapytanie!");
                return;
            }

            string MySQLConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=baza_fitness";

            MySqlConnection databaseConnection = new MySqlConnection(MySQLConnectionString);

            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            if (todo == 0)
            {
                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();

                    if (myReader.HasRows)
                    {

                        MessageBox.Show("Twoje zapytanie wygenerowało listę.");


                        DataTable dt = new DataTable();
                        dt.Load(myReader);
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Zapytanie wykonane sukcesywnie!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd zapytania: " + ex.Message);

                }
            }

            if (todo == 1)
            {
                try
                {
                    databaseConnection.Open();

                    MySqlDataReader myReader = commandDatabase.ExecuteReader();
                    
                    string exportPath = @"C:\Users\Jakub\Desktop\";
                    string exportCsv = "Export_dane.csv";
                    StreamWriter csvFile = null;

                    csvFile = new StreamWriter(@exportPath + exportCsv);

                    for (int i = 0; i< myReader.FieldCount; i++)
                    {
                        csvFile.Write(String.Format("\"{0}\",",myReader.GetName(i)));
                    }
                    csvFile.Write(String.Format("\"{0}\" \n", myReader.GetName(myReader.FieldCount-1)));

                    while (myReader.Read())
                    {
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            csvFile.Write(String.Format("\"{0}\",", myReader[i]));
                        }
                        csvFile.Write(String.Format("\"{0}\" \n", myReader[myReader.FieldCount - 1]));
                    }

                    csvFile.Close();
                    MessageBox.Show("Plik eksportowany pomyślnie!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd zapytania lub zapisu pliku: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.AllowDrop = true;
            richTextBox1.DragDrop += RichTextBox1_DragDrop;
        }

        private void RichTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if(data!=null)
            {
                var fileNames = data as string[];
                if(fileNames.Length>0)
                {
                    richTextBox1.Text = "";
                    richTextBox1.Text = fileNames[0] + "\n";
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileNames[0]);
                    richTextBox1.Text += sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("lol");
            richTextBox1.Text = richTextBox1.Text.Replace("\"", "'");
            string[] csv = richTextBox1.Text.Split('\n');
            string values = csv[1].Replace("'", "`");
            string[] table = csv[0].Split('\\');
            table = table[table.Length-1].Split('.');

            string query = "insert into `" + table[0] + "`(" + values + ") values \n";

            for (int i = 2; i < csv.Length - 2; i++)
            {
                query += "(" + csv[i] + "), \n";
            }
            richTextBox1.Text = "";
            runQuery(query + "(" + csv[csv.Length - 2] + ");", 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            runQuery(textBox1.Text, 1);
        }
    }
}
