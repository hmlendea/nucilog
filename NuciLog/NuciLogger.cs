using System;
using System.IO;

using NuciLog.Configuration;
using NuciLog.Core;

namespace NuciLog
{
    public sealed class NuciLogger : Logger
    {
        readonly NuciLoggerSettings settings;

        public NuciLogger(NuciLoggerSettings settings)
        {
            this.settings = settings;
        }

        protected override void WriteLog(LogLevel level, Func<string> logMessage)
        {
            if (level > settings.MinimumLevel)
            {
                return;
            }

            string timestamp = DateTime.Now.ToString(settings.TimestampFormat);
            string logLevel = level.ToString().ToUpper();
            string formattedLog = string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, logMessage());

            Console.WriteLine(formattedLog);

            if (settings.IsFileOutputEnabled &&
                !string.IsNullOrWhiteSpace(settings.LogFilePath))
            {
                File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine);
            }
        }
    }
}
