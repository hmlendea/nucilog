using System;
using System.Collections.Generic;
using System.IO;

using NuciLog.Core;

namespace NuciLog
{
    public sealed class NuciLogger : Logger
    {
        public string TimestampFormat { get; set; }

        public string LogLineFormat { get; set; }

        public string LogFilePath { get; set; }

        public bool IsFileOutputEnabled { get; set; }

        public NuciLogger()
        {
            TimestampFormat = "yyyy/MM/dd HH:mm:ss.fff";
            LogLineFormat = "{0}|{1}|{2}|{3}";

            LogFilePath = "logfile.log";
            IsFileOutputEnabled = true;
        }

        protected override void WriteLog(LogLevel level, string logMessage)
        {
            string timestamp = DateTime.Now.ToString(TimestampFormat);
            string formattedLog = string.Format(LogLineFormat, timestamp, SourceContext, level.ToString().ToUpper(), logMessage);

            Console.WriteLine(formattedLog);

            if (IsFileOutputEnabled && !string.IsNullOrWhiteSpace(LogFilePath))
            {
                File.AppendAllText(LogFilePath, formattedLog + Environment.NewLine);
            }
        }
    }
}
