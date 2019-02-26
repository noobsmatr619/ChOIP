using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        //private string strkey = "Nigel's key 5432";
        //private byte [] key =null;
        // AesCryptoServiceProvider algorithm = null;//The crypto algorithm.  This uses AES.
        Socket socket;
        EndPoint local, remote;
        public Form3()
        {
            InitializeComponent();
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);  // udp so that a secured connection is made without firewall of server as server is not available dgram is used thus userdig protocol is secured moreover udp is faster and this connection  is not a corporate network
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            textBox1.Text = GetLocalIP();
            button1.Enabled = false;
            textBox5.Enabled = false;
        }
        //private string  CyptoMAsk(string mask)
        //{
        //    if (algorithm == null)
        //    {
        //        String stringIV = "random 16 chars ";
        //        algorithm = new AesCryptoServiceProvider();//Creates the default implementation, which is             //
        //                                                   // AES 


        //        int lengthIV = stringIV.Length;
        //        byte[] iv = new byte[lengthIV];
        //        for (int i = 0; i < lengthIV; i++)
        //        {
        //            iv[i] = (byte)stringIV[i];
        //        }
        //        algorithm.IV = iv;//Convert the IV into an array of bytes. 
        //    }
        //    int lengthKey = strkey.Length;
        //    key = new byte[lengthKey];
        //    for (int i = 0; i < lengthKey; i++)
        //        key[i] = (byte)strkey[i];

        //    algorithm.Key = key;//// Convert the key (string) into an array of bytes and      //
        //                        // assign the key to the algorithm.  

        //    int lengthInput = mask.Length;
        //    byte[] bytesInput = new byte[lengthInput];
        //    for (int i = 0; i < lengthInput; i++)
        //    {
        //        bytesInput[i] = (byte)mask[i]; //Convert the text into an array of bytes.  
        //    }
        //    ICryptoTransform encryptor = algorithm.CreateEncryptor();
        //    byte[] bytesOutput = encryptor.TransformFinalBlock(bytesInput, 0, bytesInput.Length);// encryptor
            
        //        string encryptedText;
        //    encryptedText = "";
        //    foreach (byte b in bytesOutput)
            

        //        encryptedText+= (char)b;
            
        //    return encryptedText;
        //}
        //private string CyptoUnMAsk(string unmask)
        //{
        //    if ((unmask.Length % 2) != 0)
        //        Console.WriteLine("Invalid hex string length");


        //    // Take two characters at a time, convert into a byte.         
        //    int length = unmask.Length;
        //    byte[] bytesIn = new byte[length / 2];


        //    int byteIndex = 0;
        //    for (int i = 0; i < length; i++)
        //    {
        //        char msb = (char)listBox1.Text[i];
        //        char lsb = (char)listBox1.Text[++i];
        //        String s = "" + msb + lsb;
        //        int n = Convert.ToInt32(s, 16); // 16 for hex, of course
        //        bytesIn[byteIndex++] = (byte)n;
        //    }


        //    // Do the decrypt.
        //    ICryptoTransform decryptor = algorithm.CreateDecryptor();
        //    byte[] bytesOut = decryptor.TransformFinalBlock(bytesIn, 0, bytesIn.Length);
        //    string decryptText;
        //    // And finally convert the decrypted butes back into text.          
           
        //    decryptText = "";
        //    foreach (byte b in bytesOut)
        //        decryptText += (char)b;
        //    return decryptText;
        //}

        /// <summary>
        /// sends message from one client to another 
        /// </summary>
        /// <param name="sResult"></param>
        private void MessageBack(IAsyncResult sResult)
        {
            try
            {
                int size = socket.EndReceiveFrom(sResult, ref remote);
                if (size > 0)
                {
                    byte[] rData = new byte[1464];
                    rData = (byte[])sResult.AsyncState;
                    ASCIIEncoding encode = new ASCIIEncoding();
                    string rMessage = encode.GetString(rData);
                    listBox1.Items.Add("Agent:" + rMessage);
                }
                byte[] trigger = new byte[1500];
                socket.BeginReceiveFrom(trigger, 0, trigger.Length, SocketFlags.None, ref remote, new AsyncCallback(MessageBack), trigger);

            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
        /// <summary>
        /// gets local ip of the running pc
        /// </summary>
        /// <returns></returns>
        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily==AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
        /// <summary>
        ///  connector method within the button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                local = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text));
                socket.Bind(local);

                remote = new IPEndPoint(IPAddress.Parse(textBox3.Text), Convert.ToInt32(textBox4.Text));
                socket.Connect(remote);

                byte[] trigger = new byte[1500];
                socket.BeginReceiveFrom(trigger, 0, trigger.Length, SocketFlags.None, ref remote, new AsyncCallback(MessageBack), trigger);
                button2.Text = "Connected";
                button2.Enabled = false;
                button1.Enabled = true;
                textBox5.Enabled = true;
                textBox5.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// click event to respond towards message sent or received 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                System.Text.ASCIIEncoding ence = new System.Text.ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = ence.GetBytes(textBox5.Text);

                socket.Send(msg);
                listBox1.Items.Add("Me: "+ textBox5.Text+" ........... "+ DateTime.Now.ToLongTimeString()+" "+DateTime.Now.ToLongDateString());
                textBox5.Clear(); 


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //CyptoUnMAsk();
            this.Hide();
        }
    }
}
