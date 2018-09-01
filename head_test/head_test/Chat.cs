using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Threading;

namespace head_test
{
    delegate void myDelegate(string rx_string);

    public partial class Chat : Form
    {
        public LidarClass mLidar = null;
        int char_count = 0;
        int char_count2 = 0;
        int flagl=1;

        public Chat(string port, int baudrate)
        {
            InitializeComponent();
            mLidar = new head_test.LidarClass();

            Application.ApplicationExit += Application_ApplicationExit;

             mLidar.Start(port,baudrate);
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            mLidar.Stop();
        }
        //Check if Enter was pressed and start timer when it happens
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) //Enter was pressed
            //{
            //   serialPort1.Write(BitConverter.GetBytes(5), 0, 1); //Entering chat mode
            Thread.Sleep(50); // 50 ms delay
            char[] char_to_send = new char[1];
            char_to_send[0] = textBox1.Text[char_count++];
           // Invoke(new myDelegate(PrintData), rx_string);
         //   textBox2.AppendText(rx_string);
          //  textBox4.AppendText(rx_string);
            char_count2++;
            //rx_string = new string(rx_data);    //Convert byte to string
            //  timer1.Start(); //start sending short msg
            //}

        }
            /* While we want to write the recieved data to the textbox, the textbox is operating on
             * different thread than the serial port, then the textbox will not know the data. 
             * In this case we invoke the printing method to operate in the correct thread
             */
 

     
        //Close serial port if the window get closed
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //  serialPort1.Open();
             //   serialPort1.DiscardInBuffer();
             //   serialPort1.Close();
            }
            catch (IOException)
            {
              //  serialPort1.DiscardInBuffer();
             //   serialPort1.Close();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void clean_Click(object sender, EventArgs e)
        {
            textBox2.Clear();       //Clear textbox2
        }

        private void back_button(object sender, EventArgs e)
        {
            try
            {
             //   serialPort1.Write(BitConverter.GetBytes(23), 0, 1);     //reset lcd mode (0x17) on MSP430 program
                var main_form = (MainMenu)Tag;
                main_form.Show();
                Close();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pc_textBox(object sender, EventArgs e)
        {

        }

        private void msp_textbox(object sender, EventArgs e)
        {

        }

        private void Chat_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3(object sender, EventArgs e)
        {

        }
    }
}
