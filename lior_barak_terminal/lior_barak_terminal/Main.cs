using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace lior_barak_terminal
{
    public partial class MainMenu : Form
    {
        bool FirstUp;
        int bintodec = 115;
        int parity = 2, buff_parity = 2; // 0=none , 1=odd , 2=even
        int baudrate = 9600, buff_baudrate = 9600;
        int databits = 7, buff_databits = 7;
        int stopbits = 1, buff_stopbits = 1; // 0=one , 1=two
        int[] SerialPort_config = { 1, 1, 0, 0, 1, 1, 1, 1 }; //Goes from 7 to 0 in the array

        /* SerialPort_config setting: Page 911 in datasheet (UCAxCTL0)
         * bit 7 : parity enable (0=disable , 1=enable)
         * bit 6 : parity type (0=odd , 1=even)
         * bit 5 : LSB/MSB (0=LSB first , 1=MSB first)
         * bit 4 : data bits (0=8bit , 1=7bit)
         * bit 3 : stop bit (0=one , 1=two)
         * bit 2-0: baudrate (0=1200 , 1=2400 , 2=4800 , 3=9600)
         */

        string port = "COM4";
        int timer_interval = 1;
        // constructor
        public MainMenu()
        {
            int i = 0;
            FirstUp = true;
            InitializeComponent();
            serialPort1.Close();
            //Initial comboboxes values
            ParityBox.DropDownStyle = ComboBoxStyle.DropDownList;
            BaudrateBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DataBitsBox.DropDownStyle = ComboBoxStyle.DropDownList;
            StopBitBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PortBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //Initialize and set selected items
            string[] ports = SerialPort.GetPortNames();
            PortBox.Items.AddRange(ports);
            PortBox.SelectedIndex = 0;
            ParityBox.SelectedIndex = 2;
            StopBitBox.SelectedIndex = 0;
            DataBitsBox.SelectedIndex = 0;
            BaudrateBox.SelectedIndex = 0;
            FirstUp = false;
        }
        // communication set
        private void set_com_settings(int mode)
        {
            serialPort1.Close();    //Port can be configged only when closed
            serialPort1.PortName = port;
            serialPort1.BaudRate = buff_baudrate;
            serialPort1.DataBits = buff_databits;
            switch (buff_parity)
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
            switch (buff_stopbits)
            {
                case 1:     //0=None parity
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 2:     //1=Odd parity
                    serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }

            if (!FirstUp)
            {
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
                        return;
                    }
                }
                serialPort1.Write(BitConverter.GetBytes(mode), 0, 1);
                Thread.Sleep(200);
                bintodec = Binary_to_Dec();
                serialPort1.Write(BitConverter.GetBytes(bintodec), 0, 1);
                Thread.Sleep(200);
                serialPort1.Close();
            }
        }
        // Chat button
        private void Chat_button_Click(object sender, EventArgs e)
        {
            Chat chat = new Chat(parity, baudrate, databits, stopbits, port, timer_interval);
            try
            {
                chat.Tag = this;
                chat.Show(this);
                Hide();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }
        // File Transfer
        
        private void FileTransferButton_Click(object sender, EventArgs e)
        {
            send_files file_transfer = new send_files(parity, baudrate, databits, stopbits, port, timer_interval);
            try
            {
                file_transfer.Tag = this;
                file_transfer.Show(this);
                Hide();
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        private void ParityBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            parity = ParityBox.SelectedIndex;
            switch (parity)
            {
                case 0:     //0=None parity
                    SerialPort_config[7] = 0;     //Disable parity
                    break;
                case 1:     //1=Odd parity
                    SerialPort_config[7] = 1;      // Enable parity
                    SerialPort_config[6] = 0;      // Odd parity
                    break;
                case 2:     //2=Even parity
                    SerialPort_config[7] = 1;         // Enable parity
                    SerialPort_config[6] = 1;         // Even parity
                    break;
                default:
                    break;
            }

            set_com_settings(22);
            buff_parity = parity;
        }

        private void BaudrateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double Timer_value, baudrate_double, buff_databits_double;
            baudrate = Convert.ToInt32(BaudrateBox.SelectedItem.ToString());

            switch (baudrate)
            {
                case 1200:
                    SerialPort_config[0] = 0;
                    SerialPort_config[1] = 0;
                    SerialPort_config[2] = 0;
                    break;
                case 2400:
                    SerialPort_config[0] = 1;
                    SerialPort_config[1] = 0;
                    SerialPort_config[2] = 0;
                    break;
                case 4800:
                    SerialPort_config[0] = 0;
                    SerialPort_config[1] = 1;
                    SerialPort_config[2] = 0;
                    break;
                case 9600:
                    SerialPort_config[0] = 1;
                    SerialPort_config[1] = 1;
                    SerialPort_config[2] = 0;
                    break;

                default:
                    break;
            }

            set_com_settings(22);  //Go to CONFIG mode (0x16) on MSP

            baudrate_double = (double)baudrate;
            buff_databits_double = (double)buff_databits;
            Timer_value = ((buff_databits_double + 4) * 1000) / baudrate_double;
            //Changing time interval for timer usage later on -> depends on baudrate [(data_bits+4)/baud rate]*1000 up
            timer_interval = Convert.ToInt32(Math.Ceiling(Timer_value)); //ceiling up the number (igul up) -> returns double ->convert to integer
            buff_baudrate = baudrate;
        }

        private void DataBitsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            databits = Convert.ToInt32(DataBitsBox.SelectedItem.ToString());

            switch (databits)
            {
                case 7:
                    SerialPort_config[4] = 1;
                    break;
                case 8:
                    SerialPort_config[4] = 0;
                    break;
                default:
                    break;
            }

            set_com_settings(22);    //Go to CONFIG mode (0x16) on MSP
            buff_databits = databits;
        }

        private void StopBitBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            stopbits = Convert.ToInt32(StopBitBox.SelectedItem.ToString());

            switch (stopbits)
            {
                case 1:
                    SerialPort_config[3] = 0;
                    break;
                case 2:
                    SerialPort_config[3] = 1;
                    break;
                default:
                    break;
            }

            set_com_settings(22); //Go to CONFIG mode (0x16) on MSP

            buff_stopbits = stopbits;
        }

        private void PortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = PortBox.SelectedItem.ToString();
        }

        /***************************Data received from the MSP430 interrupt***************************/
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            char[] RxByte = new char[1];        //Create one char variable
            serialPort1.Read(RxByte, 0, 1);     //Read the received data into RxByte
            if ((int)RxByte[0] == 3)   //Ack from MSP430 - 03 hex, defined on MSP430 program
            {
                serialPort1.Close();
                MessageBox.Show("MSP430 changed communication configuration");
            }
        }

        /***************************On Close***************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serialPort1.Open();
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
            }
            catch (IOException)
            {
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
            }
        }

        /*********************************************************************************/
        /************************************Functions************************************/
        /*********************************************************************************/
        private int Binary_to_Dec()
        {
            int dec = 0;

            if (buff_databits == 7) //Without bit 5 (MSB bit)
            {
                dec = 64 * SerialPort_config[7] + 32 * SerialPort_config[6] + 16 * SerialPort_config[4] +
                        8 * SerialPort_config[3] + 4 * SerialPort_config[2] + 2 * SerialPort_config[1] + SerialPort_config[0];
            }
            else                    //With bit 5 (MSB bit)
                dec = 128 * SerialPort_config[7] + 64 * SerialPort_config[6] + 32 * 0 + 16 * SerialPort_config[4] +
                        8 * SerialPort_config[3] + 4 * SerialPort_config[2] + 2 * SerialPort_config[1] + SerialPort_config[0];
            return dec;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}