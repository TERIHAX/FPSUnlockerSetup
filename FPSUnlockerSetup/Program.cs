using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FPSUnlockerSetup
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Setup setup = new Setup();            
            foreach (GroupBox box in setup.Controls.OfType<GroupBox>())
            {
                box.Location = new Point(9, 5);

                if (box.Name != "setupPage")
                {
                    box.Visible = false;
                }
            }

            Application.Run(setup);
        }
    }
}
