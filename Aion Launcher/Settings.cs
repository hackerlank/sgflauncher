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
        public static bool newsEnabled          = true;
        public static bool launchUpdateEnabled  = false;
        public static bool deleteFilesEnabled   = true;
        public static bool DNSconnectEnabled    = true;

        public static string[] allowedFiles = { "Aion Launcher.exe", "cc.ini", "Pub.key", "system.cfg", "SystemOptionGraphics.cfg" };

        public static string clientUrl = "https://sgfgaming.com/dist/aion_4.7.5-enu.exe.torrent";
        public static string launchFilesUrl = "https://aionserver.sgfgaming.com/launcher/";
        public static string onlineUrl = "https://aionserver.sgfgaming.com/launcher/online.txt";
        public static string dnsName            = "aionserver.sgfgaming.com";

        public static string gameseverIP        = "23.248.146.126"; //148.251.232.44//127.0.0.1
        public static string loginserverIP      = "23.248.146.126";
    }
}
