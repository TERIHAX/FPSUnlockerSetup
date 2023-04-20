using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Drawing;
using Microsoft.Win32;

namespace FPSUnlockerSetup
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        string downloadLink = "https://github.com/TERIHAX/FPSUnlockerSetup/raw/main/ignorefile";
        string folderLocation = "";
        string unlockerSettings = "";
        bool installationStarted = false;
        bool installationSuccessful = false;

        private void ExitBtnEvent(object sender, EventArgs e) => Environment.Exit(0);

        public async Task<string> Install(bool autoCreateFolder)
        {
            if
            (
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\RBXFPSUnlocker"))
                ||
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RBXFPSUnlocker"))
                ||
                Directory.Exists("C:\\ProgramData\\RBXFPSUnlocker")
                ||
                Directory.Exists("C:\\ProgramData\\RBXFPSUnlockerBACKUP")
            )
            {
                if
                (
                    Directory.Exists(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\RBXFPSUnlocker")))
                    ||
                    Directory.Exists(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RBXFPSUnlocker")))
                    ||
                    Directory.Exists(File.ReadAllText("C:\\ProgramData\\RBXFPSUnlocker"))
                    ||
                    Directory.Exists(File.ReadAllText("C:\\ProgramData\\RBXFPSUnlockerBACKUP"))
                )
                {
                    MessageBox.Show("You already have an existing installation of RBXFPSUnlocker!");

                    return "There is already an existing installation of RBXFPSUnlocker!";
                }
            }

            try
            {
                using (WebClient client = new WebClient())
                {
                    if (autoCreateFolder)
                    {
                        Directory.CreateDirectory(folderLocation += "\\RBXFPSUnlocker");
                        folderLocation += "\\RBXFPSUnlocker";
                    }

                    if (File.Exists(folderLocation + "\\settings"))
                    {
                        unlockerSettings = File.ReadAllText(folderLocation + "\\settings");
                    }

                    if (Directory.Exists(folderLocation))
                    {
                        Directory.Delete(folderLocation, true);
                    }

                    Directory.CreateDirectory(folderLocation);
                    await client.DownloadFileTaskAsync(client.DownloadStringTaskAsync(downloadLink).Result, folderLocation + "\\RBXFPSUnlockerUpdater.exe");

                    if (unlockerSettings == "")
                    {
                        File.WriteAllText(folderLocation + "\\settings", "UnlockClient=true\r\nUnlockStudio=false\r\nFPSCapValues=[2.000000, 3.000000, 5.000000, 10.000000, 15.000000, 30.000000, 60.000000, 75.000000, 100.000000, 120.000000, 144.000000, 150.000000, 165.000000, 200.000000, 240.000000, 250.000000, 300.000000, 350.000000, 360.000000]\r\nFPSCapSelection=0\r\nFPSCap=0.000000\r\nCheckForUpdates=false\r\nNonBlockingErrors=true\r\nSilentErrors=true\r\nQuickStart=true\r\n");
                    }
                    else
                    {
                        File.WriteAllText(folderLocation + "\\settings", unlockerSettings);
                    }

                    if (File.Exists("FPSUnlockerUninstaller.exe"))
                    {
                        File.Delete("FPSUnlockerUninstaller.exe");
                    }

                    await client.DownloadFileTaskAsync(await client.DownloadStringTaskAsync(new Uri("https://github.com/TERIHAX/FPSUnlockerSetup/raw/main/ignorefile3")), "FPSUnlockerUninstaller.exe");
                    
                    RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true).CreateSubKey("RBXFPSUnlockerInstallation");
                    key.SetValue("DisplayName", "RBXFPSUnlocker");
                    key.SetValue("UninstallString", "FPSUnlockerUninstaller.exe");
                    key.SetValue("NoModify", 1);
                    key.SetValue("NoRepair", 1);
                }

                installationSuccessful = true;

                return "";
            }
            catch (Exception e)
            {
                return e.Message + ".";
            }
        }

        public Task Uninstall(bool showUninstallationMsg = false)
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
                    MessageBox.Show("You don't seem to have an existing RBXFPSUnlocker installation!\r\n\r\nOr your installation could be corrupted.\r\nHowever, no currently \"known\" corruptions were found.", "No Existing Installation!");
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

            if (showUninstallationMsg)
            {
                MessageBox.Show("RBX FPS Unlocker successfully uninstalled.", "Successfully Uninstalled!");
            }

            return Task.CompletedTask;
        }

        private void installBtn_Click(object sender, EventArgs e)
        {
            if (!Environment.Is64BitOperatingSystem)
            {
                windowsBitLbl.Visible = true;
                chooseLocInstallBtn.Visible = false;
            }

            installPage.Visible = true;
        }

        private void Setup_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            if (!installationStarted || installationSuccessful)
            {
                Environment.Exit(0);
            }
        }

        private void creditLbl_Click(object sender, EventArgs e) => Process.Start("https://github.com/TERIHAX/FPSUnlockerSetup");

        private void installBackBtn_Click(object sender, EventArgs e) => installPage.Visible = false;

        private void chooseLocInstallBtn_Click(object sender, EventArgs e)
        {
            installPage.Visible = false;
            locationPage.Visible = true;
        }

        private void locationChooseBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "Choose a folder to install RBXFPSUnlocker",
                ShowNewFolderButton = true
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    locationTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void noticeBtn_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(locationTextBox.Text))
            {
                locationPage.Visible = false;
                noticePage.Visible = true;
            }
            else
            {
                MessageBox.Show("The chosen folder does not exist!", "Folder Doesn't Exist!");
            }
        }

        private void autoCreateFolderChecked(object sender, EventArgs e)
        {
            if (autoCreateFolderCheckBox.Text == "/")
            {
                autoCreateFolderCheckBox.Text = " ";
            }
            else
            {
                autoCreateFolderCheckBox.Text = "/";
            }
        }

        private void backNoticeBtn_Click(object sender, EventArgs e)
        {
            noticePage.Visible = false;
            locationPage.Visible = true;
        }

        private void locationBackBtn_Click(object sender, EventArgs e)
        {
            locationPage.Visible = false;
            installPage.Visible = true;
        }

        private async void startInstallationBtn_Click(object sender, EventArgs e)
        {
            noticePage.Visible = false;
            installingPage.Visible = true;

            installationStarted = true;

            try
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker"));
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker"));

                if (autoCreateFolderCheckBox.Text == "/")
                {
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker\\installationpath"), folderLocation + "\\RBXFPSUnlocker");
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\RBXFPSUnlocker\\installationexepath"), folderLocation + "\\RBXFPSUnlocker\\FPSUnlockerUpdater.exe");
                }
                else
                {
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker\\installationpath"), folderLocation + "\\RBXFPSUnlocker");
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "\\RBXFPSUnlocker\\installationexepath"), folderLocation + "\\RBXFPSUnlocker\\FPSUnlockerUpdater.exe");
                }
            }
            catch
            {
                Directory.CreateDirectory("C:\\ProgramData\\RBXFPSUnlocker");
                Directory.CreateDirectory("C:\\ProgramData\\RBXFPSUnlockerBACKUP");

                if (autoCreateFolderCheckBox.Text == "/")
                {
                    File.WriteAllText("C:\\ProgramData\\RBXFPSUnlocker\\installationpath", folderLocation + "\\RBXFPSUnlocker");
                    File.WriteAllText("C:\\ProgramData\\RBXFPSUnlocker\\installationexepath", folderLocation + "\\RBXFPSUnlocker\\FPSUnlockerUpdater.exe");
                }
                else
                {
                    File.WriteAllText("C:\\ProgramData\\RBXFPSUnlockerBACKUP\\installationpath", folderLocation + "\\RBXFPSUnlocker");
                    File.WriteAllText("C:\\ProgramData\\RBXFPSUnlockerBACKUP\\installationexepath", folderLocation + "\\RBXFPSUnlocker\\FPSUnlockerUpdater.exe");
                }
            }

            await Install(autoCreateFolderCheckBox.Text == "/");
            if (installationSuccessful)
            {
                for (int i = progressBarInner.Width; i < 40; i++)
                {
                    try
                    {
                        await Task.Delay(5);
                        progressBarInner.Width += 2;
                    }
                    catch
                    {
                        continue;
                    }
                }

                for (int i = progressBarInner.Width; i < 95; i++)
                {
                    try
                    {
                        await Task.Delay(1);
                        progressBarInner.Width += 10;
                    }
                    catch
                    {
                        continue;
                    }
                }

                for (int i = progressBarInner.Width; i < 500; i++)
                {
                    try
                    {
                        await Task.Delay(1);
                        progressBarInner.Width += 1;
                    }
                    catch
                    {
                        continue;
                    }
                }

                await Task.Delay(500);
                installingPage.Visible = false;
                installationFinishedPage.Visible = true;

                installationStarted = false;
            }
        }

        private async void finishInstallationBtn_Click(object sender, EventArgs e)
        {
            if (desktopShortcutChkBox.Text == "/")
            {
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)new IWshRuntimeLibrary.WshShell().CreateShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RBX FPS Unlocker.lnk"));
                shortcut.TargetPath = folderLocation + "\\RBXFPSUnlockerUpdater.exe";
                shortcut.Save();
            }

            if (startMenuChkBox.Text == "/")
            {
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)new IWshRuntimeLibrary.WshShell().CreateShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "RBX FPS Unlocker.lnk"));
                shortcut.TargetPath = folderLocation + "\\RBXFPSUnlockerUpdater.exe";
                shortcut.Save();
            }

            if (runAfterExitChkBox.Text == "/")
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderLocation + "\\RBXFPSUnlockerUpdater.exe",
                    WorkingDirectory = folderLocation
                });

                await Task.Delay(500);
                Environment.Exit(0);
            }
        }

        private void desktopShortcutChecked(object sender, EventArgs e)
        {
            if (desktopShortcutChkBox.Text == "/")
            {
                desktopShortcutChkBox.Text = " ";
            }
            else
            {
                desktopShortcutChkBox.Text = "/";
            }
        }

        private void startMenuShortcutChecked(object sender, EventArgs e)
        {
            if (startMenuChkBox.Text == "/")
            {
                startMenuChkBox.Text = " ";
            }
            else
            {
                startMenuChkBox.Text = "/";
            }
        }

        private void runAfterExitChecked(object sender, EventArgs e)
        {
            if (runAfterExitChkBox.Text == "/")
            {
                runAfterExitChkBox.Text = " ";
            }
            else
            {
                runAfterExitChkBox.Text = "/";
            }
        }

        private async void repairBtn_Click(object sender, EventArgs e)
        {
            await Uninstall();

            if (!Environment.Is64BitOperatingSystem)
            {
                windowsBitLbl.Visible = true;
                chooseLocInstallBtn.Visible = false;
            }

            installPage.Visible = true;
        }

        private async void reinstallBtn_Click(object sender, EventArgs e)
        {
            await Uninstall(true);

            if (!Environment.Is64BitOperatingSystem)
            {
                windowsBitLbl.Visible = true;
                chooseLocInstallBtn.Visible = false;
            }

            installPage.Visible = true;
        }

        private async void removeBtn_Click(object sender, EventArgs e) => await Uninstall(true);
    }
}
