namespace lior_barak_terminal
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ParityBox = new System.Windows.Forms.ComboBox();
            this.BaudrateBox = new System.Windows.Forms.ComboBox();
            this.DataBitsBox = new System.Windows.Forms.ComboBox();
            this.StopBitBox = new System.Windows.Forms.ComboBox();
            this.PortBox = new System.Windows.Forms.ComboBox();
            this.ChatButton = new System.Windows.Forms.Button();
            this.FileTransferButton = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(18, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parity";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(18, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(177)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(18, 100);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Data bits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(177)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(18, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stop bit";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(18, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 21);
            this.label5.TabIndex = 4;
            this.label5.Text = "Port";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // ParityBox
            // 
            this.ParityBox.FormattingEnabled = true;
            this.ParityBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ParityBox.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.ParityBox.Location = new System.Drawing.Point(141, 93);
            this.ParityBox.Margin = new System.Windows.Forms.Padding(2);
            this.ParityBox.Name = "ParityBox";
            this.ParityBox.Size = new System.Drawing.Size(53, 21);
            this.ParityBox.TabIndex = 5;
            this.ParityBox.SelectedIndexChanged += new System.EventHandler(this.ParityBox_SelectedIndexChanged);
            // 
            // BaudrateBox
            // 
            this.BaudrateBox.FormattingEnabled = true;
            this.BaudrateBox.Items.AddRange(new object[] {
            "9600",
            "4800",
            "2400",
            "1200"});
            this.BaudrateBox.Location = new System.Drawing.Point(141, 65);
            this.BaudrateBox.Margin = new System.Windows.Forms.Padding(2);
            this.BaudrateBox.Name = "BaudrateBox";
            this.BaudrateBox.Size = new System.Drawing.Size(68, 21);
            this.BaudrateBox.TabIndex = 6;
            this.BaudrateBox.SelectedIndexChanged += new System.EventHandler(this.BaudrateBox_SelectedIndexChanged);
            // 
            // DataBitsBox
            // 
            this.DataBitsBox.FormattingEnabled = true;
            this.DataBitsBox.Items.AddRange(new object[] {
            "7",
            "8"});
            this.DataBitsBox.Location = new System.Drawing.Point(141, 118);
            this.DataBitsBox.Margin = new System.Windows.Forms.Padding(2);
            this.DataBitsBox.Name = "DataBitsBox";
            this.DataBitsBox.Size = new System.Drawing.Size(54, 21);
            this.DataBitsBox.TabIndex = 7;
            this.DataBitsBox.SelectedIndexChanged += new System.EventHandler(this.DataBitsBox_SelectedIndexChanged);
            // 
            // StopBitBox
            // 
            this.StopBitBox.FormattingEnabled = true;
            this.StopBitBox.Items.AddRange(new object[] {
            "1",
            "2"});
            this.StopBitBox.Location = new System.Drawing.Point(141, 143);
            this.StopBitBox.Margin = new System.Windows.Forms.Padding(2);
            this.StopBitBox.Name = "StopBitBox";
            this.StopBitBox.Size = new System.Drawing.Size(45, 21);
            this.StopBitBox.TabIndex = 8;
            this.StopBitBox.SelectedIndexChanged += new System.EventHandler(this.StopBitBox_SelectedIndexChanged);
            // 
            // PortBox
            // 
            this.PortBox.DropDownHeight = 90;
            this.PortBox.FormattingEnabled = true;
            this.PortBox.IntegralHeight = false;
            this.PortBox.ItemHeight = 13;
            this.PortBox.Location = new System.Drawing.Point(117, 40);
            this.PortBox.Margin = new System.Windows.Forms.Padding(2);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(92, 21);
            this.PortBox.TabIndex = 9;
            this.PortBox.SelectedIndexChanged += new System.EventHandler(this.PortBox_SelectedIndexChanged);
            // 
            // ChatButton
            // 
            this.ChatButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ChatButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.ChatButton.Location = new System.Drawing.Point(11, 223);
            this.ChatButton.Margin = new System.Windows.Forms.Padding(2);
            this.ChatButton.Name = "ChatButton";
            this.ChatButton.Size = new System.Drawing.Size(121, 84);
            this.ChatButton.TabIndex = 10;
            this.ChatButton.Text = "Chat";
            this.ChatButton.UseVisualStyleBackColor = true;
            this.ChatButton.Click += new System.EventHandler(this.Chat_button_Click);
            // 
            // FileTransferButton
            // 
            this.FileTransferButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FileTransferButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.FileTransferButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.FileTransferButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(0)), true);
            this.FileTransferButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FileTransferButton.Location = new System.Drawing.Point(161, 223);
            this.FileTransferButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileTransferButton.Name = "FileTransferButton";
            this.FileTransferButton.Size = new System.Drawing.Size(133, 84);
            this.FileTransferButton.TabIndex = 11;
            this.FileTransferButton.Text = "File Transfer";
            this.FileTransferButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.FileTransferButton.UseVisualStyleBackColor = true;
            this.FileTransferButton.Click += new System.EventHandler(this.FileTransferButton_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.DtrEnable = true;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(56, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "Serial Port Setting";
            this.label6.Click += new System.EventHandler(this.label6_Click_1);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(305, 355);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FileTransferButton);
            this.Controls.Add(this.ChatButton);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.StopBitBox);
            this.Controls.Add(this.DataBitsBox);
            this.Controls.Add(this.BaudrateBox);
            this.Controls.Add(this.ParityBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ParityBox;
        private System.Windows.Forms.ComboBox BaudrateBox;
        private System.Windows.Forms.ComboBox DataBitsBox;
        private System.Windows.Forms.ComboBox StopBitBox;
        private System.Windows.Forms.ComboBox PortBox;
        private System.Windows.Forms.Button ChatButton;
        private System.Windows.Forms.Button FileTransferButton;
        private System.IO.Ports.SerialPort serialPort1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label6;
    }
}

