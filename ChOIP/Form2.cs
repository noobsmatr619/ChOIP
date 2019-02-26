using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        string connectionThread= @"Data Source=.;Initial Catalog=UsRegDB;Integrated Security=True;";
        public Form2()
        {
            InitializeComponent();
        }
        /// <summary>
        /// click evernt for registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
                MessageBox.Show("please fill mandatory fields");
            else if (textBox4.Text != textBox5.Text)
                MessageBox.Show("Password do not match");
            else
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionThread))
                {
                    sqlCon.Open();
                    SqlCommand sqlCmd = new SqlCommand("UserAdd", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@FirstName", textBox1.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@LastName", textBox2.Text.Trim());
                    
                    sqlCmd.Parameters.AddWithValue("@Username", textBox3.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Password", textBox4.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Registration is successfull");
                    Clear();

                }
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            //SqlConnection connector = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tituh\OneDrive\Documents\locallogin.mdf;Integrated Security=True;Connect Timeout=30");
            //connector.Open();
            //string sql = "INSERT INTO dbo(First Name,Last Name ,Username,Password,Confirm Password) VALUES ('" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "')";
            //SqlCommand cmd = new SqlCommand(sql, connector);
            //cmd.ExecuteNonQuery();
            //MessageBox.Show("done");

        }
        /// <summary>
        /// method to clear any entity of database
        /// </summary>
        void Clear()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
