using System;
using System.IO;
using System.Windows.Forms;

namespace FPSUnlockerUninstaller
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string mainConfigFolderLoc = "";
            string backupConfigFolderLoc = "";

            try
            {
                mainConfigFolderLoc = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker\\installationpath"));
                backupConfigFolderLoc = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker\\installationpath"));
            }
            catch
            {
                try
                {
                    mainConfigFolderLoc = File.ReadAllText("C:\\ProgramData\\RBXFPSUnlocker\\installationpath");
                    backupConfigFolderLoc = File.ReadAllText("C:\\ProgramData\\RBXFPSUnlockerBACKUP\\installationpath");
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\r\n  You don't seem to have an existing RBXFPSUnlocker installation!\r\n\r\n  Or your installation could be corrupted.");

                    MessageBox.Show("You don't seem to have an existing RBXFPSUnlocker installation!\r\n\r\nOr your installation could be corrupted.\r\nHowever, no currently \"known\" corruptions were found.", "No Existing Installation!");
                    Environment.Exit(0);
                }
            }

            if (Directory.Exists(mainConfigFolderLoc))
            {
                Directory.Delete(mainConfigFolderLoc, true);
            }

            if (Directory.Exists(backupConfigFolderLoc))
            {
                Directory.Delete(backupConfigFolderLoc, true);
            }

            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker")))
            {
                Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker"), true);
            }

            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker")))
            {
                Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker"), true);
            }

            if (Directory.Exists("C:\\ProgramData\\RBXFPSUnlocker"))
            {
                Directory.Delete("C:\\ProgramData\\RBXFPSUnlocker");
            }

            if (Directory.Exists("C:\\ProgramData\\RBXFPSUnlockerBACKUP"))
            {
                Directory.Delete("C:\\ProgramData\\RBXFPSUnlockerBACKUP");
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "RBX FPS Unlocker.lnk")))
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "RBX FPS Unlocker.lnk"));
            }

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RBX FPS Unlocker.lnk")))
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RBX FPS Unlocker.lnk"));
            }

            MessageBox.Show("RBX FPS Unlocker successfully uninstalled.", "Successfully Uninstalled!");
        }
    }
}
