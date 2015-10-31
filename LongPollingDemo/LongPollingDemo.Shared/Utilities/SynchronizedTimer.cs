using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace LongPolling.Shared.Utilities
{
    public sealed class SynchronizedTimer : IDisposable
    {
        private object _syncLock = new object();
        private object _timerSyncLock = new object();
        private Timer _timer;

        private TimeSpan _dueTime;
        private TimeSpan _interval;

        private Action _action;


        public SynchronizedTimer(Action action, TimeSpan dueTime, TimeSpan interval)
        {
            _dueTime = dueTime;
            _interval = interval;

            if (null == action)
            {
                action = () => { };
            }

            _action = action;
        }

        public SynchronizedTimer(TimerCallback callback, TimeSpan dueTime, TimeSpan interval)
            : this( () => callback(null), dueTime, interval)
        {

        }

        public void Start()
        {
            lock (_syncLock)
            {
                if (null == _timer)
                {
                    _timer = new Timer(OnInterval, null, _dueTime, _interval);
                }
            }
        }

        public void Change(TimeSpan dueTime, TimeSpan interval)
        {
            lock (_syncLock)
            {
                _timer.Change(dueTime, interval);
            }
        }

        public void Stop()
        {
            lock (_syncLock)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
                _timer = null;
            }
        }

        private void OnInterval(object state)
        {
            bool entered = false;

            try
            {
                if (Monitor.TryEnter(_timerSyncLock))
                {
                    entered = true;

                    _action();
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
            finally
            {
                if (entered)
                {
                    Monitor.Exit(_timerSyncLock);
                }
            }
        }

        public void Dispose()
        {
            if (null != _timer)
            {
                Stop();
            }
        }
    }
}
