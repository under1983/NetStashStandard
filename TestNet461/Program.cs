using NetStashStandard.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestNet461
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            NetStashLog log = new NetStashLog("172.168.2.102", 5356, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion, "hola",false);

            log.Error("Testing", System.Reflection.MethodBase.GetCurrentMethod());

            Thread.Sleep(500);

            log.Stop();
        }
    }
}
