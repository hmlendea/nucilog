using NuciLog.Core;

namespace NuciLog.Configuration
{
    /// <summary>
    /// Represents the configuration of a <see cref="NuciLogger"/> instance.
    /// </summary>
    public sealed class NuciLoggerSettings
    {
        /// <summary>
        /// Gets or sets the format used to render log timestamps.
        /// </summary>
        public string TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the composite format of a log line.
        /// </summary>
        public string LogLineFormat { get; set; }

        /// <summary>
        /// Gets or sets the path of the file that receives the log entries.
        /// </summary>
        public string LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the minimum severity that is written.
        /// </summary>
        public LogLevel MinimumLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether log entries are written to a file.
        /// </summary>
        public bool IsFileOutputEnabled { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="NuciLoggerSettings"/> class.
        /// </summary>
        public NuciLoggerSettings()
        {
            TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
            LogLineFormat = "{0}｜{1}｜{2}｜{3}";
            LogFilePath = "logfile.log";
            MinimumLevel = LogLevel.Info;
            IsFileOutputEnabled = true;
        }
    }
}
