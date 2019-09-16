using NuciLog.Core;

namespace NuciLog.Configuration
{
    public sealed class NuciLoggerSettings
    {
        public string TimestampFormat { get; set; }

        public string LogLineFormat { get; set; }

        public string LogFilePath { get; set; }

        public LogLevel MinimumLevel { get; set; }

        public bool IsFileOutputEnabled { get; set; }

        public NuciLoggerSettings()
        {
            TimestampFormat = "yyyy/MM/dd HH:mm:ss.fff";
            LogLineFormat = "{0}|{1}|{2}|{3}";
            LogFilePath = "logfile.log";
            MinimumLevel = LogLevel.Information;
            IsFileOutputEnabled = true;
        }
    }
}
