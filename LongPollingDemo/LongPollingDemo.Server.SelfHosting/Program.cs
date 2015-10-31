using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LongPolling.Host;
using LongPolling;
using LongPolling.Server;
using LongPolling.Server.External;
using LongPolling.Server.Manual;
using LongPolling.Server.SelfHosting;
using LongPolling.Shared;
using System.ServiceModel;

namespace LongPolling.Server.SelfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = null;

            try
            {
                PrintThreadsInformation();

                host = new ServiceHost(new ExternalNotificationService());

                var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behaviour.InstanceContextMode = InstanceContextMode.Single;

                foreach (var endPoint in host.Description.Endpoints)
                {
                    endPoint.Binding.CloseTimeout = TimeSpan.FromHours(2);
                    endPoint.Binding.OpenTimeout = TimeSpan.FromHours(2);
                    endPoint.Binding.ReceiveTimeout = TimeSpan.FromHours(2);
                    endPoint.Binding.SendTimeout = TimeSpan.FromHours(2);
                }

                host.Open();

                Log.Instance.Info("Service is ready...");

                Console.WriteLine("Press enter to stop service.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                if (null != host)
                {
                    host.Close();
                }
            }
            
            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }

        static void PrintThreadsInformation()
        {
            var minWorker = -1;
            var minCompletion = -1;
            ThreadPool.GetMinThreads(out  minWorker, out minCompletion);

            var maxWorker = -1;
            var maxCompletion = -1;
            ThreadPool.GetMaxThreads(out maxWorker, out maxCompletion);

            var availableWorker = -1;
            var availableCompletion = -1;
            ThreadPool.GetAvailableThreads(out availableWorker, out availableCompletion);

            Log.Instance.Debug("####################");
            Log.Instance.Debug("MinWorker: '{0}', MinCompletion: '{1}'", minWorker, minCompletion);
            Log.Instance.Debug("MaxWorker: '{0}', MaxCompletion: '{1}'", maxWorker, maxCompletion);
            Log.Instance.Debug("AvailableWorker: '{0}', AvailableCompletion: '{1}'", availableWorker, availableCompletion);
            Log.Instance.Debug("####################{0}", Environment.NewLine);
        }
    }
}
