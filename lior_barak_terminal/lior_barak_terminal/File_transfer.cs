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
using System.Globalization;

namespace lior_barak_terminal
{
    public partial class send_files : Form
    {
        string text, datetext;
        string file_name;
        int char_count = 0;
        int files_count = 0;
        int file_done = 0;
        // constructor
        public send_files(int parity, int baudrate, int databits, int stopbits, string port, int timer_interval)
        {
            InitializeComponent();
            textBox1.ReadOnly = true;  //presents saving details
            textBox3.ReadOnly = true;  //presents loading details
            timer1.Interval = timer_interval;

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
                case 1:     //0=None parity
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case 2:     //1=Odd parity
                    serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    break;
            }
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
                    Close();
                    return;
                }
            }
        }
        // send browse button
        private void send_browse_button(object sender, EventArgs e)
        { //Browse button for SEND        
            DialogResult result = openFileDialog1.ShowDialog();    //Creates browse window
            if (result == DialogResult.OK)
            {
                file_name = openFileDialog1.FileName;
                textBox1.Text = file_name;                      //Show file name (address) on textbox
            }
        }
        // add to spuler button
        private void add_to_spuler_button(object sender, EventArgs e)
        {      //Add to spuler button adding giles and order them ///activates FILE_TRANSFER_GET mode on MSP430
            if (textBox1.Text == "") MessageBox.Show("File address is missing");
            else
            {
                files_count++;
                this.QueTable.Rows.Add(this.comboBox1.SelectedItem, this.QueTable.RowCount, this.textBox1.Text.ToString(), File.ReadAllText(file_name).Length, "Pending");
                this.QueTable.Sort(this.QueTable.Columns[0], ListSortDirection.Ascending);
                //  this.QueTable.Sort(this.QueTable.Columns[4], ListSortDirection.Descending);

            }
        }
        // sent buttun
        private void send_all(object sender, EventArgs e)
        {      //send all files
            serialPort1.Write(BitConverter.GetBytes(12), 0, 1); //FILE_TRANSFER_GET mode
            this.timer2.Start();
            for (int i = 0; i < files_count; i++)
            {
                if (this.QueTable.Rows[i].Cells[4].Value == "Pending") send_to_MSP(sender, e);
            }
        }

        // send to msp Function 
        private void send_to_MSP(object sender, EventArgs e)
        {
            if (this.QueTable.Rows[file_done].Cells[4].Value == "Pending")
            {
                text = File.ReadAllText(this.QueTable.Rows[0].Cells[2].Value.ToString()); // text from file. "\r\n"=new line in windows
                serialPort1.Write(BitConverter.GetBytes(12), 0, 1); //FILE_TRANSFER_GET mode
                Thread.Sleep(50);
                timer1.Start();                                     //Starts timer for sending file

            }

        }
        // timer event        
        private void timer1_Tick(object sender, EventArgs e)
        {  //Timer event for file sending
            //this.progressBar1.Increment(100 / text.Length);
            timer1.Stop();                                      //Stops until we send the next character

            char c_temp;
            this.QueTable.Rows[file_done].Cells[4].Value = "Sending";
            if (text.Length > char_count)
            {                      //Send each character
                char[] chat_to_send = { '0' };    //start_bit
                c_temp = text[char_count++];
                chat_to_send[0] = c_temp;
                serialPort1.Write(chat_to_send, 0, 1);
                timer1.Start();
                timer1.Enabled = true;  //Enable to get another character
            }
            else
            {  //Reading file completed
                char_count = 0;     //Initial for the next time
                serialPort1.Write(BitConverter.GetBytes(3), 0, 1);  //E-OF = 0x03
                timer1.Stop();
                timer1.Enabled = false;
                text = "";
                this.QueTable.Rows[file_done].Cells[4].Value = "Done";
                this.progressBar2.Increment(100 / files_count);

                if (file_done < (files_count - 1))
                    file_done++;
                // this.QueTable.Sort(this.QueTable.Columns[0], ListSortDirection.Ascending);
                // this.QueTable.Sort(this.QueTable.Columns[4], ListSortDirection.Descending);

                send_to_MSP(sender, e);
            }
        }
        // Recieve browse button
        private void receive_browse_button(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                file_name = saveFileDialog1.FileName;
                textBox3.Text = file_name;                           //Show file name on textbox
            }
        }

        //Get file from MSP430 memory button and write into textbox3 address
        private void get_msp_file_button(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                File.Delete(file_name);                             //Override file (need to delete first)
                datetext = DateTime.Now.ToString() + "\r\n";
                File.AppendAllText(file_name, datetext);
                serialPort1.Write(BitConverter.GetBytes(21), 0, 1); //FILE_from_MEM mode
                Thread.Sleep(1);
            }
            else MessageBox.Show("Get file address is missing");
        }
        // Back button
        private void back_button_Click(object sender, EventArgs e)
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
        // close button
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {  //Close serial port if the window get closed
            try
            {
                serialPort1.Open();
                serialPort1.Write(BitConverter.GetBytes(15), 0, 1);
                Thread.Sleep(100);
                serialPort1.Close();
            }
            catch
            {
                serialPort1.Write(BitConverter.GetBytes(15), 0, 1);
                Thread.Sleep(100);
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
            }
        }

        /***************************Data received from the MSP430 interrupt***************************/
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            char[] rx_data = new char[1];
            string rx_string;
            serialPort1.Read(rx_data, 0, 1);
            if ((int)rx_data[0] == 36) MessageBox.Show("File Sent");            //First Ack 0x24 in msp
            else if ((int)rx_data[0] == 3) MessageBox.Show("File received");    //Second Ack 0x03==EOF

            else
            {                                                              //read data
                rx_string = new string(rx_data);
                if (file_name != null)
                {
                    File.AppendAllText(file_name, rx_string);
                    serialPort1.DiscardInBuffer();
                }

            }
        }

        private void Table_change_ev(object sender, EventArgs e) { }

        private void Form3_Load(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void receive_textBox(object sender, EventArgs e) { }

        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void textBox6_TextChanged(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void priority_comboBox(object sender, EventArgs e) { }

        private void QueTable_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e) { }

        private void send_textBox(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void progressBar2_Click(object sender, EventArgs e)
        {

        }
    }
}
