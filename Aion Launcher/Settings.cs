using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aion_Launcher
{
    class Settings
    {
        public static int gameseverPort         = 7777;
        public static int loginserverPort       = 2106;

        public static bool onlineEnabled        = false;
        public static bool newsEnabled          = false;
        public static bool launchUpdateEnabled  = false;
        public static bool deleteFilesEnabled   = true;
        public static bool DNSconnectEnabled    = true;

        public static string[] allowedFiles = { "Aion Launcher.exe", "cc.ini", "Pub.key", "system.cfg", "SystemOptionGraphics.cfg" };

        public static string clientUrl = "https://sgfgaming.com/dist/aion_4.7.5-enu.exe.torrent";
        public static string launchFilesUrl     = "http://x.x.x.x/launch_false/";
        public static string onlineUrl          = "http://x.x.x.x/test/online.txt";
        public static string dnsName            = "aionserver.sgfgaming.com";

        public static string gameseverIP        = "192.168.122.10"; //148.251.232.44//127.0.0.1
        public static string loginserverIP      = "192.168.122.10";
    }
}
