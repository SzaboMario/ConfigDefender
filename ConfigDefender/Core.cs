using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ConfigDefender
{
    class Core
    {
        StreamWriter sw;
        private bool run;
        bool startStop = false;
        internal readonly string logTxt = "Log.txt";
        Form1 form;
        Thread thread;
        public Core(Form form1)
        {
            form = (Form1)form1;
        }
        internal List<string[]> GetFileData(string filesTxT)
        {
            List<string[]> fileList = new List<string[]>();
            if (FilesTextFileIsExists(filesTxT))
            {
                string[] lineArr = null;
                using (StreamReader sr = new StreamReader(filesTxT))
                {
                    WriteLog($"Loading {filesTxT}:");
                    while (!sr.EndOfStream)
                    {
                        lineArr = sr.ReadLine().Split('=');
                        fileList.Add(lineArr);
                    }
                    sr.Close();
                }
            }
            if (fileList.Count == 0) { WriteLog($"{filesTxT} is empty."); }
            WriteLog($"Loading {filesTxT} is complete.");
            return fileList;
        }
        internal string[] GetFileNamesFromFolder(string folderName)
        {
            string[] files = null;
            if (BackupFolderIsExist(folderName))
            {
                files = Directory.GetFiles(form.backupFolder, "*.*", SearchOption.AllDirectories);//backupfolderból fájlok beolvasása
            }
            else
            {
                files = new string[0];
            }
            if (files.Length > 0) WriteLog($"Files from {folderName} loaded.");
            else WriteLog($"{folderName} folder is empty.");
            return files;
        }
        internal bool CompareFileNames(List<string[]> fileDataList, string[] fileNamesFromFolderArr)
        {
            string filename = "";
            string fullFileName = "";
            List<string> deletedData = new List<string>();
            List<string> tmp = new List<string>();
            try
            {
                foreach (string it in fileNamesFromFolderArr)
                {
                    tmp.Add(it.Substring(it.LastIndexOf("\\") + 1));
                }
                foreach (string[] item in fileDataList)
                {
                    fullFileName = item[0];
                    filename = fullFileName.Substring(fullFileName.LastIndexOf("\\") + 1);
                    if (!tmp.Contains(filename))//ha a lista nem tartalmazza a backup file-t
                    {
                        DialogResult res = MessageBox.Show(null, $"The Backup folder does not contain the {fullFileName}.\nDo you want to save to the Backup folder?\n(If not, this data will be deleted from the files.txt).)", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);//ha nem található fájl a backup mappában
                        if (res == DialogResult.Yes)
                        {
                            if (File.Exists(fullFileName))
                            {
                                File.Copy(fullFileName, form.backupFolder + "\\" + filename, true);
                                WriteLog($"{fullFileName} file copied to Backup folder by user.");
                            }
                            else
                            {
                                WriteLog($"{fullFileName} file not found!");
                            }
                        }
                        else if (res == DialogResult.No)
                        {
                            deletedData.Add(fullFileName);
                            WriteLog($"{fullFileName}  deleted from files.txt by user.");
                        }
                    }
                }
                if (deletedData.Count > 0)
                {
                    foreach (string item in deletedData)
                    {
                        fileDataList.RemoveAll(x => x[0] == item);
                    }
                    SetDataToListBox(fileDataList);
                    SaveListBoxDataToFile();
                }
            }
            catch (Exception ex)
            {
                WriteLog("CompareFilesAndData: " + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
            return true;
        }
        internal void SetDataToListBox(List<string[]> fileDataList)
        {
            form.listBox.Items.Clear();
            if (fileDataList != null && fileDataList.Count > 0)
            {
                foreach (var item in fileDataList)
                {
                    form.listBox.Items.Add(item[0]);
                }
                WriteLog($"ListBox data loading in OK.");
            }
        }
        internal bool FilesTextFileIsExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                WriteLog($"The {fileName} does not exist -> Create new one.");
                File.Create(fileName).Close();
                WriteLog($"{fileName} created!");
            }
            WriteLog($"The {fileName} is exists");
            return true;
        }
        internal bool BackupFolderIsExist(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                WriteLog($"The {folderName} does not exist -> Create new one.");
                Directory.CreateDirectory(folderName);
                WriteLog($"{folderName} created!");
            }
            WriteLog($"The {folderName} is exists");
            return true;
        }
        internal void WriteLog(string data)
        {
            try
            {
                using (StreamWriter wr = new StreamWriter(logTxt, true))
                {
                    wr.WriteLine($"- {DateTime.Now}: {data}");
                    wr.Flush();
                    wr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("WriteLog: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
        internal void CheckerThread()
        {
            bool res = false;
            int notFindCounter = 0;
            List<string> firstFileList = new List<string>(), secFileList = new List<string>();
            while (run)
            {
                if (form.listBox.Items.Count > 0)
                {
                    foreach (string item in form.listBox.Items)
                    {
                        firstFileList.Clear();
                        secFileList.Clear();
                        if (!File.Exists(item))
                        {
                            WriteLog($"Not find file: {item}");
                            notFindCounter++;
                            if (notFindCounter == 20)
                            {
                                MessageBox.Show($"Not find file: {item}");
                                notFindCounter = 0;
                            }
                        }
                        else
                        {
                            firstFileList = GetConfigDataFromFile(item);
                            secFileList = GetConfigDataFromFile(form.backupFolder + "\\" + item.Substring(3));
                            if (firstFileList.Count > 0 && secFileList.Count > 0 && firstFileList.Count == secFileList.Count)
                            {
                                res = CompareFileDatas(item, firstFileList, form.backupFolder + "\\" + item.Substring(3), secFileList);
                            }
                            else MessageBox.Show("Comparable files not same line numbers.\r\n" +
                              $"File: {item}\r\n" +
                              $"Checked file line count: {firstFileList.Count}\r\n" +
                              $"Backup file line count: { secFileList.Count}");
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }
        private bool CompareFileDatas(string fFileName, List<string> firstFile, string sFileName, List<string> secFile)
        {
            if (DateTime.Parse(File.GetLastWriteTime(fFileName).ToString()) != DateTime.Parse(File.GetLastWriteTime(sFileName).ToString()))
            {
                Thread.Sleep(1000);
                for (int first = 0; first < firstFile.Count; first++)
                {
                    if (firstFile[first] != secFile[first])
                    {
                        UpdateBackupFile(sFileName, firstFile[first], secFile[first]);
                        WriteLog($"\n" +
                            $"------------------------\r\n" +
                            $"Not same data in {fFileName}:\r\n" +
                            $"Checked file data: -> {firstFile[first]}\r\n" +
                            $"Backup file data:  -> {secFile[first]}\r\n" +
                            $"Old param({secFile[first]}) in {sFileName} updated to {firstFile[first]}\r\n" +
                            $"------------------------");
                    }
                }
                return false;
            }
            return true;
        }
        private void UpdateBackupFile(string fileName, string oldParam, string newParam)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == newParam)
                    {
                        lines[i] = oldParam;
                    }
                }
                System.IO.File.WriteAllLines(fileName, lines);
            }
            catch (Exception ex)
            {
                WriteLog("UpdateBackupFile: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
        internal List<string> GetConfigDataFromFile(string fileName)
        {
            string allData;
            List<string> tmp = new List<string>();
            try
            {
                if (File.Exists(fileName))
                {
                    using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
                    {
                        while (!sr.EndOfStream)
                        {
                            tmp.Add(sr.ReadLine());
                        }
                        sr.Close();
                    }
                }
                else
                {
                    DialogResult res = MessageBox.Show(null, $"Not find {fileName} file!", "ERROR", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                WriteLog("GetConfigDataFromFile: " + ex.Message + "\n" + ex.StackTrace);
            }
            return tmp;
        }
        internal void StartStop()
        {
            using (PassForm pf = new PassForm())
            {
                if (form.listBox.Items.Count > 0 && pf.ShowDialog() == DialogResult.OK)
                {
                    if (pf.textBox1.Text == "vision")
                    {
                        if (!startStop)
                        {
                            startStop = !startStop;
                            thread = new Thread(new ThreadStart(CheckerThread));
                            thread.IsBackground = true;
                            run = true;
                            thread.Start();
                            WriteLog("Started");
                            form.startBtn.Text = "Started";
                            form.startBtn.BackColor = Color.LightGreen;
                            form.addFolderBtn.Enabled = false;
                            form.listClearBtn.Enabled = false;
                            form.deleteSelectedBtn.Enabled = false;
                        }
                        else
                        {
                            startStop = !startStop;
                            run = false;
                            WriteLog("Stopped");
                            form.startBtn.Text = "Stopped";
                            form.startBtn.BackColor = Color.Tomato;
                            form.addFolderBtn.Enabled = true;
                            form.listClearBtn.Enabled = true;
                            form.deleteSelectedBtn.Enabled = true;
                        }
                    }
                    else
                    {
                        WriteLog($"Wrong password, when user try delete listBox({pf.textBox1.Text})");
                    }
                }
            }
        }
        internal void SaveListBoxDataToFile()
        {
            try
            {
                if (form.listBox.Items.Count == 0)
                {
                    try
                    {
                        File.Delete(form.filesTxT);
                        FileStream fs = File.Create(form.filesTxT);
                        fs.Close();
                        WriteLog("files.txt clear OK.");
                    }
                    catch (Exception ex)
                    {
                        WriteLog("SaveListBoxDataToFile: " + ex.Message + "\n" + ex.StackTrace);
                    }
                }
                else
                {
                    using (StreamWriter sr = new StreamWriter(form.filesTxT, false))
                    {
                        foreach (string item in form.listBox.Items)
                        {
                            if (item != "")
                            {
                                string folderTree = item.Substring(3, item.LastIndexOf("\\") - 3);
                                string fileName = item.Substring(item.LastIndexOf("\\") + 1);
                                sr.WriteLine($"{item}={File.GetLastWriteTime(item)}");
                                WriteLog($"Saved from program: {item}={File.GetLastWriteTime(item)} to {form.filesTxT}");
                                if (!Directory.Exists(form.backupFolder + "\\" + folderTree))
                                {
                                    Directory.CreateDirectory(form.backupFolder + "\\" + folderTree);
                                }
                                if (File.Exists(item)) File.Copy(item, form.backupFolder + "\\" + folderTree + "\\" + fileName, true);
                                else { WriteLog($"Not find file: {item}"); form.listBox.Items.Remove(item); }
                            }
                        }
                        sr.Flush();
                        sr.Close();
                        WriteLog("File write to " + form.backupFolder + " complete.");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("SaveListBoxDataToFile: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
