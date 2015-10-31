using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Atomic = System.Threading.Interlocked;

namespace LongPolling.Shared
{
    public class RequestPersistancyConfiguration
    {
        private int _attemptsCount;
        private DateTime _creationDateTime = DateTime.Now;
        private DateTime? _firstExecutionDateTime;
        private DateTime? _lastExecutionDateTime;
        private DateTime? _lastFinishExecutionDateTime;

        public int? MaxAttempts { get; protected set; }
        public TimeSpan? Interval { get; protected set; }
        public TimeSpan? ExpirationPeriod { get; protected set; }
        public DateTime? ExpirationDate { get; protected set; }

        public int AttemptsCount { get { lock (_lock) { return _attemptsCount; } } }
        public DateTime CreationDateTime { get { lock (_lock) { return _creationDateTime; } } }
        public DateTime? FirstExecutionDateTime { get { lock (_lock) { return _firstExecutionDateTime; } } }
        public DateTime? LastExecutionDateTime { get { lock (_lock) { return _lastExecutionDateTime; } } }
        public DateTime? LastFinishedExecutionDateTime { get { lock (_lock) { return _lastFinishExecutionDateTime; } } }

        private object _lock = new object();

        public RequestPersistancyConfiguration()
        {
            Init(1);
        }

        public RequestPersistancyConfiguration(int maxAttempts)
            : this(maxAttempts, null, (TimeSpan?)null)
        {

        }

        public RequestPersistancyConfiguration(int maxAttempts, TimeSpan interval)
            : this(maxAttempts, interval, (TimeSpan?)null)
        {

        }

        public RequestPersistancyConfiguration(int? maxAttempts = null, TimeSpan? interval = null, TimeSpan? expirationPeriod = null)
        {
            DateTime? date = null;

            if (null != expirationPeriod)
                date = _creationDateTime + expirationPeriod;

            Init(maxAttempts, interval, expirationPeriod, date);
        }

        public RequestPersistancyConfiguration(int? maxAttempts = null, TimeSpan? interval = null, DateTime? expirationDate = null)
        {
            TimeSpan? span = null;

            if (null != expirationDate)
                span = expirationDate - _creationDateTime;

            if (span <= TimeSpan.Zero)
                span = null;

            Init(maxAttempts, interval, span, expirationDate);
        }

        private void Init(int? maxAttempts = null, TimeSpan? interval = null, TimeSpan? expirationPeriod = null, DateTime? expirationDate = null)
        {
            this.MaxAttempts = maxAttempts;
            this.Interval = interval;
            this.ExpirationPeriod = expirationPeriod;
            this.ExpirationDate = expirationDate;
        }

        /// <summary>
        /// Will register an execution attempt for the task. It will update the status accordingly.
        /// </summary>
        public void RegisterStartExecution()
        {
            lock (_lock)
            {
                var now = DateTime.Now;

                if (null == _firstExecutionDateTime)
                    _firstExecutionDateTime = now;

                _lastExecutionDateTime = now;

                ++_attemptsCount;
            }
        }

        public void RegisterEndExecution()
        {
            lock (_lock)
            {
                var now = DateTime.Now;

                _lastFinishExecutionDateTime = now;
            }
        }

        public bool IsFirstExecution
        {
            get
            {
                return Atomic.Equals(_attemptsCount, 0);
            }
        }

        /// <summary>
        /// Will determine by the configuration, if the task is valid for another execution
        /// </summary>
        public bool IsValidForExecution
        {
            get
            {
                lock (_lock)
                {
                    if (null != MaxAttempts && _attemptsCount >= MaxAttempts)
                        return false;

                    var now = DateTime.Now;

                    if (null != ExpirationDate && ExpirationDate < now)
                        return false;

                    //if (null != Interval && null != LastExecutionDateTime && now - LastExecutionDateTime < Interval)
                    //if (null != Interval && null != LastFinishedExecutionDateTime && now - LastFinishedExecutionDateTime < Interval)
                    //return false;

                    return true;
                }
            }
        }

        public bool IsValidForRescheduling
        {
            get
            {
                lock (_lock)
                {
                    if (null != MaxAttempts && _attemptsCount >= MaxAttempts)
                        return false;

                    var now = DateTime.Now;

                    if (null != ExpirationDate && ExpirationDate < now)
                        return false;

                    return true;
                }
            }
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}", this.MaxAttempts, this.Interval, this.ExpirationPeriod).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var casted = obj as RequestPersistancyConfiguration;

            if (null == obj || casted == null)
                return false;

            return casted.MaxAttempts == this.MaxAttempts && casted.Interval == this.Interval && casted.ExpirationPeriod == this.ExpirationPeriod;
        }

        public override string ToString()
        {
            return string.Format("AttemptsCount: '{0}', Interval: '{1}', ExpirationPeriod: '{2}', MaxAttempts: '{3}', ExpirationDate: '{4}', CreationDateTime: '{5}', FirstExecutionDateTime: '{6}', LastExecutionDateTime: '{7}', LastFinishedExecutionTime: '{8}'", AttemptsCount, Interval, ExpirationPeriod, MaxAttempts, ExpirationDate, CreationDateTime, FirstExecutionDateTime, LastExecutionDateTime, LastFinishedExecutionDateTime);
        }
    }
}
