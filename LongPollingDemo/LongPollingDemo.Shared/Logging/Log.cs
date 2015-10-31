using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    public class Log : ILogger
    {
        #region Constants
        private const string LOG_MESSAGE_PREFIX = "---";
        #endregion

        #region Variables
        private static readonly ILogger m_LogInstance = new Log();
        private static readonly ILogger m_AppLog = new ServerLogAdapter(System.Diagnostics.Process.GetCurrentProcess().ProcessName, "LongPolling", new string[] { typeof(Log).FullName });

        private bool _Disposed;
        #endregion

        #region Constructor
        /// <summary>
        /// Constractor is protected!
        /// Part of SingleTone implementation.
        /// </summary>
        protected Log()
        {

        }
        #endregion

        #region Public properties
        public static ILogger Instance
        {
            get
            {
                return m_LogInstance;
            }
        }


        // Use these properties only to run some external
        // logic depends on the log level.
        // For example build complicated string for writing 
        // to the log file iteratively.
        #region ILogger
        public bool IsDebugEnabled
        {
            get { return m_AppLog.IsDebugEnabled; }
        }
        public bool IsErrorEnabled
        {
            get { return m_AppLog.IsErrorEnabled; }
        }
        public bool IsFatalEnabled
        {
            get { return m_AppLog.IsFatalEnabled; }
        }
        public bool IsInfoEnabled
        {
            get { return m_AppLog.IsInfoEnabled; }
        }
        public bool IsWarnEnabled
        {
            get { return m_AppLog.IsWarnEnabled; }
        }
        #endregion

        #endregion

        #region Public Methods

        #region Fatal
        public void Fatal(string message)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Fatal(LOG_MESSAGE_PREFIX + message);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Fatal method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Fatal(string message, Exception ex)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Fatal(LOG_MESSAGE_PREFIX + message, ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Fatal method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Fatal(string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Fatal(LOG_MESSAGE_PREFIX + formattedMessage, parameters);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Fatal method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Fatal(Exception ex, string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                if (m_AppLog.IsFatalEnabled)
                    m_AppLog.Fatal(LOG_MESSAGE_PREFIX + string.Format(formattedMessage, parameters), ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Fatal method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        #endregion

        #region Error
        public void Error(string message)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Error(LOG_MESSAGE_PREFIX + message);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Error method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Error(string message, Exception ex)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Error(LOG_MESSAGE_PREFIX + message, ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Error method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Error(string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Error(LOG_MESSAGE_PREFIX + formattedMessage, parameters);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Error method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Error(Exception ex, string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                if (m_AppLog.IsErrorEnabled)
                    m_AppLog.Error(LOG_MESSAGE_PREFIX + string.Format(formattedMessage, parameters), ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Error method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        #endregion

        #region Warning
        public void Warning(string message)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Warning(LOG_MESSAGE_PREFIX + message);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Warning method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Warning(string message, Exception ex)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Warning(LOG_MESSAGE_PREFIX + message, ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Warning method of ApplicationLog. ### AppLog Error ###", logEx);
                }
                catch
                { }
            }
        }
        public void Warning(string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Warning(LOG_MESSAGE_PREFIX + formattedMessage, parameters);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Warning method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Warning(Exception ex, string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                if (m_AppLog.IsWarnEnabled)
                    m_AppLog.Warning(LOG_MESSAGE_PREFIX + string.Format(formattedMessage, parameters), ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Warning method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        #endregion

        #region Information
        public void Info(string message)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Info(LOG_MESSAGE_PREFIX + message);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Info method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Info(string message, Exception ex)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Info(LOG_MESSAGE_PREFIX + message, ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Info method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Info(string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Info(LOG_MESSAGE_PREFIX + formattedMessage, parameters);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Info method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Info(Exception ex, string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                if (m_AppLog.IsInfoEnabled)
                    m_AppLog.Info(LOG_MESSAGE_PREFIX + string.Format(formattedMessage, parameters), ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Info method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }

        #endregion

        #region Debug
        public void Debug(string message)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Debug(LOG_MESSAGE_PREFIX + message);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Debug method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Debug(string message, Exception ex)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Debug(LOG_MESSAGE_PREFIX + message, ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Debug method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Debug(string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                m_AppLog.Debug(LOG_MESSAGE_PREFIX + formattedMessage, parameters);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Debug method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        public void Debug(Exception ex, string formattedMessage, params object[] parameters)
        {
            // Wraps the call to ApplicationLog.
            // Every exception will be catch and will not affect the code that called the logger method.
            try
            {
                if (m_AppLog.IsDebugEnabled)
                    m_AppLog.Debug(LOG_MESSAGE_PREFIX + string.Format(formattedMessage, parameters), ex);
            }
            catch (Exception logEx)
            {
                try
                {
                    m_AppLog.Warning("Exception thrown while calling to the Debug method of ApplicationLog.", logEx);
                }
                catch
                { }
            }
        }
        #endregion

        #endregion

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
                m_AppLog.Dispose();

                _Disposed = true;
            }
        }


        public string Name
        {
            get { return m_AppLog.Name; }
        }


        public LogLevel LogLevel
        {
            get
            {
                return m_AppLog.LogLevel;
            }
            set
            {
                m_AppLog.LogLevel = value;
            }
        }
    }
}
