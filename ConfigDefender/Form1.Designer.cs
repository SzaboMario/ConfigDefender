
namespace ConfigDefender
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
            this.addFolderBtn = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.startBtn = new System.Windows.Forms.Button();
            this.listClearBtn = new System.Windows.Forms.Button();
            this.deleteSelectedBtn = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.openLogBtn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.addFilesBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // addFolderBtn
            // 
            this.addFolderBtn.Location = new System.Drawing.Point(198, 12);
            this.addFolderBtn.Name = "addFolderBtn";
            this.addFolderBtn.Size = new System.Drawing.Size(90, 24);
            this.addFolderBtn.TabIndex = 0;
            this.addFolderBtn.Text = "Add folder";
            this.addFolderBtn.UseVisualStyleBackColor = true;
            this.addFolderBtn.Click += new System.EventHandler(this.addFileBtn_Click);
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.FormattingEnabled = true;
            this.listBox.HorizontalScrollbar = true;
            this.listBox.Location = new System.Drawing.Point(1, 78);
            this.listBox.Name = "listBox";
            this.listBox.ScrollAlwaysVisible = true;
            this.listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox.Size = new System.Drawing.Size(489, 225);
            this.listBox.Sorted = true;
            this.listBox.TabIndex = 1;
            // 
            // startBtn
            // 
            this.startBtn.BackColor = System.Drawing.Color.Tomato;
            this.startBtn.Location = new System.Drawing.Point(12, 7);
            this.startBtn.MinimumSize = new System.Drawing.Size(75, 23);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(90, 34);
            this.startBtn.TabIndex = 2;
            this.startBtn.Text = "Stopped";
            this.startBtn.UseVisualStyleBackColor = false;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // listClearBtn
            // 
            this.listClearBtn.Location = new System.Drawing.Point(390, 42);
            this.listClearBtn.Name = "listClearBtn";
            this.listClearBtn.Size = new System.Drawing.Size(90, 24);
            this.listClearBtn.TabIndex = 3;
            this.listClearBtn.Text = "Delete all";
            this.listClearBtn.UseVisualStyleBackColor = true;
            this.listClearBtn.Click += new System.EventHandler(this.listClearBtn_Click);
            // 
            // deleteSelectedBtn
            // 
            this.deleteSelectedBtn.Location = new System.Drawing.Point(390, 12);
            this.deleteSelectedBtn.Name = "deleteSelectedBtn";
            this.deleteSelectedBtn.Size = new System.Drawing.Size(90, 24);
            this.deleteSelectedBtn.TabIndex = 4;
            this.deleteSelectedBtn.Text = "Delete selected";
            this.deleteSelectedBtn.UseVisualStyleBackColor = true;
            this.deleteSelectedBtn.Click += new System.EventHandler(this.deleteSelectedBtn_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClosed += new System.EventHandler(this.notifyIcon1_BalloonTipClosed);
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 24);
            this.button1.TabIndex = 5;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openLogBtn
            // 
            this.openLogBtn.Location = new System.Drawing.Point(294, 12);
            this.openLogBtn.Name = "openLogBtn";
            this.openLogBtn.Size = new System.Drawing.Size(90, 24);
            this.openLogBtn.TabIndex = 6;
            this.openLogBtn.Text = "Open log";
            this.openLogBtn.UseVisualStyleBackColor = true;
            this.openLogBtn.Click += new System.EventHandler(this.openLogBtn_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(294, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 24);
            this.button2.TabIndex = 7;
            this.button2.Text = "Open backup";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // addFilesBtn
            // 
            this.addFilesBtn.Location = new System.Drawing.Point(198, 42);
            this.addFilesBtn.Name = "addFilesBtn";
            this.addFilesBtn.Size = new System.Drawing.Size(90, 24);
            this.addFilesBtn.TabIndex = 8;
            this.addFilesBtn.Text = "Add files";
            this.addFilesBtn.UseVisualStyleBackColor = true;
            this.addFilesBtn.Click += new System.EventHandler(this.addFilesBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Config files|*.txt;*.ini;*.cfg";
            this.openFileDialog1.Multiselect = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 314);
            this.Controls.Add(this.addFilesBtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.openLogBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.deleteSelectedBtn);
            this.Controls.Add(this.listClearBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.addFolderBtn);
            this.Name = "Form1";
            this.Text = " Config Defender";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button addFolderBtn;
        public System.Windows.Forms.ListBox listBox;
        public System.Windows.Forms.Button startBtn;
        public System.Windows.Forms.Button listClearBtn;
        public System.Windows.Forms.Button deleteSelectedBtn;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button openLogBtn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button addFilesBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

