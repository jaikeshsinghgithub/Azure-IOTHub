namespace IoTDMSample
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.logView = new System.Windows.Forms.TextBox();
            this.startD2C = new System.Windows.Forms.Button();
            this.fanSpeed = new System.Windows.Forms.NumericUpDown();
            this.temperature = new System.Windows.Forms.NumericUpDown();
            this.pressure = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.waterLevel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fanSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.temperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waterLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fan Speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Temperature";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pressure";
            // 
            // logView
            // 
            this.logView.Location = new System.Drawing.Point(12, 145);
            this.logView.Multiline = true;
            this.logView.Name = "logView";
            this.logView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logView.Size = new System.Drawing.Size(704, 315);
            this.logView.TabIndex = 6;
            // 
            // startD2C
            // 
            this.startD2C.Location = new System.Drawing.Point(19, 116);
            this.startD2C.Name = "startD2C";
            this.startD2C.Size = new System.Drawing.Size(46, 23);
            this.startD2C.TabIndex = 7;
            this.startD2C.Text = "&Start";
            this.startD2C.UseVisualStyleBackColor = true;
            this.startD2C.Click += new System.EventHandler(this.startD2C_Click);
            // 
            // fanSpeed
            // 
            this.fanSpeed.Location = new System.Drawing.Point(98, 10);
            this.fanSpeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.fanSpeed.Name = "fanSpeed";
            this.fanSpeed.Size = new System.Drawing.Size(120, 20);
            this.fanSpeed.TabIndex = 8;
            this.fanSpeed.Value = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            // 
            // temperature
            // 
            this.temperature.Location = new System.Drawing.Point(98, 36);
            this.temperature.Name = "temperature";
            this.temperature.Size = new System.Drawing.Size(120, 20);
            this.temperature.TabIndex = 9;
            this.temperature.Value = new decimal(new int[] {
            125,
            0,
            0,
            65536});
            // 
            // pressure
            // 
            this.pressure.Location = new System.Drawing.Point(98, 62);
            this.pressure.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.pressure.Name = "pressure";
            this.pressure.Size = new System.Drawing.Size(120, 20);
            this.pressure.TabIndex = 10;
            this.pressure.Value = new decimal(new int[] {
            925,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(71, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "&End";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // waterLevel
            // 
            this.waterLevel.DecimalPlaces = 2;
            this.waterLevel.Location = new System.Drawing.Point(98, 88);
            this.waterLevel.Name = "waterLevel";
            this.waterLevel.Size = new System.Drawing.Size(120, 20);
            this.waterLevel.TabIndex = 13;
            this.waterLevel.Value = new decimal(new int[] {
            925,
            0,
            0,
            65536});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Water Level";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 472);
            this.Controls.Add(this.waterLevel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pressure);
            this.Controls.Add(this.temperature);
            this.Controls.Add(this.fanSpeed);
            this.Controls.Add(this.startD2C);
            this.Controls.Add(this.logView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.fanSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.temperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waterLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox logView;
        private System.Windows.Forms.Button startD2C;
        private System.Windows.Forms.NumericUpDown fanSpeed;
        private System.Windows.Forms.NumericUpDown temperature;
        private System.Windows.Forms.NumericUpDown pressure;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown waterLevel;
        private System.Windows.Forms.Label label4;
    }
}

