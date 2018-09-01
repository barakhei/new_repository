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

namespace head_test
{
    public partial class MainMenu : Form
    {
        #region Variables
      
     //   DecimationTableCalculation mDecimationTableCalculation = null;
        
     //   bool FirstUp;
  //      int bintodec = 115;
     //   int parity = 2, buff_parity = 2; // 0=none , 1=odd , 2=even
        int baudrate = 9600, buff_baudrate = 9600;
     //   int databits = 7, buff_databits = 7;
    //    int stopbits = 1, buff_stopbits = 1; // 0=one , 1=two
      //  int[] SerialPort_config = { 1, 1, 0, 0, 1, 1, 1, 1 }; //Goes from 7 to 0 in the array

        /* SerialPort_config setting: Page 911 in datasheet (UCAxCTL0)
         * bit 7 : parity enable (0=disable , 1=enable)
         * bit 6 : parity type (0=odd , 1=even)
         * bit 5 : LSB/MSB (0=LSB first , 1=MSB first)
         * bit 4 : data bits (0=8bit , 1=7bit)
         * bit 3 : stop bit (0=one , 1=two)
         * bit 2-0: baudrate (0=1200 , 1=2400 , 2=4800 , 3=9600)
         */

        string port = "COM4";
        #endregion
        // constructor
        public MainMenu()
        {
            InitializeComponent();
        

            //Initial comboboxes values
            BaudrateBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PortBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //Initialize and set selected items
            string[] ports = SerialPort.GetPortNames();
            PortBox.Items.AddRange(ports);
          //  PortBox.SelectedIndex = 0;
            BaudrateBox.SelectedIndex = 0;
         //   FirstUp = false;
          
        }

        
            // Chat button
            private void Chat_button_Click(object sender, EventArgs e)
        {
            Chat chat = new Chat(port,baudrate);
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
            //send_files file_transfer = new send_files(parity, baudrate, databits, stopbits, port, timer_interval);
           // try
           // {
         //       file_transfer.Tag = this;
           //     file_transfer.Show(this);
           //     Hide();
          //  }
          //  catch (ObjectDisposedException)
          //  {
         //       return;
         //   }
        }

        private void BaudrateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            double baudrate_double;
            baudrate = Convert.ToInt32(BaudrateBox.SelectedItem.ToString());

        //    set_com_settings(22);  //Go to CONFIG mode (0x16) on MSP

            baudrate_double = (double)baudrate;
        }

       private void PortBox_SelectedIndexChanged(object sender, EventArgs e)
       {
            port = PortBox.SelectedItem.ToString();
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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