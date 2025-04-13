using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using testDocking;

namespace DockSample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        public static string ProjectDirectory { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && Directory.Exists(args[0]))
                ProjectDirectory = Path.GetFullPath(args[0]);
            else
            {
                MessageBox.Show("A non existent or invalid path was detected while startup. Defaulting to the startup path of the application","SystemForge: invalid path",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                ProjectDirectory = Path.GetFullPath(Application.StartupPath);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new card());
        }
    }
}