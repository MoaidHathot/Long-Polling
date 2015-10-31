using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LongPolling;
using LongPolling.Shared;
using LongPolling.Shared.Utilities;

namespace LongPolling.Server
{
    public abstract class NotificationsCenterBase : IDisposable
    {
        private OrderedDictionary _users;
        private SynchronizedTimer _timer;

        private int _eventsPerInterval;

        private object _syncLock = new object();

        public NotificationsCenterBase(TimeSpan dueTime, TimeSpan interval, int eventsPerInterval)
        {
            _users = new OrderedDictionary();

            _timer = new SynchronizedTimer(OnTimertick, dueTime, interval);

            _eventsPerInterval = eventsPerInterval;

            Start();
        }

        protected virtual void Start()
        {
            _timer.Start();
        }

        protected  virtual void Stop()
        {
            _timer.Stop();
        }

        protected virtual void OnTimertick()
        {
            DispatchNotifications();
        }

        private void DispatchNotifications()
        {
            User[] users;
            List<User> notifyUsers;

            lock (_syncLock)
            {
                var count = _users.Count;
                users = new User[count];

                _users.Keys.CopyTo(users, 0);

                notifyUsers = users.Take(_eventsPerInterval).ToList();

                notifyUsers.ForEach(u => { _users.Remove(u); _users[u] = u; });
            }

            Log.Instance.Info("Dispatching '{0}' notifications", notifyUsers.Count);

            DispatchNotifications(notifyUsers);

            Log.Instance.Debug("'{0}' notifications were dispatched...", notifyUsers.Count);
        }

        protected abstract void DispatchNotifications(List<User> users);

        public void Register(User user)
        {
            lock (_syncLock)
            {
                RegisterUser(user);
                _users[user.ID] = user;
            }
        }

        protected abstract void RegisterUser(User user);

        public void Unregister(User user)
        {
            lock (_syncLock)
            {
                UnregisterUser(user);
                _users.Remove(user);
            }
        }

       protected abstract void UnregisterUser(User user);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _timer)
                {
                    Stop();
                    _timer.Dispose();
                    _timer = null;
                }

                if (null != _users)
                {
                    _users.Clear();
                    _users = null;
                }
            }
        }
    }
}
