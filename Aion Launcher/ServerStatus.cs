using System;
using System.Net.Sockets;
using System.Net;

namespace Aion_Launcher
{
    class ServerStatus
    {
        public static bool isServerOnline(String server, int port)
        {
            TcpClient TcpScan = new TcpClient();
            var iar = TcpScan.BeginConnect(server, port, ConnectCallback, TcpScan);
            if (!iar.AsyncWaitHandle.WaitOne(700, false))
            {
                TcpScan.Close();
                return false;
            }
            else
            {
                TcpScan.Close();
                return true;
            }
        }

        public static int getOnline()
        {
            try
            {
                WebClient client = new WebClient();
                string s = client.DownloadString(Settings.onlineUrl);
                int i = int.Parse(s);
                return i;
            }
            catch
            {
                return 0;
            }
        }

        static void ConnectCallback(IAsyncResult iar)
        {

        }
    }
}
