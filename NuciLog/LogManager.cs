using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using NuciLog;
using NuciLog.Enumerations;
using NuciLog.Extensions;

namespace NuciLog
{
    /// <summary>
    /// Log Manager.
    /// </summary>
    public class LogManager
    {
        static volatile LogManager instance;
        static object syncRoot = new object();

        StreamWriter writer;
        TimeSpan elapsedGameTime;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new LogManager();
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the verbosity level.
        /// </summary>
        /// <value>The verbosity level.</value>
        public int VerbosityLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logging is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the format of the timestamp.
        /// </summary>
        /// <value>The format of the timestamp.</value>
        public string TimestampFormat { get; set; }

        /// <summary>
        /// Gets or sets the logs directory path.
        /// </summary>
        /// <value>The path to the logs directory.</value>
        public string LogsDirectory { get; set; }

        /// <summary>
        /// Gets the name of the log file.
        /// </summary>
        /// <value>The log file name.</value>
        public string LogName => $"Log.{DateTime.Now.ToString("yyy-MM-dd")}.log";

        /// <summary>
        /// Gets the path to the log file.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogPath => Path.Combine(LogsDirectory, LogName);

        /// <summary>
        /// Loads the content.
        /// </summary>
        public void LoadContent()
        {
            // TODO: What if the game keeps running into the next day?
            writer = new StreamWriter(LogPath, true);
            writer.AutoFlush = true;

            VerbosityLevel = 1;
            TimestampFormat = "yyyy/MM/dd HH:mm:ss.ffffzzz";

            elapsedGameTime = new TimeSpan();
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public void UnloadContent()
        {
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Update the content.
        /// </summary>
        /// <param name="elapsedGameTime">Elapsed game time.</param>
        public void Update(TimeSpan elapsedGameTime)
        {
            this.elapsedGameTime = elapsedGameTime;
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="ex">Exception.</param>
        public void Error(Operation operation, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, OperationStatus.Failure);

            WriteLine($"ERROR|{log}", ex);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Error(Operation operation, string message, Exception ex = null)
        {
            Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
            {
                { LogInfoKey.Message, message }
            };

            string log = LogBuilder.BuildKvpMessage(operation, OperationStatus.Failure, logDetails);

            WriteLine($"ERROR|{log}", ex);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Error(Operation operation, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, OperationStatus.Failure, logDetails);

            WriteLine($"ERROR|{log}", ex);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="message">Text.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Error(Operation operation, string message, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            logDetails.Add(LogInfoKey.Message, message);
            Error(operation, logDetails, ex);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="ex">Exception.</param>
        public void Info(Operation operation, OperationStatus status, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, status);

            WriteLine($"INFO|{log}", ex);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Info(Operation operation, OperationStatus status, string message, Exception ex = null)
        {
            Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
            {
                { LogInfoKey.Message, message }
            };

            string log = LogBuilder.BuildKvpMessage(operation, status, logDetails);

            WriteLine($"INFO|{log}", ex);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Info(Operation operation, OperationStatus status, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, status, logDetails);

            WriteLine($"INFO|{log}", ex);
        }

        /// <summary>
        /// Writes the information.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="message">Text.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Info(Operation operation, OperationStatus status, string message, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            logDetails.Add(LogInfoKey.Message, message);
            Info(operation, status, logDetails, ex);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="ex">Exception.</param>
        public void Warn(Operation operation, OperationStatus status, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, status);

            WriteLine($"WARN|{log}", ex);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        public void Warn(Operation operation, OperationStatus status, string message, Exception ex = null)
        {
            Dictionary<LogInfoKey, string> logDetails = new Dictionary<LogInfoKey, string>
            {
                { LogInfoKey.Message, message }
            };

            string log = LogBuilder.BuildKvpMessage(operation, status, logDetails);

            WriteLine($"WARN|{log}", ex);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Warn(Operation operation, OperationStatus status, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            string log = LogBuilder.BuildKvpMessage(operation, status, logDetails);

            WriteLine($"WARN|{log}", ex);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="operation">Operation.</param>
        /// <param name="status">Operation status.</param>
        /// <param name="message">Text.</param>
        /// <param name="logDetails">Details.</param>
        /// <param name="ex">Exception.</param>
        public void Warn(Operation operation, OperationStatus status, string message, Dictionary<LogInfoKey, string> logDetails, Exception ex = null)
        {
            logDetails.Add(LogInfoKey.Message, message);
            Warn(operation, status, logDetails, ex);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="message">Text.</param>
        /// <param name="ex">Exception.</param>
        void WriteLine(string message, Exception ex = null)
        {
            string processedStackTrace = string.Empty;
            
            if (ex != null)
            {
                processedStackTrace = ex.StackTrace.Replace(Environment.NewLine, "$");
            }

            string logEntry = $"{DateTime.Now.ToString(TimestampFormat)}|{elapsedGameTime.TotalMilliseconds}|{message}|{processedStackTrace}";

            writer.WriteLine(logEntry);

            Debug.WriteLine(logEntry);
        }
    }
}
