using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
//Data Source=.;Initial Catalog=UsRegDB;Integrated Security=True
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string connectionThread = @"Data Source=.;Initial Catalog=UsRegDB;Integrated Security=True;";// connects to database 
        public Form1()
        {
            InitializeComponent();
            CaptchaGenerator();
            button3.Hide();
            label4.Hide();
            label5.Hide();
            pictureBox1.Hide();
            button1.Hide();
            textBox3.Hide();
            button2.Hide();
            //button6.Hide();
        }
        int number = 0;
        /// <summary>
        /// Generates captcha
        /// </summary>
        private void CaptchaGenerator()
        {
            Random captchaImage = new Random();
             number = captchaImage.Next(10000, 100000);
            var image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            var font = new Font("TimesNewRoman", 25, FontStyle.Strikeout, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(image);
            graphics.DrawString(number.ToString(), font, Brushes.Green, new Point(0, 0));
            pictureBox1.Image = image;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CaptchaGenerator();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox3.Text == number.ToString())
            {
               // MessageBox.Show("Match Text with Captcha");

                button3.Show();
                textBox3.Hide();
                label4.Show();
                label5.Hide();
                button2.Hide();
                button1.Hide();
            }
            else
            {
                // MessageBox.Show("Does not Match Text with Captcha");
                
                label5.Show();


            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
        /// <summary>
        ///  connects to databse with button click event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionThread))
            {

                sqlCon.Open();
                string newCommand="select UserName from Table_UsREG where UserName='"+ textBox1.Text+"' and Password='"+ textBox2.Text+"'";
                SqlDataAdapter adp = new SqlDataAdapter(newCommand, sqlCon);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count >= 1)
                {
                    pictureBox1.Show();
                    button1.Show();
                    textBox3.Show();
                    button2.Show();
                }
                else
                {
                    MessageBox.Show("Invalid  login");
                }
            }
            
        }
    }
}
