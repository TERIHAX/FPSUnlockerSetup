using System;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace FPSUnlockerUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RBXFPSUnlocker Updater by TERI (Tires#3415)";
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("Checking for updates...\r\nLoading...\r\nLaunching...");
            
            if (File.Exists("rbxfpsunlocker.exe"))
            {
                File.Delete("rbxfpsunlocker.exe");
            }

            if (File.Exists("rbxfpsunlocker-x64.zip"))
            {
                File.Delete("rbxfpsunlocker-x64.zip");
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(client.DownloadString(new Uri("https://github.com/TERIHAX/FPSUnlockerSetup/raw/main/ignorefile2")), "./rbxfpsunlocker-x64.zip");
            }

            if (File.Exists("rbxfpsunlocker-x64.zip"))
            {
                ZipFile.ExtractToDirectory("rbxfpsunlocker-x64.zip", Environment.CurrentDirectory);
            }

            Thread.Sleep(100);

            if (File.Exists("rbxfpsunlocker.exe"))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "rbxfpsunlocker.exe",
                    WorkingDirectory = Environment.CurrentDirectory
                });

                Console.WriteLine("Launched!\r\n\r\nRBXFPSUnlocker should now be in your system tray.");
            }

            Thread.Sleep(500);
            Environment.Exit(0);
        }
    }
}
