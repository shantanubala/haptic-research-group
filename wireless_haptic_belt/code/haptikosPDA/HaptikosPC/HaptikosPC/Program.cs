using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Haptikos
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] // Single Thread Application
        static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}