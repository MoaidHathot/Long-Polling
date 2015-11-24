using System.Threading;
using System.Net;
using System.Diagnostics;
using LongPolling.Shared;
using LongPolling.Client.Runner;

namespace LongPolling.Client.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostname = Dns.GetHostName();

            for (var instance = 0; instance < Settings.Default.InstancesCount; ++instance)
            {
                var prefix = string.Format("{0}_{1}", hostname, instance);

                System.Console.WriteLine("Starting prefix '{0}' by path: '{1}'", prefix, Settings.Default.ClientFilePath);

                var settings = Settings.Default;

                Process.Start(Settings.Default.ClientFilePath, string.Format("{0} {1} {2} {3} {4} {5} {6}", prefix, hostname, instance, settings.ClientsCount, settings.ClientsReadingTimeout,
                                                                                                                    settings.DelayBetweenClients, settings.ClientsRuntime));

                System.Console.WriteLine("prefix '{0}' was started.", prefix);

                System.Console.WriteLine("Delaying instance starting by '{0}'", Settings.Default.DelayBetweenInstances);

                Thread.Sleep(Settings.Default.DelayBetweenInstances);
            }

            System.Console.WriteLine("Finish starting instances");
        }
    }
}
