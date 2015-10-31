using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NICE.Logging.Common;
using NICE.Logging.Server;

using Level = NICE.Logging.Common.LogLevel;

namespace LongPolling.Shared
{
    public class ServerLogAdapter : ILogger
    {
        private readonly ILog _Log;
        private bool _Disposed;
        private readonly string _Name;

        public ServerLogAdapter(string loggerName, string moduleName, string[] ignoreList)
        {
            var set = new HashSet<string>(ignoreList);
            set.Add(typeof(ServerLogAdapter).FullName);

            _Name = loggerName;

            _Log = ServerLog.CreateLogging(new ServerLogConfiguration(loggerName, moduleName, set.ToArray()));

            _Log.SetLogLevel(Level.Debug);
            _Log.SetMaxFileSize(1024 * 1024 * 100);
        }

        public void Fatal(string message)
        {
            _Log.Write(Level.Fatal, message);
        }

        public void Fatal(string message, Exception ex)
        {
            _Log.Write(Level.Fatal, ex, message);
        }

        public void Fatal(string formattedMessage, params object[] parameters)
        {
            _Log.Write(Level.Fatal, formattedMessage, parameters);
        }

        public void Fatal(Exception ex, string formattedMessage, params object[] parameters)
        {
            _Log.Write(Level.Fatal, ex, formattedMessage, parameters);
        }

        public void Error(string message)
        {
            _Log.Write(Level.Error, message);
        }

        public void Error(string message, Exception ex)
        {
            _Log.Write(Level.Error, ex, message);
        }

        public void Error(string formattedMessage, params object[] parameters)
        {
            _Log.Write(Level.Error, formattedMessage, parameters);
        }

        public void Error(Exception ex, string formattedMessage, params object[] parameters)
        {
            _Log.Write(Level.Error, ex, formattedMessage, parameters);
        }

        public void Warning(string message)
        {
            _Log.Write(Level.Warn, message);
        }

        public void Warning(string message, Exception ex)
        {
            //_Log.Write(Level.Warn, ex, message);
            _Log.Write(Level.Warn, message, ex);
        }

        public void Warning(string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Warn, formattedMessage, parameters);
        }

        public void Warning(Exception ex, string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Warn, ex, formattedMessage, parameters);
        }

        public void Info(string message)
        {

            _Log.Write(Level.Info, message);
        }

        public void Info(string message, Exception ex)
        {

            _Log.Write(Level.Info, ex, message);
        }

        public void Info(string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Info, formattedMessage, parameters);
        }

        public void Info(Exception ex, string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Info, ex, formattedMessage, parameters);
        }


        public void Debug(string message)
        {

            _Log.Write(Level.Debug, message);
        }

        public void Debug(string message, Exception ex)
        {

            _Log.Write(Level.Debug, ex, message);
        }

        public void Debug(string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Debug, formattedMessage, parameters);
        }

        public void Debug(Exception ex, string formattedMessage, params object[] parameters)
        {

            _Log.Write(Level.Debug, ex, formattedMessage, parameters);
        }

        public bool IsDebugEnabled
        {
            get { return _Log.isLogLevelSupported(Level.Debug); }
        }

        public bool IsErrorEnabled
        {
            get { return _Log.isLogLevelSupported(Level.Error); }
        }

        public bool IsFatalEnabled
        {
            get { return _Log.isLogLevelSupported(Level.Fatal); }
        }

        public bool IsInfoEnabled
        {
            get { return _Log.isLogLevelSupported(Level.Info); }
        }

        public bool IsWarnEnabled
        {
            get { return _Log.isLogLevelSupported(Level.Warn); }
        }

        public ILog ILog { get { return _Log; } }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_Disposed)
                return;

            if (disposing)
            {
                
                    _Log.CloseLogging();
            }

            _Disposed = true;
        }

        public string Name
        {
            get { return _Name; }
        }


        public LogLevel LogLevel
        {
            get
            {
                return _Log.GetLogLevel().ToITICLogLevel();
            }
            set
            {
                _Log.SetLogLevel(value.ToInfraLogLevel());
            }
        }
    }
}
