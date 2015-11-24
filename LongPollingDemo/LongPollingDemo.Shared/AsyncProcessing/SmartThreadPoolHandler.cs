using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;
using Timer = System.Threading.Timer;

namespace LongPolling.Shared
{
    public class SmartThreadPoolHandler<RequestType> : IDisposable
    {
        #region Private Members

        private static SmartThreadPool m_SmartThreadPool = null;
        private static System.Threading.Timer statisticsTimer;
        private Amib.Threading.Func<int> _getActiveThreads;
        private Amib.Threading.Func<int> _getInUseThreads;
        private Amib.Threading.Func<int> _getQueuedWorkItems;
        private Amib.Threading.Func<int> _getCompletedWorkItems;
        private bool m_Started;
        private bool m_Disposed;
        protected readonly object m_SyncRoot = new object();
        private const int _DefaultMinThreadCount = 3;
        #endregion Private Members


        #region Constructor
        protected SmartThreadPoolHandler()
        {
            try
            {
                InitializeLocalPerformanceCounters();
                System.Console.WriteLine("SmartThreadPool Handler constructor created");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("SmartThreadPool Handler constructor failed", ex);
                throw;
            }
        }
        #endregion Constructor


        #region Public methods
        public void Start(int MaxThreadCount, int minThreadCount = _DefaultMinThreadCount)
        {
            lock (m_SyncRoot)
            {
                if (m_Disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (m_Started)
                    throw new InvalidOperationException("SmartThreadPool already started.");

                STPStartInfo startInfo = new STPStartInfo();
                startInfo.MinWorkerThreads = minThreadCount;
                startInfo.MaxWorkerThreads = MaxThreadCount;
                startInfo.IdleTimeout = 20000; // After 20 seconds, a thread counts as an idle one
                startInfo.EnableLocalPerformanceCounters = true;
                startInfo.ThreadPoolName = "Smart Threadpool";
                m_SmartThreadPool = new SmartThreadPool(startInfo);
                m_SmartThreadPool.Start();
                statisticsTimer = new Timer(new TimerCallback(StatisticsTimer), null, 5000, 120000);
                m_Started = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_Started)
                    Stop();
                if (statisticsTimer != null)
                    statisticsTimer.Dispose();
            }

            m_Disposed = true;
        }
        protected virtual void OnProcess(RequestType request) { }

        #endregion

        #region Private methods

        private void InitializeLocalPerformanceCounters()
        {
            _getActiveThreads = () => Convert.ToInt32(m_SmartThreadPool.PerformanceCountersReader.ActiveThreads);
            _getInUseThreads = () => Convert.ToInt32(m_SmartThreadPool.PerformanceCountersReader.InUseThreads);
            _getQueuedWorkItems = () => Convert.ToInt32(m_SmartThreadPool.PerformanceCountersReader.WorkItemsQueued);
            _getCompletedWorkItems = () => Convert.ToInt32(m_SmartThreadPool.PerformanceCountersReader.WorkItemsProcessed);
        }

        private void StatisticsTimer(object obj)
        {

            //Thread.CurrentThread.Name = "SmartThreadPool Statistics";

            System.Console.WriteLine("########## Number Of Active Threads :{0} ", _getActiveThreads().ToString());
            System.Console.WriteLine("########## Number Of In Use Threads :{0} ", _getInUseThreads().ToString());
            System.Console.WriteLine("########## Work Item Queued : {0} ", _getQueuedWorkItems().ToString());
            System.Console.WriteLine("########## Number Of Complete Items: {0} ", _getCompletedWorkItems().ToString());
        }
        #endregion

        protected void Stop()
        {
            lock (m_SyncRoot)
            {
                if (m_Disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                if (!m_Started)
                    throw new InvalidOperationException("SmartThreadPool not started.");

                SmartThreadPool.Shutdown();

                m_Started = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            lock (m_SyncRoot)
            {
                if (m_Disposed)
                    return;

                Dispose(true);
            }
            System.Console.WriteLine("SmartThreadPool was Disposed.");
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets number of threads running in assemblage.
        /// </summary>
        public int ActiveThreadsCount
        {
            get
            {
                if (m_Disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return _getInUseThreads();

            }
        }
        public int AliveThreadsCount
        {
            get
            {
                if (m_Disposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return _getActiveThreads();
            }
        }
        /// <summary>
        /// Gets if threads in the assemblage have been started.
        /// </summary>
        public bool IsStarted
        {
            get
            {
                lock (m_SyncRoot)
                {
                    if (m_Disposed)
                        throw new ObjectDisposedException(GetType().FullName);

                    return m_Started;
                }
            }
        }
        /// <summary>
        /// Gets if instance of assemblage has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_Disposed;
                }
            }
        }
        ///<summary>
        /// SmartThreadPool, used to balance the request queues.
        /// </summary>
        public SmartThreadPool SmartThreadPool
        {
            get { return m_SmartThreadPool; }
        }


        #endregion


    }
}
