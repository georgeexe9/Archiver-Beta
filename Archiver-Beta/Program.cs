using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archiver_Beta
{
    internal static class Program
    {
        /// <summary>
        /// =========================RECEIPT ARCHIVER BETA==============================
        /// ----------------![THE MAIN ENTRY POINT FOR THE APPLICATION]!----------------
        /// :Project by George Lavchanski 217knr - KN1:
        /// :Dedicated to Slav:
        /// Please, read full documentation about the project!
        /// Written in: C# (.NET Framework, Windows Form Applications)
        /// Database:SQL Server
        /// Icons pack  Source: https://icons8.com/icons/ultraviolet
        /// georgeexe GitHub: https://github.com/georgeexe9
        /// </summary>
        [STAThread]
        static void Main()

        {  
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HomeForm());
            


        }
    }
}
