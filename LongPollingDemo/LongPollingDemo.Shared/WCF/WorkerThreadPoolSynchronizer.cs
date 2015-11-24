using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LongPolling.Shared
{
    public class WorkerThreadPoolSynchronizer : SynchronizationContext
    {
        static RequestProcessor _threadPool = null;

        static WorkerThreadPoolSynchronizer()
        {
            _threadPool = new RequestProcessor("Notification Processor");

            var max = 6000;
            var min = 1000;

            System.Console.WriteLine("Max LongPooling threads: '{0}'", max);
            System.Console.WriteLine("Min LongPooling threads: '{0}'", min);

            _threadPool.Start(max, min);

            //_threadPool.Start(7000, 3000);
            //_threadPool.Start(500, 300);
        }
        public override void Post(SendOrPostCallback d, object state)
        {
            // WCF almost always uses Post
            //ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
            _threadPool.BeginInvoke(new ActionRequest(() =>
            {
                new WaitCallback(d)(state);

            }));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            // Only the peer channel in WCF uses Send
            d(state);
        }
    }
}
