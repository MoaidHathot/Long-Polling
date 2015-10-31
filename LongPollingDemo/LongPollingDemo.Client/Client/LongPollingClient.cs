using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LongPolling.Shared;
using LongPolling.Client.NotificationServiceReference;

namespace LongPolling.Client
{
    public class LongPollingClient : IDisposable
    {
        private NotificationServiceReference.INotificationService _service;

        private User _user;
        private Task _task;

        private TimeSpan _timeout;

        private CancellationTokenSource _cancellationToken;

        public Task Task { get { return _task; } }

        public LongPollingClient(User identifications, TimeSpan timeout, NotificationServiceReference.INotificationService service)
        {
            _timeout = timeout;

            _user = identifications;
            _service = service;
        }

        public virtual void Start()
        {
            _cancellationToken = new CancellationTokenSource();

            _task = Task.Factory.StartNew((state) =>
            {
                _service.Register(_user);

                Log.Instance.Info("User: '{0}' is registered successfully.", _user);

                while (!_cancellationToken.IsCancellationRequested)
                {
                    var notifications = GetNotifications(_timeout);

                    var receivedDateTime = DateTime.UtcNow;

                    var notificationsCount = null == notifications ? 0 : notifications.Count();

                    Log.Instance.Debug("User: '{0}' is processing '{1}' notifications.", _user, notificationsCount);

                    ProcessNotifications(notifications);

                    ReportNotifications(receivedDateTime, notifications);
                }

            }, null, _cancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

            _task.ContinueWith((t) => Log.Instance.Warning("User: '{0}' has a faulted task. Exception: '{1}'", _user, t.Exception.Flatten()), TaskContinuationOptions.OnlyOnFaulted);
        }

        protected virtual void ProcessNotifications(IEnumerable<Notification> notifications)
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Format("User: '{0}' received the following notifications: ", _user));

            if (null != notifications)
            {
                foreach (var notification in notifications)
                {
                    builder.AppendLine(string.Format("User: '{0}', notification: '{1}'", _user, notification));
                }

                Log.Instance.Debug(builder.ToString());
            }
        }

        protected virtual void ReportNotifications(DateTime receivedtime, IEnumerable<Notification> notifications)
        {
            if (null != notifications)
            {
                var builder = new StringBuilder();

                builder.AppendLine(string.Format("User: '{0}' reports the following notifications: ", _user));

                foreach (var notification in notifications)
                {
                    builder.AppendLine(string.Format("User: '{0}', delta: '{1}', now: '{2}', receivedUTC: '{3}', notificationUTC: '{4}'", _user, receivedtime - notification.TimeStamp, receivedtime, notification.TimeStamp, notification));
                }

                Log.Instance.Debug(builder.ToString());
            }
        }

        protected virtual IEnumerable<Notification> GetNotifications(TimeSpan timeout)
        {
            if (TimeSpan.Zero != timeout)
            {
                return _service.GetNotificationsUserTimeSpan(new GetNotificationsUserTimeSpanRequest(_user, timeout)).GetNotificationsUserTimeSpanResult;
            }
            else
            {
                return _service.GetNotifications(new GetNotificationsRequest(_user)).GetNotificationsResult;
            }
        }

        public virtual void Stop()
        {
            try
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    _cancellationToken.Cancel();
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Info("User: '{0}' had exception while stopping. Exception: '{1}'", _user, ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _task)
                {
                    try
                    {
                        if (!_cancellationToken.IsCancellationRequested)
                        {
                            Stop();
                        }

                        if (_task.Wait(_timeout))
                        {
                            Log.Instance.Debug("User: '{0}' finished waiting '{1}' successfully for the task to dispose.", _user, _timeout);
                        }
                        else
                        {
                            Log.Instance.Debug("User: '{0}' failed to wait '{1}' for the task to dispose.", _user, _timeout);
                        }

                        _task.Dispose();
                    }
                    catch (AggregateException ex)
                    {
                        Log.Instance.Warning("User: '{0}' received an exception during task disposing. Exception: '{1}'", _user, ex.Flatten());
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Warning("User: '{0}' received an exception during task disposing. Exception: '{1}'", _user, ex);
                    }

                    _task = null;
                }

                if (null != _cancellationToken)
                {
                    _cancellationToken.Dispose();
                    _cancellationToken = null;
                }
            }
        }
    }
}
