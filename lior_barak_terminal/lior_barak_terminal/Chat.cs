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

namespace lior_barak_terminal
{
    delegate void myDelegate(string rx_string);

    public partial class Chat : Form
    {
        int char_count = 0;
        int char_count2 = 0;
        int flagl=1;

        public Chat(int parity, int baudrate, int databits, int stopbits, string port, int timer_interval)
        {
            InitializeComponent();

            timer1.Interval = timer_interval;   //Sets interval for the TICK event (baudrate)

            //Port configuration
            serialPort1.PortName = port;
            serialPort1.BaudRate = baudrate;
            serialPort1.DataBits = databits;

            switch (parity)
            {
                case 0:     //0=None parity
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    break;
                case 1:     //1=Odd parity
                    serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case 2:     //2=Even parity
                    serialPort1.Parity = System.IO.Ports.Parity.Even;
                    break;
                default:
                    break;
            }

            switch (stopbits)
            {
                case 1:     //0 = one stop bit
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 2:     //1 = two stop bits
                    serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }
            //End configuration

            //Open port (2 tries)
            try
            {
                serialPort1.Open();
            }
            catch (IOException)
            {
                try
                {
                    serialPort1.Open();
                }
                catch (IOException)
                {
                    MessageBox.Show("Failed to open connection");
                    this.Close();
                    return;
                }
            }
            serialPort1.Write(BitConverter.GetBytes(14), 0, 1);     //End_msg mode (0x0E) on MSP430 program

        }

        //Check if Enter was pressed and start timer when it happens
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) //Enter was pressed
            //{
                serialPort1.Write(BitConverter.GetBytes(5), 0, 1); //Entering chat mode
                Thread.Sleep(50); // 50 ms delay
                timer1.Start(); //start sending short msg
            //}
            
        }

        //Event for each tick (immitates the baudrate)
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (textBox1.Text.Length > char_count)  //Write data on MSP430 char by char till the string ends
            //{
                flagl = 0; 
                char[] char_to_send = new char[1];
                char_to_send[0] = textBox1.Text[char_count++];
                serialPort1.Write(char_to_send, 0, 1);
                Thread.Sleep(100); // 1 ms delay
                timer1.Stop();
                timer1.Enabled = false;
                if (char_count % 15 == 0)
                {
                    serialPort1.Write(BitConverter.GetBytes(16), 0, 1);
                }
           
        }

        //When data received write it on the 2nd textbox and discared serialport1 buffer
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            char[] rx_data = { '0' };
            string rx_string;
            serialPort1.Read(rx_data, 0, 1);    //Read 1 byte from the MSP430 to rx_data
            serialPort1.DiscardInBuffer();
            rx_string = new string(rx_data);    //Convert byte to string

            /* While we want to write the recieved data to the textbox, the textbox is operating on
             * different thread than the serial port, then the textbox will not know the data. 
             * In this case we invoke the printing method to operate in the correct thread
             */
            Invoke(new myDelegate(PrintData), rx_string);

        }

        private void PrintData(string rx_string)
        {
            if (flagl == 1)
            {
                textBox4.AppendText(rx_string);
                char_count2++;
                if (char_count2%28==0) serialPort1.Write(BitConverter.GetBytes(17), 0, 1);
            }
            else
            {
                textBox2.AppendText(rx_string);
                flagl = 1;
            }
            //textBox1.Clear();       //Clear textbox1
            //textBox1.Select(0, 0);   //Return to the beggining of textbox1 for the next message
        }

        //If there's an error while receiving data from MSP
        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Error Received");
        }

        //Close serial port if the window get closed
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //  serialPort1.Open();
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
            }
            catch (IOException)
            {
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                serialPort1.Write(BitConverter.GetBytes(23), 0, 1);     //reset lcd mode (0x17) on MSP430 program
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
