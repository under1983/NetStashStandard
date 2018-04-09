using NetStashStandard.Log;
using System;
using System.Threading;

namespace TestCore
{
    class Program
    {
        static void Main(string[] args)
        {
            NetStashLog log = new NetStashLog("172.168.2.102", 5356, "Testing Net 4.6.1 with DLL Net Standard", System.Diagnostics.Process.GetCurrentProcess().ProcessName, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion,true);

            log.Error("Testing", System.Reflection.MethodBase.GetCurrentMethod());

            Thread.Sleep(50);

            Console.ReadLine();
            log.Stop();
        }
    }
}
