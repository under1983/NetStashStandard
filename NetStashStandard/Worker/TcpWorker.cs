using NetStashStandard.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetStashStandard.Worker
{
    public static class TcpWorker
    {
        
        static string currentapp = string.Empty;
        static string currentappversion = string.Empty;
        static string user = string.Empty;
        static string logstashIp = string.Empty;
        static int logstashPort = -1;

        static object _lock = new object();
        static bool isRunning = false;
        static Task run;

        static bool stopCalled = false;


        public static void Initialize(string logstashAddressIp, int logstashAddressPort, string currentApp, string currentAppVersion, string User)
        {
            if (string.IsNullOrWhiteSpace(logstashAddressIp))
                throw new ArgumentNullException("logstashAddressIp");

            logstashIp = logstashAddressIp;
            logstashPort = logstashAddressPort;
            user = User;
            currentapp = currentApp;
            currentappversion = currentAppVersion;
            Run();
        }

        public static void Run()
        {
            if (stopCalled) return;

            if (run == null || run.Status != TaskStatus.Running)
            {
                run = Task.Factory.StartNew(() =>
                {
                    lock (_lock)
                        isRunning = true;

                    while (isRunning)
                    {
                        try
                        {
                            Runner();
                        }
                        catch (Exception ex)
                        {
                            NetStashLog log = new NetStashLog(logstashIp, logstashPort, user, currentapp, currentappversion, Log.NetStashLog.TypeNet);
                            log.InternalError("Logstash communication error: " + ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
                        }
                    }
                });
            }
        }

        internal static void Restart()
        {
            lock (_lock)
                isRunning = true;

            stopCalled = false;

            Run();
        }

        internal static void Stop()
        {
            lock (_lock)
                isRunning = false;

            stopCalled = true;

            if (run != null) run.Wait();
        }

        private static void Runner()
        {
            Storage.Proxy.LogProxy proxy = new Storage.Proxy.LogProxy();
            Dictionary<long, string> evs;

            lock (_lock)
            {
                evs = proxy.GetList();
                if (evs.Count <= 0)
                {
                    isRunning = false;
                    return;
                }
            }

            Send(evs, UpdateEntry);
        }

        private static void UpdateEntry(long id)
        {
            Storage.Proxy.LogProxy proxy = new Storage.Proxy.LogProxy();
            proxy.Delete(id);
        }

        private static void Send(Dictionary<long, string> evs, Action<long> after)
        {
            using (TcpClient client = new TcpClient(logstashIp, logstashPort))
            using (StreamWriter writer = new StreamWriter(client.GetStream()))
            {
                foreach (KeyValuePair<long, string> ev in evs)
                {
                    writer.WriteLine(ev.Value.Replace(Environment.NewLine, "@($NL$)@"));
                    after(ev.Key);
                }
            }
        }
    }
}
