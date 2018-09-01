namespace head_test
{
    partial class StatusControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StatusLabel = new System.Windows.Forms.Label();
            this.SetFrameRateButton = new System.Windows.Forms.Button();
            this.FrameRateNumeric = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.FrameRateNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(4, 4);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(51, 20);
            this.StatusLabel.TabIndex = 0;
            this.StatusLabel.Text = "label1";
            // 
            // SetFrameRateButton
            // 
            this.SetFrameRateButton.Location = new System.Drawing.Point(403, 4);
            this.SetFrameRateButton.Name = "SetFrameRateButton";
            this.SetFrameRateButton.Size = new System.Drawing.Size(102, 23);
            this.SetFrameRateButton.TabIndex = 1;
            this.SetFrameRateButton.Text = "Set Frame Rate";
            this.SetFrameRateButton.UseVisualStyleBackColor = true;
            //this.SetFrameRateButton.Click += new System.EventHandler(this.SetFrameRateButton_Click);
            // 
            // FrameRateNumeric
            // 
            this.FrameRateNumeric.Location = new System.Drawing.Point(511, 7);
            this.FrameRateNumeric.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.FrameRateNumeric.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.FrameRateNumeric.Name = "FrameRateNumeric";
            this.FrameRateNumeric.Size = new System.Drawing.Size(81, 20);
            this.FrameRateNumeric.TabIndex = 2;
            this.FrameRateNumeric.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // StatusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FrameRateNumeric);
            this.Controls.Add(this.SetFrameRateButton);
            this.Controls.Add(this.StatusLabel);
            this.Name = "StatusControl";
            this.Size = new System.Drawing.Size(656, 446);
            ((System.ComponentModel.ISupportInitialize)(this.FrameRateNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button SetFrameRateButton;
        private System.Windows.Forms.NumericUpDown FrameRateNumeric;
    }
}
