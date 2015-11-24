using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using LongPolling;
using LongPolling.Shared;
using LongPolling.Client.NotificationServiceReference;

namespace LongPolling.Client
{
    public class ClientProgram
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Starting Client with args: '{0}'", args.ToSingleLineStrin());

            var clientArgs = new ClientArgs(args);

            System.Console.WriteLine("Starting Client with ClientArgs: '{0}'", clientArgs);
            //System.Console.WriteLine("Starting ClientPrefix: '{0}', ClientsCount: '{1}', ClientReadingTimeout: '{2}', DelayBetweenClients: '{3}', ClientsRunTime: '{4}'", clientArgs.ClientsPrefix, clientArgs.ClientsCount, clientArgs.ClientReadingTimeout, clientArgs.DelayBetweenClients, clientArgs.ClientsRuntime);

            var clients = new List<LongPollingClient>(clientArgs.ClientsCount);

            var hostname = Dns.GetHostName();

            for (var index = 0; index < clientArgs.ClientsCount; ++index)
            {
                //var UserID = string.Format("({0})#{1}", clientArgs.ClientsPrefix, index);
                //var client = new Client(new User(UserID), clientArgs.ClientReadingTimeout, new NotificationServiceClient("NetTcpBinding_INotificationService"));

                var user = new User(hostname, clientArgs.SetIndex, index);

                LongPollingClient client;

                client = new LongPollingClient(user, clientArgs.ClientReadingTimeout, new NotificationServiceClient("NetTcpBinding_INotificationService"));

                client.Start();

                System.Console.WriteLine("Client with User: '{0}' is started", user);

                Thread.Sleep(clientArgs.DelayBetweenClients);
            }

            System.Console.WriteLine("ClientPrefix: '{0}', '{1}' clients were created and started", clientArgs.ClientsPrefix, clientArgs.ClientsCount);

            var now = DateTime.Now;

            var epsilon = TimeSpan.FromMinutes(5);

            var runtime = clientArgs.ClientsRuntime + epsilon;

            System.Console.WriteLine("ClientPrefix: '{0}' is running for '{1}', Expected stop: '{2}', Now: '{3}', epsilon: '{4}', runtime with epsilon: '{5}'", clientArgs.ClientsPrefix, clientArgs.ClientsRuntime, now + clientArgs.ClientsRuntime, now, epsilon, runtime);

            if (Task.WaitAll(clients.Select(c => c.Task).ToArray(), runtime))
            {
                System.Console.WriteLine("ClientPrefix: '{0}' finished waiting successfully for all '{1}' clients.", clientArgs.ClientsPrefix, clientArgs.ClientsCount);
            }
            else
            {
                System.Console.WriteLine("ClientsPrefix: '{0}' did not finish waiting for all '{1}' clients.", clientArgs.ClientsCount);
            }

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }

        class ClientArgs
        {
            public string ClientsPrefix { get; set; }
            public string HostName { get; set; }
            public int SetIndex { get; set; }
            public int ClientsCount { get; set; }
            public TimeSpan ClientReadingTimeout { get; set; }
            public TimeSpan DelayBetweenClients { get; set; }
            public TimeSpan ClientsRuntime { get; set; }
            public bool IsActiveMQ { get; set; }
            public string ActiveMQBrokerURL { get; set; }


            public ClientArgs(string[] args)
            {
                var index = 0;

                ClientsPrefix = TryGet(args, index++, Guid.NewGuid().ToString(), (raw) => raw);
                HostName = TryGet(args, index++, Dns.GetHostName(), (raw) => raw);
                SetIndex = TryGet(args, index++, -1, (raw) => int.Parse(raw));
                ClientsCount = TryGet(args, index++, 100, (raw) => int.Parse(raw));
                ClientReadingTimeout = TryGet(args, index++, TimeSpan.FromMinutes(2), (raw) => TimeSpan.Parse(raw));
                DelayBetweenClients = TryGet(args, index++, TimeSpan.Zero, (raw) => TimeSpan.Parse(raw));
                ClientsRuntime = TryGet(args, index++, TimeSpan.FromHours(2), (raw) => TimeSpan.Parse(raw));
                IsActiveMQ = TryGet(args, index++, false, (raw) => bool.Parse(raw));
                ActiveMQBrokerURL = TryGet(args, index++, "failover:(tcp://RtiScaleClient3:61616?wireFormat.tightEncodingEnabled=false)", (raw) => raw);
            }

            private T TryGet<T>(string[] args, int index, T defaultValue, Func<string, T> getFunc)
            {
                if (index >= args.Length)
                {
                    System.Console.WriteLine("'Arguments missing the '{0}' argument. Will use default: '{1}'", index, defaultValue);
                    return defaultValue;
                }

                var raw = args[index];

                try
                {
                    return getFunc(raw);
                }
                catch //(Exception ex)
                {
                    System.Console.WriteLine("Failed to parse '{0}'. Will return default: '{1}'", raw, defaultValue);

                    return defaultValue;
                }
            }

            public override string ToString()
            {
                return string.Format("ClientsPrefix: '{0}', HostName: '{1}', SetIndex: '{2}', ClientsCount: '{3}', ClientReadingtimeout: '{4}', DelayBetweenClients: '{5}', ClientsRuntime: '{6}', IsActiveMQ: '{7}', ActiveMQBrokerURL: '{8}'", 
                                    ClientsPrefix, HostName, SetIndex, ClientsCount, ClientReadingTimeout, DelayBetweenClients, ClientsRuntime, IsActiveMQ, ActiveMQBrokerURL);
            }
        }
    }
}
