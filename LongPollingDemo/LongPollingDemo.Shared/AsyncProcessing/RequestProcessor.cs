using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    public class RequestProcessor : SmartThreadPoolHandler<Request>
    {
        #region Variables
        private readonly string m_RequestorName;
        private long m_RequestsRecevied;
        private long m_RequestsProcessed;

        #endregion

        #region Constructors
        public RequestProcessor(string name = "")
        {
            m_RequestorName = GetType().Name + (string.IsNullOrWhiteSpace(name) ? string.Empty : " - " + name);

            System.Console.WriteLine("RequestProcessor was created.");
        }
        #endregion

        #region Public methods
        ///// <summary>
        ///// Processes request in synchronous manner.
        ///// </summary>
        ///// <param name="request">Request that contains user code to be executed.</param>
        //public void SynchronousInvoke(Request request)
        //{
        //    BeginInvoke(request);
        //    EndInvoke(request);

        //    System.Console.WriteLine("SynchronousInvoke was completed. (Request: '{0}')", request);
        //}
        /// <summary>
        /// Processes request in asynchronous manner.
        /// </summary>
        /// <param name="request">Request to execute.</param>
        public void BeginInvoke(Request request)
        {
            lock (m_SyncRoot)
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(m_RequestorName);

                if (!IsStarted)
                    throw new InvalidOperationException(string.Format("'{0}' is not started.", m_RequestorName));

                // The "++" operation will not be an atomic operation in 32Bit systems.
                //++m_RequestsRecevied;
                System.Threading.Interlocked.Increment(ref m_RequestsRecevied);

                //m_Queue.Enqueue(request);
                SmartThreadPool.QueueWorkItem(OnProcess, request);
            }

            System.Console.WriteLine("'{0}': Request is queued for further processing. (Request: '{1}')", m_RequestorName, request);
        }
        ///// <summary>
        ///// Waits for request started by BeginProcessRequest to be completed.
        ///// </summary>
        ///// <remarks>Do not synchronie this method. The call to waitHandle.WaitOne() method
        ///// should be out of lock.</remarks>
        ///// <param name="request">Object returned by BeginInvoke.</param>
        //public void EndInvoke(Request request)
        //{
        //    if (IsDisposed)
        //        throw new ObjectDisposedException(m_RequestorName);

        //    if (!IsStarted)
        //        throw new InvalidOperationException(string.Format("'{0}' is not started.", m_RequestorName));


        //    request.AsyncWaitHandle.WaitOne();
        //    request.CompletedSynchronously = true;

        //    //SmartThreadPool.QueueWorkItem(OnProcess);
        //    System.Console.WriteLine("EndInvoke cal was completed. (Request: '{0}')", request);
        //}
        #endregion

        #region Public Properties
        public int RequestsCount
        {
            get
            {
                lock (m_SyncRoot)
                {
                    if (IsDisposed)
                        throw new ObjectDisposedException(GetType().FullName);

                    return (int)SmartThreadPool.PerformanceCountersReader.WorkItemsQueued;
                }
            }
        }
        #endregion

        #region Protected Methods
        protected override void OnProcess(Request request)
        {
            if (null == request)
            {
                System.Console.WriteLine("The request is null. The request will be ignored.");
                return;
            }

            try
            {
                ProcessRequest(request);
                ProcessResult(request);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Request processing failed. (Request: '{0}'). Exception: [{1}]", request, ex);
            }
            finally
            {
                System.Threading.Interlocked.Increment(ref m_RequestsProcessed);
            }
        }

        protected virtual void ProcessResult(Request request)
        {
            System.Console.WriteLine("Handling the request result. Request: [{0}]", request);

            if (!request.IsCompleted)
            {
                System.Console.WriteLine("A request that is expected to be completed, is not completed yet. Exception: [{0}]", request);
                return;
            }

            if (!request.IsFailed)
            {
                System.Console.WriteLine("A request was completed successfully. (Request: '{0}')", request);
            }
            else
            {
                System.Console.WriteLine("A request was failed. Will be handled. (Request: '{0}')", request);

                HandleFailedRequest(request);
            }
        }

        protected virtual void HandleFailedRequest(Request request)
        {
            System.Console.WriteLine("Will not attempt to handle the request: [{0}]", request);
        }

        //protected void FireEvent(EventHandler<RequestExecutionEventArgs> group, RequestExecutionEventArgs args)
        //{
        //    var handlers = group;

        //    if (null != handlers)
        //    {
        //        handlers(this, args);
        //    }
        //}

        protected virtual void ProcessRequest(Request request)
        {
            try
            {
                System.Console.WriteLine("Executing the request: [{0}]", request);

                request.Execute();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Request executing failed. (Request: '{0}'). Exception: [{1}]", request, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (IsStarted)
                    Stop();
            }

            base.Dispose(disposing);
        }
        #endregion

        #region IRequestProcessorLoadBalancing Implementation
        public long RequestsRecevied
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_RequestsRecevied;
                }
            }
        }

        public long RequestProcessed
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_RequestsProcessed;
                }
            }
        }
        #endregion IRequestProcessorLoadBalancing Implementation
    }
}
