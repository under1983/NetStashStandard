using NetStashStandard.Log;
using System;
using System.Threading;

namespace TestCore
{
    class Program
    {
        static void Main(string[] args)
        {
            NetStashLog log = new NetStashLog("172.168.2.102", 5356, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion, "hola", true);

            log.Error("Testing", System.Reflection.MethodBase.GetCurrentMethod());

            Thread.Sleep(50);

            Console.ReadLine();
            log.Stop();
        }
    }
}
