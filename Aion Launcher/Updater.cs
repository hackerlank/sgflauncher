using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Aion_Launcher
{
    class Updater
    {
        string launchPath = Application.StartupPath;
        string[] lines;

        List<ClientFile> clientFiles = new List<ClientFile>();

        public UpdaterTemp temp;

        public void selfUpdate()
        {
            if (!Settings.launchUpdateEnabled)
                return;

            try
            {
                WebClient client = new WebClient();
                string actual = client.DownloadString(Settings.launchFilesUrl + "/version.txt");
                string current = Application.ProductVersion;

                if (!File.Exists("update.exe"))
                {
                    Uri ui = new Uri(Settings.launchFilesUrl + "/update.exe");
                    WebClient c = new WebClient();
                    c.DownloadFile(ui, launchPath + "\\update.exe");
                }
                if (!actual.Equals(current))
                {
                    Process.Start("update.exe");
                    Environment.Exit(0);
                }
            }
            catch { }
        }

        public void downloadHashes()
        {
            try
            {
                WebClient webClient = new WebClient();
                Uri ui = new Uri(Settings.clientUrl + "/hash.txt");
                webClient.DownloadFile(ui, launchPath + "\\hash.txt");

                lines = File.ReadAllLines(Application.StartupPath + "\\hash.txt");
                File.Delete(Application.StartupPath + "\\hash.txt");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Апдейт сервер недоступен. Подключитесь к интернету. Либо напишите на aionsuccess@gmail.com", "Внимание!");               
            }
        }

        public void update()
        {
            downloadHashes();

            if (lines == null)
            {
                temp.button_play.Enabled = true;
                temp.button_update.Enabled = true;
                return;
            }

            foreach (String line in lines)
            {
                string[] fileInfo = line.Split(':');
                clientFiles.Add(new ClientFile(fileInfo[0], fileInfo[1]));
            }

            checkFolders(clientFiles);

            foreach (ClientFile clientFile in clientFiles)
            {
                string path = (launchPath + "\\" + clientFile.Name);
                bool exists = System.IO.File.Exists(path);

                if (!exists || isUpdateRequired(path, clientFile.Hash))
                {
                    temp.filesToUpdate++;
                    temp.filesRemains++;
                }
            }

            foreach (ClientFile clientFile in clientFiles)
            {
                string path = (launchPath + "\\" + clientFile.Name);
                bool exists = System.IO.File.Exists(path);

                if (!exists || isUpdateRequired(path, clientFile.Hash))
                {
                    downloadFile(clientFile.Name);
                }
            }

            temp.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;

            if (temp.filesRemains == 0)
                temp.onUpdateEnd();

        }

        public void checkFolders(List<ClientFile> list)
        {
            foreach (ClientFile clientFile in list)
            {
                string filePath = (Application.StartupPath + "\\" + clientFile.Name);
                string[] patterns = filePath.Split('\\');
                int num = patterns.Length;

                if (num > 0)
                {
                    string path = Program.delLastpattern(filePath, '\\');
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                }
            }
        }

        public void downloadFile(string fileName)
        {
            try
            {
                WebClient web = new WebClient();
                Uri url = new Uri(Settings.clientUrl + fileName);
                web.DownloadFileCompleted += new AsyncCompletedEventHandler(temp.client_DownloadFileCompleted);
                web.DownloadFileAsync(url, launchPath + "\\" + fileName);
            }
            catch { }
        }

        public bool isUpdateRequired(string path, string hashcode)
        {
            FileInfo file = new FileInfo(path);
            string hash = Program.GetHashString(file.Length.ToString());

            if (!hash.Equals(hashcode))
                return true;
            else
                return false;

        }

    }

    class UpdaterTemp
    {
        public int filesToUpdate = 0;
        public int filesRemains = 0;

        public ProgressBar progressBar;
        public Label label;

        public Button button_play;
        public Button button_update;

        public UpdaterTemp(ProgressBar pb, Label label, Button button_play, Button button_update)
        {
            this.progressBar = pb;
            this.label = label;
            this.button_play = button_play;
            this.button_update = button_update;
        }

        public void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            filesRemains--;
            label.Text = "Файлов на скачку: " + filesRemains;

            if (filesRemains == 0)
            {
                onUpdateEnd();
            }

        }

        public void onUpdateEnd()
        {
            button_play.Enabled = true;
            button_update.Enabled = true;
            label.Text = "Готово можно играть!!!";
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar.Value = 100;
        }
    }
}
