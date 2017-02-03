using System;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Security;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

//Сокеты
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Timers;

//импорты для почты
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.NetworkInformation;






namespace Aion_Launcher
{
    public partial class Form1 : Form
    {

        string dir = Application.StartupPath;

        NewsParser newsParser = new NewsParser();
        Updater updater = new Updater();
        Findcheats findcheats = new Findcheats();

        public Form1()
        {
            InitializeComponent();
            onLoad();
            //апдейт клиента
            //updateClient();
            //System.Windows.Forms.MessageBox.Show("Началось обновление клиента. Ожидайте...", "Внимание!");


        }
        //на загрузку формы делаем
        void onLoad()
        {
            Thread t = new Thread(updater.selfUpdate);
            t.Start();
            getServerStatus();
            // if (Properties.Settings.Default.autoUpdate)
            writeiptofile();
            GetMACAddress();
            ping_server();

        }


        void updateClient()
        {
            button_play.Enabled = false;
            button_play.Enabled = false;
            updater.temp = new UpdaterTemp(progressBar1, labelDownloadFile, button_play, button_play);
            updater.update();
        }

        //описание статуса сервера
        void getServerStatus()
        {
            if (ServerStatus.isServerOnline(Settings.loginserverIP, Settings.loginserverPort))
            {
                label_LoginStatus.ForeColor = Color.GreenYellow;
                label_LoginStatus.Text = ("Online");
            }
            else
            {
                label_LoginStatus.ForeColor = Color.Red;
                label_LoginStatus.Text = ("Offline");
            }

            if (ServerStatus.isServerOnline(Settings.gameseverIP, Settings.gameseverPort))
            {
                label_GameStatus.ForeColor = Color.GreenYellow;
                label_GameStatus.Text = ("Online");
            }
            else
            {
                label_GameStatus.ForeColor = Color.Red;
                label_GameStatus.Text = ("Offline");
            }





        }



        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        //кнопка играть (или старт)
        private void button_play_Click(object sender, EventArgs e)
        {
            //проиграть звук при нажатии
            playClickSound();
            //удалить ненужные файлы
            //deleteFiles();
            //апдейт клиента
            updateClient();
            //удаляем процессы ненужные
            deletecheatsprocesses();
            //открываем сокеты
            //startsocket();




            bool exists = System.IO.File.Exists(dir + "\\bin32\\aion.bin");

            if (!exists)
            {
                System.Windows.Forms.MessageBox.Show("bin32\\aion.bin не найден. Поместите лаунчер в папку с игрой.", "ошибка");
                Close();
            }
            else
            {
                string connectIP;
                string Arguments = String.Empty;
                string log = Properties.Settings.Default.login;
                string pass = Properties.Settings.Default.pass;

                if (Settings.DNSconnectEnabled)
                    connectIP = System.Net.Dns.GetHostEntry(Settings.dnsName).AddressList[0].ToString();
                else
                    connectIP = Settings.loginserverIP;

                if (log == String.Empty || pass == String.Empty)
                    Arguments = "\\bin32\\aion.bin -ip:" + connectIP + " -port:" + Settings.loginserverPort + " -cc:7  -noweb -nowebshop -nokicks -nobs -ncg -ls -charnamemenu -megaphone";
                else
                    Arguments = "\\bin32\\aion.bin -ip:" + connectIP + " -port:" + Settings.loginserverPort + " -cc:7 -account:" + log + "-password:" + pass + "  -noweb -nowebshop -nokicks -nobs -ncg -ls -charnamemenu -megaphone";

                //STARTUPINFO si = new STARTUPINFO();
               // PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
               // CreateProcess(dir + "\\bin32\\aion.bin", Arguments, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref si, out pi);

                if (!Properties.Settings.Default.savePass)
                {
                    Properties.Settings.Default.pass = String.Empty;

                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }

            }
        }

        //играть звук по клику на кнопку
        void playClickSound()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.Stream = Properties.Resources.click;
            sp.Play();
        }


       //удаление читов на компе ssleay32.dll(хомяк)




       //удаление процессов читов
       private void deletecheatsprocesses() 
       {
           List<string> name = new List<string>
           //процесс, который нужно убить
           { "LightHack", "Aion F2P LiteHack", "aiHACKon", "libeay32", "phx", "acp", "wpf", "walker", "wss", "hlapex", "Clicker",
             "avk", "wpf", "N/A", "(UNVERIFIED)", "UNVERIFIED", "hack", "cheat", "L2px", "LookOut", "AionScript", "FlightPath", "Annimation", "Lion", "Radar",
             "RuBot", "AngelBot", "Packet", "NoAnim", "SpeedHack", "Speed", "inject"};
          
           System.Diagnostics.Process[] etc = System.Diagnostics.Process.GetProcesses();//получим процессы
           foreach (System.Diagnostics.Process anti in etc)//обойдем каждый процесс
           {
               try {
               foreach (string s in name)
               {
                   if (anti.ProcessName.ToLower().Contains(s.ToLower())) //найдем нужный и убьем
                   {
                     //ЕСЛИ ЧИТ ИДЕНТИФИЦИРОВАН ТО: // TODO нужно сделать какойто делей на запись
                     //записываем процессы в файлик
                     //CreateList();
                     //получаем мас и записываем
                     //GetMACAddress();
                     //получаем IP и Имя ПК и записываем в файл
                     //writeiptofile();
                     //потом убиваем процесс
                     anti.Kill();
                     name.Remove(s);
                     //сначала отсылаем лог файл на почту
                     //sendlogmail();
                     // Specify that the text can display mnemonic characters.
                     label10.UseMnemonic = true;
                     label10.ForeColor = Color.Red;
                     label10.Text = "STATUS: CHEATER";
                     killaion();

                   }
               }
                   }
                 catch (System.InvalidOperationException)
                 {
                  //MessageBox.Show("Закрытие читов завершино. Лог сохранен и может быть применен в спорных разбирательствах.", "Вы читер!!!");
                 }
                  catch (System.ComponentModel.Win32Exception) { }
               
           }
        }

       void killaion()
       {
           string target_name = "AION.bin";
           System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
           try
           {
               System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == target_name);
               target_proc.Kill();
           }
           catch (InvalidOperationException)
           {
               MessageBox.Show("Процесс " + target_name + "уже завершен!", "Внимание!");

           }
           MessageBox.Show( "Айон завершен! Так как вы читер.", "Внимание!");
           this.Close();
           
       }
    




        //удаление файлов по клику на кнопку играть (сверху добавлен на клик)
        void deleteFiles()
        {
            if (!Settings.deleteFilesEnabled)
                return;

            string[] files = Directory.GetFiles(dir, "*.*");
            string[] allowed = Settings.allowedFiles;

            foreach (String file in files)
            {
                bool b = false;
                foreach (String allow in allowed)
                {
                    if (file.Contains(allow))
                        b = true;
                }

                if (b != true)
                    File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }

        //кнопка настройки
        private void button_settings_Click(object sender, EventArgs e)
        {
            playClickSound();

            //Hide();
            FormSettings f = new FormSettings();
            f.Show();
            //f.Show();

        }



        [DllImport("kernel32.dll")]
        static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
                                bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
                                string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
        // нет кнопки апдейта клиента так как он должен ето делать сам
        private void button_update_Click(object sender, EventArgs e)
        {
            playClickSound();
            updateClient();
        }


        private void label3_Click(object sender, EventArgs e)
        {
            playClickSound();
            Process.Start("http://aion.mmotop.ru/servers/19456");
        }


        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label_GameServer_Click(object sender, EventArgs e)
        {

        }

        private void labelDownloadFile_Click(object sender, EventArgs e)
        {

        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label_GameStatus_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void label_LoginServer_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        //клик на картинку античит
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
            //MessageBox.Show("Античит активирован. Разработчик @yayaya - @Andrey, M. По вопросам e-mail: aionsccess@gmail.com", "Античит"); 
        }
        //Кнопка обновления онлайн / оффлайн сервера
        private void label1_Click_1(object sender, EventArgs e)
        {
            getServerStatus();
            playClickSound();
        }
        //таймер который проверяет процессы на читы каждые 30 сек
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            //удаляем процесы по таймеру
            deletecheatsprocesses();


        }
        //таймер на 20 минут отправка лога на почту.
        private void timer3_Tick(object sender, EventArgs e)
        {
            //отправка почты с логом
            sendlogmail();
            


        }

        //отправка на фтп лог файла
        void sendlogmail() {
            FileInfo fileInf = new FileInfo(@"C:\Processes.txt");
            string uri = "ftp://" + "148.251.232.44" + "/" + fileInf.Name;
            FtpWebRequest reqFTP;
            // Создаем объект FtpWebRequest
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + "148.251.232.44" + "/" + fileInf.Name + "-" + label2.Text + "-" + label5.Text));
            // Учетная запись
            reqFTP.Credentials = new NetworkCredential("aionlog1", "123456");
            reqFTP.KeepAlive = false;
            // Задаем команду на закачку
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Тип передачи файла
            reqFTP.UseBinary = true;
            // Сообщаем серверу о размере файла
            reqFTP.ContentLength = fileInf.Length;
            // Буффер в 2 кбайт
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
             // Файловый поток
            FileStream fs = fileInf.OpenRead();
            try
            {
               Stream strm = reqFTP.GetRequestStream();
               // Читаем из потока по 2 кбайт
                contentLen = fs.Read(buff, 0, buffLength);
                // Пока файл не кончится
                while (contentLen != 0)
                {
                     strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Закрываем потоки
                strm.Close();
                fs.Close();
            }
           catch (Exception ex)
            {
 
                MessageBox.Show(ex.Message, "Ошибка");
 
            }
 
        }


        //записываем процессы в файлик который потом нужно отослать на почту.
        private static void CreateList()
        {
            try
            {
                Process[] processInfo = Process.GetProcesses();

                Stream fs = new FileStream(@"C:\Processes.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    foreach (Process p in processInfo)
                    {
                        string processString = string.Format("Process name: {0,-30} Process ID: {1,-15}",
                                                             p.ProcessName, p.Id);
                        try
                        {
                            sw.WriteLine(processString);
                        }
                        catch (System.IO.IOException)
                        {
                        }

                    }
                }
                fs.Close();
            }
            catch (System.IO.IOException){ }
            }

        //метод получения IP и Имя компа
        void writeiptofile()
        {
            // для работы нужно импортировать пространство имен System.Net
            // using System.Net;

            // получаем хост
            string myHost = System.Net.Dns.GetHostName();
            label4.Text = myHost;

            // получаем IP-адрес хоста
            string myIP = System.Net.Dns.GetHostEntry(myHost).AddressList[0].ToString();            
            label2.Text = myIP;

            try { 
            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Processes.txt", true);
            writer.WriteLine("IP of user: " + myIP + " PC-name of user: " + myHost);
            writer.Close();
            }
            catch (System.IO.IOException) { }
        }
        // ip
        private void label2_Click(object sender, EventArgs e)
        {

        }
        // namr of computer
        private void label4_Click(object sender, EventArgs e)
        {

        }
        //MAC
        private void label5_Click(object sender, EventArgs e)
        {

        }
        //Метод получения МАС и дописывание его последней строкой в файл
        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                    //отправляем мас в лейбл
                    label5.Text = sMacAddress;
                    try
                    {
                        System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Processes.txt", true);
                        writer.WriteLine("МАС of user: " + sMacAddress);
                        writer.Close();
                    }
                    catch (System.IO.IOException) { }
                    
                }
            } 
            return sMacAddress;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //получаем мас и записываем
            GetMACAddress();
            //получаем IP и Имя ПК и записываем в файл
            writeiptofile();
            //записываем процессы в файлик
            CreateList();
        }

        void timer5_Tick(object sender, EventArgs e)
        {
            ping_server();
        }

         void ping_server()
        {
            Ping ping = new Ping();
            int i = 0;
            string n = "Хорошый пинг";
            string m = "Средний пинг";
            string b = "Плохой пинг";
            while (i < 10)
            {
                PingReply pingReply = ping.Send("127.0.0.1");
                //Console.WriteLine(pingReply.RoundtripTime); //время ответа
                //Console.WriteLine(pingReply.Status);        //статус
                //Console.WriteLine(pingReply.Address);       //IP
                //Thread.Sleep(500);
                i++;
                if (pingReply.RoundtripTime < 60)
                {
                    label11.UseMnemonic = true;
                    label11.ForeColor = Color.GreenYellow;
                    label11.Text = n;
                }
                if (pingReply.RoundtripTime > 61 )
                {
                    label12.UseMnemonic = true;
                    label12.ForeColor = Color.OrangeRed;
                    label12.Text = m;
                }
                if (pingReply.RoundtripTime >= 400)
                {
                    label14.UseMnemonic = true;
                    label14.ForeColor = Color.Red;
                    label14.Text = b;
                }

            }
        }

         private void label_LoginStatus_Click(object sender, EventArgs e)
         {

         }
         //не задействованный таймер
         private void timer6_Tick(object sender, EventArgs e)
         {

         }

         private void label9_Click(object sender, EventArgs e)
         {

         }

         private void button2_Click(object sender, EventArgs e)
         {
             playClickSound();
             Process.Start("http://reg70.tk/");
         }


         private void  startsocket()
         {
             Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //Создаем основной сокет
             IPAddress ipAddress = null; //IP-адресс
             IPEndPoint Addr = null; //конечная точка(IP и порт)

                
                ipAddress = Dns.GetHostEntry("127.0.0.1").AddressList[0];
                Addr = new IPEndPoint(ipAddress, 2106); //"localhost" = 127.0.0.1
                s.Connect(Addr); //Коннектимся к срверу
                while (true) //Вечная истина :)
                {
                    byte[] buffer = new byte[656545];
                    s.Send(buffer); //Отправляем
                    s.Close(); //Закрываем сокет
                }


         }

         private void button3_Click(object sender, EventArgs e)
         {
             this.Close();

         }



        }
}
