using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Aion_Launcher
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form2 first = new Form2();
                DateTime end = DateTime.Now + TimeSpan.FromSeconds(5);
                first.Show();
                while (end > DateTime.Now)
                {
                    Application.DoEvents();
                }
                first.Close();
                first.Dispose();
                Application.Run(new Form1());
            }
        }

        public static string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        public static string delLastpattern(string s, char c)
        {
            string[] temp = s.Split(c);
            string output = string.Empty;

            for (int i = 0; i < (temp.Length - 1); i++)
            {
                output += temp[i] + "\\";
            }

            return output;
        }
    }
}
