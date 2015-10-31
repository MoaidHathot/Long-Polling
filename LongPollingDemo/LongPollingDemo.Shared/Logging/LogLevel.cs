using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    [Serializable]
    public sealed class LogLevel
    {
        /// <summary>
        /// used to map between values to LogLevels.
        /// </summary>
        private static Dictionary<string, LogLevel> m_LogLevelsMap = new Dictionary<string, LogLevel>();

        public static readonly LogLevel Off = new LogLevel("Off", "OFF");
        public static readonly LogLevel Fatal = new LogLevel("Fatal", "FATAL");
        public static readonly LogLevel Error = new LogLevel("Error", "ERROR");
        public static readonly LogLevel Warning = new LogLevel("Warning", "WARN");
        public static readonly LogLevel Info = new LogLevel("Info", "INFO");
        public static readonly LogLevel Debug = new LogLevel("Debug", "DEBUG");
        /// <summary>
        /// Is Existed only for legacy purposes. Developers LOVES to mark a LogLevel as 'ALL' when actually it should be exactly as 'Debug' LogLeve.
        /// It is added to this file only to handle a scenario when trying to read a LogLevel with value 'ALL'
        /// </summary>
        public static readonly LogLevel All = new LogLevel("All", "ALL", false);

        /// <summary>
        /// The name of the LogLevel that will be used for general purposes (maybe also as a description)
        /// </summary>
        private string m_Name;
        /// <summary>
        /// The value of the LogLevel. this values will be the actual values that is retrived/written to/from the .config file and/or read by the log4net.
        /// </summary>
        private string m_Value;

        private bool m_ShowInList;

        private LogLevel(string name, string value, bool showInList = true)
        {
            m_Name = name;
            m_Value = value;
            m_ShowInList = showInList;

            m_LogLevelsMap.Add(value, this);
        }

        /// <summary>
        /// Will return the Name of the object(NOT THE VALUE).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_Name;
        }

        /// <summary>
        /// Will return a LogLevel after reading a VALUE.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static explicit operator LogLevel(string value)
        {
            return m_LogLevelsMap[value.Trim().ToUpper()];
        }

        /// <summary>
        /// Will return the string (LogLevel) VALUE of this object.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static explicit operator string(LogLevel logLevel)
        {
            return logLevel.Value;
        }

        /// <summary>
        /// Will return all the values for all the objects that are flags as 'ShowInList'
        /// </summary>
        public static string[] Values
        {
            get
            {
                var list = new List<string>();

                foreach (var key in m_LogLevelsMap.Keys)
                {
                    var entry = m_LogLevelsMap[key];

                    if (entry.ShowInList)
                        list.Add(key);
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// Will return all the Names of all the objects that are flags as 'ShowInList'
        /// </summary>
        public static string[] Names
        {
            get
            {
                var list = new List<string>();

                foreach (var key in m_LogLevelsMap.Keys)
                {
                    var entry = m_LogLevelsMap[key];

                    if (entry.ShowInList)
                        list.Add(entry.Name);
                }

                return list.ToArray();
            }
        }

        public string Value
        {
            get
            {
                return m_Value;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// Determines if the value and name ob the object should be returned when calling to 'Names' or 'Values'
        /// </summary>
        private bool ShowInList
        {
            get
            {
                return m_ShowInList;
            }
        }
    }
}
