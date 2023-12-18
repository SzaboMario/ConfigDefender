using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ConfigDefender
{
    public partial class Form1 : Form
    {
        public readonly string filesTxT = "files.txt";
        public readonly string backupFolder = "ConfigBackup";
        Core core;
        internal string[] fileNamesFromFolderArr;
        internal List<string[]> fileDataList;
        private bool run;
        public Form1()
        {
            InitializeComponent();
            core = new Core(this);
            this.notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.notifyIcon1.ContextMenuStrip.Items.Add("Open log", null, this.MenuOpenLog_Click);
            this.notifyIcon1.ContextMenuStrip.Items.Add("Open program", null, this.MenuOpen_Click);
            this.notifyIcon1.ContextMenuStrip.Items.Add("Exit", null, this.MenuExit_Click);
            // this.ShowInTaskbar = false;
        }
        private void MenuExit_Click(object sender, EventArgs e)
        {
            using (PassForm pf = new PassForm())
            {
                pf.StartPosition = FormStartPosition.CenterScreen;
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        notifyIcon1.Visible = false;
                        notifyIcon1.Dispose();
                        core.WriteLog("==========Program exit==========");
                        run = false;
                        Environment.Exit(0);
                    }
                }
            }
        }
        private void MenuOpen_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private void MenuOpenLog_Click(object sender, EventArgs e)
        {
            openLogBtn_Click(sender,null);
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Icon = this.Icon;
                notifyIcon1.Text = "ConfigDefender";
                notifyIcon1.Visible = true;
                this.Hide();
            }
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {//indításkori ellenőrzés
            core.WriteLog("==========Program started==========");
            fileDataList = core.GetFileData(filesTxT);//ha létezik, akkor betölti az adatokat            
            fileNamesFromFolderArr = core.GetFileNamesFromFolder(backupFolder);//ha van akkor betőlti a fájlok nevét         
            core.SetDataToListBox(fileDataList);
            core.CompareFileNames(fileDataList, fileNamesFromFolderArr);//adatok összehasonlítása
        }
        internal void AddSelectedFileToListBox(string[] fileArray)
        {
            if (listBox.Items.Count == 0)
            {
                foreach (var item in fileArray)
                {
                    listBox.Items.Add(item);
                }
            }
            else
            {
                foreach (string it in fileArray)
                {
                    if (!listBox.Items.Contains(it))
                    {
                        listBox.Items.Add(it);
                    }
                }
            }
        }
        private void addFileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = folderBrowserDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string folders = folderBrowserDialog1.SelectedPath;
                    string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*", SearchOption.AllDirectories).
                        Where(s => s.EndsWith(".txt") || s.EndsWith(".ini") || s.EndsWith(".cfg")).ToArray();
                    AddSelectedFileToListBox(files);
                }
                core.SaveListBoxDataToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void startBtn_Click(object sender, EventArgs e)
        {
            core.StartStop();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (PassForm pf = new PassForm())
            {
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        notifyIcon1.Visible = false;
                        notifyIcon1.Dispose();
                        run = false;
                        core.WriteLog("==========Program exit==========");
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        private void listClearBtn_Click(object sender, EventArgs e)
        {
            using (PassForm pf = new PassForm())
            {
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        listBox.Items.Clear();
                        core.WriteLog($"Removed all item from list by user.");
                    }
                    else
                    {
                        core.WriteLog($"Wrong password, when user try delete listBox items({pf.textBox1.Text})");
                    }
                }
            }
        }
        private void deleteSelectedBtn_Click(object sender, EventArgs e)
        {
            using (PassForm pf = new PassForm())
            {
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        while (listBox.SelectedItems.Count > 0)
                        {
                            core.WriteLog($"Removed selected files from program list(not original folder):{listBox.SelectedItems[0]}");
                            listBox.Items.Remove(listBox.SelectedItems[0]);
                        }
                    }
                    else
                    {
                        core.WriteLog($"Wrong password, when user try delete listBox item({pf.textBox1.Text})");
                    }
                }
            }
        }
        private void notifyIcon1_BalloonTipClosed(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(null, "Operation:\n" +
                "The program is able to monitor and log the changes made in the monitored txt and ini files.\n" +
                "If it finds a difference between two config files,\n" +
                "it will be logged and the user will be able to restore the old values ​​later in the event of an error.\n" +
                "Using the program:\n" +
                "- Add files: Add files to scan to the list.\n" +
                "- Delete selected: Deletes the selected items from the list.\n" +
                "- Delete all: Deletes all items in the list.\n" +
                "- Start / Stop: Start monitoring.\n" +
                "- Backup folder: Creates a backup of items added to the list (for checking).\n" +
                "As soon as a file change is detected, it logs the change and also modifies the contents of the config file.\n" +
                "If it is empty, but there is a file in files.txt,\n" +
                "it will ask you if you want to save the file to the backup folder(if not delete it from the list).\n" +
                "Periodically check the contents of the backup folder to prevent many configs from accumulating.\n" +
                "- Files.txt: Saves the path to items added to the list here.\n" +
                "- Log.txt: Here the program logs the results at runtime.", "Help window", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void openLogBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(core.logTxt))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = core.logTxt,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (PassForm pf = new PassForm())
            {
                if (pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        if (Directory.Exists(backupFolder))
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = backupFolder,
                                UseShellExecute = true,
                                Verb = "open"
                            });
                        }
                    }
                    else
                    {
                        core.WriteLog($"Wrong password, when user try delete listBox item({pf.textBox1.Text})");
                    }
                }
            }
        }
        private void addFilesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string[] files = openFileDialog1.FileNames;
                    AddSelectedFileToListBox(files);
                }
                core.SaveListBoxDataToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
