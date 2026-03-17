namespace WeatherApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox zipTextBox;
        private System.Windows.Forms.Button fetchButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.PictureBox tempPictureBox;
        private System.Windows.Forms.Label tempLabel;
        private System.Windows.Forms.PictureBox weatherPictureBox;
        private System.Windows.Forms.Label weatherLabel;
        private System.Windows.Forms.PictureBox windPictureBox;
        private System.Windows.Forms.Label windLabel;
        private System.Windows.Forms.Label moonLabel;
        private System.Windows.Forms.Button alertButton;
        private System.Windows.Forms.Label lastUpdateLabel;
        private System.Windows.Forms.Timer updateTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.zipTextBox = new System.Windows.Forms.TextBox();
            this.fetchButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.tempPictureBox = new System.Windows.Forms.PictureBox();
            this.tempLabel = new System.Windows.Forms.Label();
            this.weatherPictureBox = new System.Windows.Forms.PictureBox();
            this.weatherLabel = new System.Windows.Forms.Label();
            this.windPictureBox = new System.Windows.Forms.PictureBox();
            this.windLabel = new System.Windows.Forms.Label();
            this.moonLabel = new System.Windows.Forms.Label();
            this.alertButton = new System.Windows.Forms.Button();
            this.lastUpdateLabel = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tempPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.weatherPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // zipTextBox
            // 
            this.zipTextBox.BackColor = System.Drawing.Color.Black;
            this.zipTextBox.ForeColor = System.Drawing.Color.White;
            this.zipTextBox.Location = new System.Drawing.Point(20, 20);
            this.zipTextBox.Name = "zipTextBox";
            this.zipTextBox.Size = new System.Drawing.Size(120, 20);
            this.zipTextBox.TabIndex = 0;
            this.zipTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // fetchButton
            // 
            this.fetchButton.BackColor = System.Drawing.Color.Black;
            this.fetchButton.ForeColor = System.Drawing.Color.White;
            this.fetchButton.Location = new System.Drawing.Point(150, 18);
            this.fetchButton.Name = "fetchButton";
            this.fetchButton.Size = new System.Drawing.Size(50, 23);
            this.fetchButton.TabIndex = 1;
            this.fetchButton.Text = "Go";
            this.fetchButton.UseVisualStyleBackColor = false;
            this.fetchButton.Click += new System.EventHandler(this.fetchButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ForeColor = System.Drawing.Color.Gold;
            this.nameLabel.Location = new System.Drawing.Point(20, 60);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(200, 29);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tempPictureBox
            // 
            this.tempPictureBox.Image = global::WeatherApp.Properties.Resources.thermometer;
            this.tempPictureBox.Location = new System.Drawing.Point(20, 100);
            this.tempPictureBox.Name = "tempPictureBox";
            this.tempPictureBox.Size = new System.Drawing.Size(32, 22);
            this.tempPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tempPictureBox.TabIndex = 3;
            this.tempPictureBox.TabStop = false;
            // 
            // tempLabel
            // 
            this.tempLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tempLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempLabel.ForeColor = System.Drawing.Color.White;
            this.tempLabel.Location = new System.Drawing.Point(122, 100);
            this.tempLabel.Name = "tempLabel";
            this.tempLabel.Size = new System.Drawing.Size(98, 22);
            this.tempLabel.TabIndex = 4;
            this.tempLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // weatherPictureBox
            // 
            this.weatherPictureBox.Location = new System.Drawing.Point(20, 140);
            this.weatherPictureBox.Name = "weatherPictureBox";
            this.weatherPictureBox.Size = new System.Drawing.Size(32, 23);
            this.weatherPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.weatherPictureBox.TabIndex = 5;
            this.weatherPictureBox.TabStop = false;
            // 
            // weatherLabel
            // 
            this.weatherLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.weatherLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weatherLabel.ForeColor = System.Drawing.Color.White;
            this.weatherLabel.Location = new System.Drawing.Point(124, 140);
            this.weatherLabel.Name = "weatherLabel";
            this.weatherLabel.Size = new System.Drawing.Size(96, 23);
            this.weatherLabel.TabIndex = 6;
            this.weatherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // windPictureBox
            // 
            this.windPictureBox.Image = global::WeatherApp.Properties.Resources.wind;
            this.windPictureBox.Location = new System.Drawing.Point(20, 180);
            this.windPictureBox.Name = "windPictureBox";
            this.windPictureBox.Size = new System.Drawing.Size(32, 23);
            this.windPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.windPictureBox.TabIndex = 7;
            this.windPictureBox.TabStop = false;
            // 
            // windLabel
            // 
            this.windLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windLabel.ForeColor = System.Drawing.Color.White;
            this.windLabel.Location = new System.Drawing.Point(124, 180);
            this.windLabel.Name = "windLabel";
            this.windLabel.Size = new System.Drawing.Size(96, 23);
            this.windLabel.TabIndex = 8;
            this.windLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // moonLabel
            // 
            this.moonLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moonLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moonLabel.ForeColor = System.Drawing.Color.Cyan;
            this.moonLabel.Location = new System.Drawing.Point(20, 220);
            this.moonLabel.Name = "moonLabel";
            this.moonLabel.Size = new System.Drawing.Size(200, 18);
            this.moonLabel.TabIndex = 9;
            this.moonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alertButton
            // 
            this.alertButton.BackColor = System.Drawing.Color.Gray;
            this.alertButton.Enabled = false;
            this.alertButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.alertButton.ForeColor = System.Drawing.Color.White;
            this.alertButton.Location = new System.Drawing.Point(76, 250);
            this.alertButton.Name = "alertButton";
            this.alertButton.Size = new System.Drawing.Size(85, 23);
            this.alertButton.TabIndex = 10;
            this.alertButton.Text = "ALERT";
            this.alertButton.UseVisualStyleBackColor = false;
            this.alertButton.Click += new System.EventHandler(this.alertButton_Click);
            // 
            // lastUpdateLabel
            // 
            this.lastUpdateLabel.Font = new System.Drawing.Font("Arial", 10F);
            this.lastUpdateLabel.ForeColor = System.Drawing.Color.Gray;
            this.lastUpdateLabel.Location = new System.Drawing.Point(20, 300);
            this.lastUpdateLabel.Name = "lastUpdateLabel";
            this.lastUpdateLabel.Size = new System.Drawing.Size(150, 16);
            this.lastUpdateLabel.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(61, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Weather:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(61, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Temp:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(61, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Wind:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(232, 300);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lastUpdateLabel);
            this.Controls.Add(this.alertButton);
            this.Controls.Add(this.moonLabel);
            this.Controls.Add(this.windLabel);
            this.Controls.Add(this.windPictureBox);
            this.Controls.Add(this.weatherLabel);
            this.Controls.Add(this.weatherPictureBox);
            this.Controls.Add(this.tempLabel);
            this.Controls.Add(this.tempPictureBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.fetchButton);
            this.Controls.Add(this.zipTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Weather App";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tempPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.weatherPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
