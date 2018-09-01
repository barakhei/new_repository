namespace head_test
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
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BaudrateBox = new System.Windows.Forms.ComboBox();
            this.PortBox = new System.Windows.Forms.ComboBox();
            this.ChatButton = new System.Windows.Forms.Button();
            this.FileTransferButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(24, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(24, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Port";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // BaudrateBox
            // 
            this.BaudrateBox.FormattingEnabled = true;
            this.BaudrateBox.Items.AddRange(new object[] {
            "115200",
            "9600",
            "4800",
            "2400"});
            this.BaudrateBox.Location = new System.Drawing.Point(188, 80);
            this.BaudrateBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BaudrateBox.Name = "BaudrateBox";
            this.BaudrateBox.Size = new System.Drawing.Size(89, 24);
            this.BaudrateBox.TabIndex = 6;
            this.BaudrateBox.SelectedIndexChanged += new System.EventHandler(this.BaudrateBox_SelectedIndexChanged);
            // 
            // PortBox
            // 
            this.PortBox.DropDownHeight = 90;
            this.PortBox.FormattingEnabled = true;
            this.PortBox.IntegralHeight = false;
            this.PortBox.ItemHeight = 16;
            this.PortBox.Location = new System.Drawing.Point(156, 49);
            this.PortBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(121, 24);
            this.PortBox.TabIndex = 9;
            this.PortBox.SelectedIndexChanged += new System.EventHandler(this.PortBox_SelectedIndexChanged);
            // 
            // ChatButton
            // 
            this.ChatButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ChatButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.ChatButton.Location = new System.Drawing.Point(15, 274);
            this.ChatButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChatButton.Name = "ChatButton";
            this.ChatButton.Size = new System.Drawing.Size(161, 103);
            this.ChatButton.TabIndex = 10;
            this.ChatButton.Text = "TEST THE HEAD CARD";
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
            this.FileTransferButton.Location = new System.Drawing.Point(215, 274);
            this.FileTransferButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FileTransferButton.Name = "FileTransferButton";
            this.FileTransferButton.Size = new System.Drawing.Size(177, 103);
            this.FileTransferButton.TabIndex = 11;
            this.FileTransferButton.Text = "Not work";
            this.FileTransferButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.FileTransferButton.UseVisualStyleBackColor = true;
            this.FileTransferButton.Click += new System.EventHandler(this.FileTransferButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(75, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 23);
            this.label6.TabIndex = 13;
            this.label6.Text = "Serial Port Setting";
            this.label6.Click += new System.EventHandler(this.label6_Click_1);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(407, 437);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FileTransferButton);
            this.Controls.Add(this.ChatButton);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.BaudrateBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox BaudrateBox;
        private System.Windows.Forms.ComboBox PortBox;
        private System.Windows.Forms.Button ChatButton;
        private System.Windows.Forms.Button FileTransferButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label6;
    }
}

