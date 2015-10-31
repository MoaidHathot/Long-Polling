using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LongPolling
{
    public class TaskResultContext<V> : IResultContext<V>
    {
        private TaskCompletionSource<V> _completionSource;

        private object _syncLock = new object();

        public TaskResultContext()
        {
            _completionSource = new TaskCompletionSource<V>();
        }

        public void OnCompleted(Action continuation)
        {
            _completionSource.Task.ContinueWith((t) => continuation());
        }

        public bool IsCompleted
        {
            get { return _completionSource.Task.IsCompleted; }
        }

        public bool IsCancelled
        {
            get { return _completionSource.Task.IsCanceled; }
        }

        public V Result
        {
            get { return GetResult(); }
        }

        public V GetResult()
        {
            return _completionSource.Task.Result;
        }

        public bool SetResult(V result)
        {
            return _completionSource.TrySetResult(result);
        }

        public bool SetCancelled()
        {
            lock (_syncLock)
            {
                if (_completionSource.Task.IsCanceled)
                {
                    return false;
                }

                _completionSource.SetCanceled();

                return true;
            }
        }

        public void Wait()
        {
            _completionSource.Task.Wait();
        }

        public bool Wait(TimeSpan timeout, bool cancelIfTimeout = true)
        {
            if (!_completionSource.Task.Wait(timeout))
            {
                lock (_syncLock)
                {
                    if (!_completionSource.Task.IsCanceled && cancelIfTimeout)
                    {
                        SetCancelled();
                    }
                }

                return false;
            }

            return true;
        }

        public void Dispose()
        {
            if (null != _completionSource)
            {
                _completionSource.Task.Dispose();
                _completionSource = null;
            }
        }
    }
}
