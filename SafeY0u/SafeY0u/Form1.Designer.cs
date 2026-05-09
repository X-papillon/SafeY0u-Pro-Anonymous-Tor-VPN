namespace SafeY0u
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richLog = new System.Windows.Forms.RichTextBox();
            this.progressBarTor = new System.Windows.Forms.ProgressBar();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblOnlineStatus = new System.Windows.Forms.Label();
            this.lblUserRelayIP = new System.Windows.Forms.Label();
            this.lblProxy = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblProgressPercent = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.lblTorPublicIP = new System.Windows.Forms.Label();
            this.txtBrowserPath = new System.Windows.Forms.TextBox();
            this.btnSelectBrowser = new System.Windows.Forms.Button();
            this.btnRemoveBrowser = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // richLog
            // 
            this.richLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(17)))), ((int)(((byte)(24)))));
            this.richLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richLog.Location = new System.Drawing.Point(16, 255);
            this.richLog.Name = "richLog";
            this.richLog.ReadOnly = true;
            this.richLog.Size = new System.Drawing.Size(679, 146);
            this.richLog.TabIndex = 0;
            this.richLog.Text = "";
            this.richLog.WordWrap = false;
            // 
            // progressBarTor
            // 
            this.progressBarTor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.progressBarTor.Location = new System.Drawing.Point(16, 407);
            this.progressBarTor.Name = "progressBarTor";
            this.progressBarTor.Size = new System.Drawing.Size(648, 8);
            this.progressBarTor.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTor.TabIndex = 1;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(16, 226);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(587, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "⏹ STOP";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(16, 199);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(587, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "▶ START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblOnlineStatus
            // 
            this.lblOnlineStatus.AutoSize = true;
            this.lblOnlineStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.lblOnlineStatus.Location = new System.Drawing.Point(13, 15);
            this.lblOnlineStatus.Name = "lblOnlineStatus";
            this.lblOnlineStatus.Size = new System.Drawing.Size(60, 13);
            this.lblOnlineStatus.TabIndex = 3;
            this.lblOnlineStatus.Text = "📡 Internet:";
            // 
            // lblUserRelayIP
            // 
            this.lblUserRelayIP.AutoSize = true;
            this.lblUserRelayIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.lblUserRelayIP.Location = new System.Drawing.Point(13, 42);
            this.lblUserRelayIP.Name = "lblUserRelayIP";
            this.lblUserRelayIP.Size = new System.Drawing.Size(92, 13);
            this.lblUserRelayIP.TabIndex = 3;
            this.lblUserRelayIP.Text = "🖥️ Your Public IP:";
            // 
            // lblProxy
            // 
            this.lblProxy.AutoSize = true;
            this.lblProxy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.lblProxy.Location = new System.Drawing.Point(13, 99);
            this.lblProxy.Name = "lblProxy";
            this.lblProxy.Size = new System.Drawing.Size(95, 13);
            this.lblProxy.TabIndex = 3;
            this.lblProxy.Text = "🔌 SOCKS5 Proxy:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.lblStatus.Location = new System.Drawing.Point(13, 128);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(112, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "⚫ Status: Not Started";
            // 
            // lblProgressPercent
            // 
            this.lblProgressPercent.AutoSize = true;
            this.lblProgressPercent.BackColor = System.Drawing.Color.Transparent;
            this.lblProgressPercent.ForeColor = System.Drawing.Color.White;
            this.lblProgressPercent.Location = new System.Drawing.Point(665, 404);
            this.lblProgressPercent.Name = "lblProgressPercent";
            this.lblProgressPercent.Size = new System.Drawing.Size(21, 13);
            this.lblProgressPercent.TabIndex = 3;
            this.lblProgressPercent.Text = "0%";
            // 
            // lblTorPublicIP
            // 
            this.lblTorPublicIP.AutoSize = true;
            this.lblTorPublicIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.lblTorPublicIP.Location = new System.Drawing.Point(13, 70);
            this.lblTorPublicIP.Name = "lblTorPublicIP";
            this.lblTorPublicIP.Size = new System.Drawing.Size(86, 13);
            this.lblTorPublicIP.TabIndex = 3;
            this.lblTorPublicIP.Text = "🌐 Tor Public IP:";
            // 
            // txtBrowserPath
            // 
            this.txtBrowserPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(17)))), ((int)(((byte)(24)))));
            this.txtBrowserPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBrowserPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBrowserPath.ForeColor = System.Drawing.Color.White;
            this.txtBrowserPath.Location = new System.Drawing.Point(146, 171);
            this.txtBrowserPath.Name = "txtBrowserPath";
            this.txtBrowserPath.ReadOnly = true;
            this.txtBrowserPath.Size = new System.Drawing.Size(337, 21);
            this.txtBrowserPath.TabIndex = 4;
            // 
            // btnSelectBrowser
            // 
            this.btnSelectBrowser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.btnSelectBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectBrowser.Location = new System.Drawing.Point(489, 169);
            this.btnSelectBrowser.Name = "btnSelectBrowser";
            this.btnSelectBrowser.Size = new System.Drawing.Size(114, 24);
            this.btnSelectBrowser.TabIndex = 5;
            this.btnSelectBrowser.Text = "📁 Select";
            this.btnSelectBrowser.UseVisualStyleBackColor = false;
            this.btnSelectBrowser.Click += new System.EventHandler(this.BtnSelectBrowser_Click);
            // 
            // btnRemoveBrowser
            // 
            this.btnRemoveBrowser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.btnRemoveBrowser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBrowser.Location = new System.Drawing.Point(609, 169);
            this.btnRemoveBrowser.Name = "btnRemoveBrowser";
            this.btnRemoveBrowser.Size = new System.Drawing.Size(86, 24);
            this.btnRemoveBrowser.TabIndex = 5;
            this.btnRemoveBrowser.Text = "🗑 Remove";
            this.btnRemoveBrowser.UseVisualStyleBackColor = false;
            this.btnRemoveBrowser.Click += new System.EventHandler(this.BtnRemoveBrowser_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(175)))), ((int)(((byte)(187)))));
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Location = new System.Drawing.Point(609, 199);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(86, 50);
            this.btnClearLog.TabIndex = 6;
            this.btnClearLog.Text = "🗑 Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(293, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 150);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(303, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(392, 130);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
            this.label2.Location = new System.Drawing.Point(13, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "🚀 Auto-Launch Browser:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.MediumSeaGreen;
            this.linkLabel1.Location = new System.Drawing.Point(562, 7);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(133, 13);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "☕ Support the Project";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SafeY0u.Properties.Resources.image_1_1778259476320_removebg_preview;
            this.pictureBox1.Location = new System.Drawing.Point(245, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 55);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Location = new System.Drawing.Point(16, 163);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(679, 1);
            this.panel2.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(17)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(710, 420);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.btnRemoveBrowser);
            this.Controls.Add(this.btnSelectBrowser);
            this.Controls.Add(this.txtBrowserPath);
            this.Controls.Add(this.lblProgressPercent);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblProxy);
            this.Controls.Add(this.lblTorPublicIP);
            this.Controls.Add(this.lblUserRelayIP);
            this.Controls.Add(this.lblOnlineStatus);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.progressBarTor);
            this.Controls.Add(this.richLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🛡️ SafeY0u Pro v1.0 - Anonymous Tor VPN by Xpapillon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richLog;
        private System.Windows.Forms.ProgressBar progressBarTor;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblOnlineStatus;
        private System.Windows.Forms.Label lblUserRelayIP;
        private System.Windows.Forms.Label lblProxy;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblProgressPercent;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label lblTorPublicIP;
        private System.Windows.Forms.TextBox txtBrowserPath;
        private System.Windows.Forms.Button btnSelectBrowser;
        private System.Windows.Forms.Button btnRemoveBrowser;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
    }
}

