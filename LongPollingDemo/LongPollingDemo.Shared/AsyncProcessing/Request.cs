using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LongPolling.Shared
{
    public abstract class Request : IAsyncResult, IDisposable
    {
        #region Variables
        private ManualResetEvent m_CompletionEvent;
        private bool m_Disposed;
        protected readonly object m_SyncRoot = new object();
        protected Request m_CancelRequest;
        private RequestPersistancyConfiguration m_PersistancyConfiguration;
        private bool m_CompletedSynchronously;
        private bool m_IsCompleted;
        private bool m_InProgress;
        private bool m_IsFailed;
        #endregion

        public Request()
            : this(new RequestPersistancyConfiguration())
        {

        }

        public Request(RequestPersistancyConfiguration configuration)
        {
            m_PersistancyConfiguration = configuration;
        }

        #region Public Methods
        public virtual Request GetCancelRequest(bool onDemand = false)
        {
            System.Console.WriteLine("Receiving Cancel request. (Cancel Request Type: '{0}')", m_CancelRequest.GetType().Name);
            return m_CancelRequest;
        }

        public void Execute()
        {
            InProgress = true;

            System.Console.WriteLine("Going to call Request.OnExecute (IsCompleted: '{0}', IsFailed: '{1}')", IsCompleted, IsFailed);
            lock (m_SyncRoot)
            {
                try
                {
                    if (m_Disposed)
                        return;

                    //if (IsCompleted)
                    //    return;

                    m_IsFailed = false;

                    if (!m_PersistancyConfiguration.IsFirstExecution)
                        ResetStatus();

                    m_PersistancyConfiguration.RegisterStartExecution();

                    OnExecute();
                }
                catch
                {
                    //lock (m_SyncRoot)
                    //{
                    IsFailed = true;
                    //}

                    throw;
                }
                finally
                {
                    //lock (m_SyncRoot)
                    //{
                    m_PersistancyConfiguration.RegisterEndExecution();

                    m_IsCompleted = true;
                    m_InProgress = false;

                    if (m_CompletionEvent != null)
                        m_CompletionEvent.Set();
                    //}
                }
            }
        }

        public void RetryAttemptsExhausted()
        {
            lock (m_SyncRoot)
            {
                try
                {
                    OnRetryAttemptsExhausted();

                    System.Console.WriteLine("Finished handling retry attempt exhaustion. Request: [{0}]", this.ToString());
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception while handling retry attempt exhaustion. Request: [{0}]. Exception: [{1}]", this.ToString(), ex);
                }
            }
        }
        #endregion

        #region Abstract Methods
        protected abstract void OnExecute();
        protected virtual void OnRetryAttemptsExhausted()
        {

        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            lock (m_SyncRoot)
            {
                if (m_Disposed)
                    return;

                if (m_CompletionEvent != null)
                {
                    m_CompletionEvent.Close();
                    m_CompletionEvent = null;
                }
                System.Console.WriteLine("Request was disposed. (Request Type: '{0}')", GetType().Name);
                m_Disposed = true;
            }
        }
        #endregion

        #region IAsyncResult Members
        public object AsyncState
        {
            get { return null; }
        }
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (m_CompletionEvent == null)
                {
                    lock (m_SyncRoot)
                    {
                        if (m_CompletionEvent == null)
                        {
                            m_CompletionEvent = new ManualResetEvent(false);
                            if (IsCompleted)
                            {
                                m_CompletionEvent.Set();
                            }
                        }
                    }
                }

                return m_CompletionEvent;
            }
        }
        //public bool CompletedSynchronously { get; set; }

        public bool CompletedSynchronously
        {
            get { lock (m_SyncRoot) { return m_CompletedSynchronously; } }
            set { lock (m_SyncRoot) { m_CompletedSynchronously = value; } }
        }

        //public bool IsCompleted { get; private set; }

        public bool IsCompleted
        {
            get { lock (m_SyncRoot) { return m_IsCompleted; } }
            private set { lock (m_SyncRoot) { m_IsCompleted = value; } }
        }

        //public bool InProgress{ get; private set; }
        public bool InProgress
        {
            get { lock (m_SyncRoot) { return m_InProgress; } }
            private set { lock (m_SyncRoot) { m_InProgress = value; } }
        }

        #endregion

        public bool IsFailed
        {
            get { lock (m_SyncRoot) { return m_IsFailed; } }
            set { lock (m_SyncRoot) { m_IsFailed = value; } }
        }

        public RequestPersistancyConfiguration PersistancyConfiguration
        {
            get { lock (m_SyncRoot) { return m_PersistancyConfiguration; } }
            private set { lock (m_SyncRoot) { m_PersistancyConfiguration = value; } }
        }

        #region Public Properties
        //public bool IsFailed { get; private set; }

        //public RequestPersistancyConfiguration PersistancyConfiguration { get; protected set; }
        #endregion

        #region Public Methods
        public void ResetStatus()
        {
            lock (m_SyncRoot)
            {
                //IsCompleted = false;
                //IsFailed    = false;

                m_IsCompleted = false;
                m_IsFailed = false;
            }
            System.Console.WriteLine("Request was Reset. (Request Type: '{0}')", GetType().Name);
        }
        public override string ToString()
        {
            return string.Format("Name: '{0}', IsCompleted: '{1}', IsFailed: '{2}', Persistancy: {3}", GetType().Name, IsCompleted, IsFailed, PersistancyConfiguration);
        }
        #endregion

        #region Protected Methods
        protected void OnRequestCanceled()
        {
            System.Console.WriteLine("OnRequestCanceled. (Request Type: '{0}')", GetType().Name);
            m_CancelRequest.Dispose();
            Dispose();
        }
        #endregion
    }
}
