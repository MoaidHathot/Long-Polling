using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    public interface ILogger : IDisposable
    {
        #region Methods
        #region Fatal
        void Fatal(string message);
        void Fatal(string message, Exception ex);
        void Fatal(string formattedMessage, params object[] parameters);
        void Fatal(Exception ex, string formattedMessage, params object[] parameters);
        #endregion

        #region Error
        void Error(string message);
        void Error(string message, Exception ex);
        void Error(string formattedMessage, params object[] parameters);
        void Error(Exception ex, string formattedMessage, params object[] parameters);
        #endregion

        #region Warning
        void Warning(string message);
        void Warning(string message, Exception ex);
        void Warning(string formattedMessage, params object[] parameters);
        void Warning(Exception ex, string formattedMessage, params object[] parameters);
        #endregion

        #region Information
        void Info(string message);
        void Info(string message, Exception ex);
        void Info(string formattedMessage, params object[] parameters);
        void Info(Exception ex, string formattedMessage, params object[] parameters);
        #endregion

        #region Debug
        void Debug(string message);
        void Debug(string message, Exception ex);
        void Debug(string formattedMessage, params object[] parameters);
        void Debug(Exception ex, string formattedMessage, params object[] parameters);
        #endregion
        #endregion

        #region Properties
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        string Name { get; }

        LogLevel LogLevel { get; set; }
        #endregion
    }
}
