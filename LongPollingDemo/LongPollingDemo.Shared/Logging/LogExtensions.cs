using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Level = NICE.Logging.Common.LogLevel;

namespace LongPolling.Shared
{
    public static class LogExtensions
    {
        public static Level ToInfraLogLevel(this LogLevel level)
        {
            if (LogLevel.All == level)
            {
                return Level.Trace;
            }
            else if (LogLevel.Debug == level)
            {
                return Level.Debug;
            }
            else if (LogLevel.Info == level)
            {
                return Level.Info;
            }
            else if (LogLevel.Warning == level)
            {
                return Level.Warn;
            }
            else if (LogLevel.Error == level)
            {
                return Level.Error;
            }
            else if (LogLevel.Fatal == level)
            {
                return Level.Fatal;
            }
            else if (LogLevel.Off == level)
            {
                return Level.Off;
            }

            return Level.Off;
        }

        public static LogLevel ToITICLogLevel(this Level level)
        {
            if (Level.Trace == level)
            {
                return LogLevel.All;
            }
            else if (Level.Debug == level)
            {
                return LogLevel.Debug;
            }
            else if (Level.Info == level)
            {
                return LogLevel.Info;
            }
            else if (Level.Warn == level)
            {
                return LogLevel.Warning;
            }
            else if (Level.Error == level)
            {
                return LogLevel.Error;
            }
            else if (Level.Fatal == level)
            {
                return LogLevel.Fatal;
            }
            else if (Level.Off == level)
            {
                return LogLevel.Off;
            }
            else if (Level.Undefined == level)
            {
                return LogLevel.Off;
            }

            return LogLevel.Off;
        }
    }
}
