using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciLog.Core
{
    public abstract class Logger : ILogger
    {
        public Type SourceContext { get; private set; }

        public void SetSourceContext<T>()
        {
            SetSourceContext(typeof(T));
        }

        public virtual void SetSourceContext(Type type)
        {
            SourceContext = type;
        }

        public void Verbose(Operation operation)
            => Verbose(operation, operationStatus: null, message: null, details: null);
        public void Verbose(Operation operation, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Verbose(Operation operation, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Verbose(Operation operation, Exception exception)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Verbose(Operation operation, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus)
            => Verbose(operation, operationStatus, message: null, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details);
        public void Verbose(string message)
            => Verbose(operation: null, operationStatus: null, message: message);
        public void Verbose(Operation operation, string message)
            => Verbose(operation, operationStatus: null, message: message, details: null);
        public void Verbose(Operation operation, string message, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Verbose(Operation operation, string message, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Verbose(Operation operation, string message, Exception exception)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Verbose(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message)
            => Verbose(operation, operationStatus, message, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, null, details);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message, null, details);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Verbose(operation, operationStatus, message, exception, null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, exception, details?.ToList());
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Verbose, logMessage);
        }

        public void Debug(Operation operation)
            => Debug(operation, operationStatus: null, message: null, details: null);
        public void Debug(Operation operation, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Debug(Operation operation, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Debug(Operation operation, Exception exception)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Debug(Operation operation, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus)
            => Debug(operation, operationStatus, message: null, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception)
            => Debug(operation, operationStatus, message: null, exception: exception, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details);
        public void Debug(string message)
            => Debug(operation: null, operationStatus: null, message: message);
        public void Debug(Operation operation, string message)
            => Debug(operation, operationStatus: null, message: message, details: null);
        public void Debug(Operation operation, string message, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Debug(Operation operation, string message, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Debug(Operation operation, string message, Exception exception)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Debug(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, string message)
            => Debug(operation, operationStatus, message, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Debug(operation, operationStatus, message, null, details);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message, null, details);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Debug(operation, operationStatus, message, exception, null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message, exception, details?.ToList());
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Debug, logMessage);
        }

        public void Info(Operation operation)
            => Info(operation, operationStatus: null, message: null, details: null);
        public void Info(Operation operation, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Info(Operation operation, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Info(Operation operation, Exception exception)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Info(Operation operation, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Info(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Info(Operation operation, OperationStatus operationStatus)
            => Info(operation, operationStatus, message: null, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception)
            => Info(operation, operationStatus, message: null, exception: exception, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details);
        public void Info(string message)
            => Info(operation: null, operationStatus: null, message: message);
        public void Info(Operation operation, string message)
            => Info(operation, operationStatus: null, message: message, details: null);
        public void Info(Operation operation, string message, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Info(Operation operation, string message, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Info(Operation operation, string message, Exception exception)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Info(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Info(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, string message)
            => Info(operation, operationStatus, message, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Info(operation, operationStatus, message, null, details);
        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message, null, details);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Info(operation, operationStatus, message, exception, null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message, exception, details?.ToList());
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Info, logMessage);
        }

        public void Warn(Operation operation)
            => Warn(operation, operationStatus: null, message: null, details: null);
        public void Warn(Operation operation, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Warn(Operation operation, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Warn(Operation operation, Exception exception)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Warn(Operation operation, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus)
            => Warn(operation, operationStatus, message: null, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception)
            => Warn(operation, operationStatus, message: null, exception: exception, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details);
        public void Warn(string message)
            => Warn(operation: null, operationStatus: null, message: message);
        public void Warn(Operation operation, string message)
            => Warn(operation, operationStatus: null, message: message, details: null);
        public void Warn(Operation operation, string message, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Warn(Operation operation, string message, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Warn(Operation operation, string message, Exception exception)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Warn(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, string message)
            => Warn(operation, operationStatus, message, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Warn(operation, operationStatus, message, null, details);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message, null, details);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Warn(operation, operationStatus, message, exception, null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message, exception, details?.ToList());
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Warn, logMessage);
        }

        public void Error(Operation operation)
            => Error(operation, operationStatus: null, message: null, details: null);
        public void Error(Operation operation, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Error(Operation operation, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Error(Operation operation, Exception exception)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Error(Operation operation, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Error(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Error(Operation operation, OperationStatus operationStatus)
            => Error(operation, operationStatus, message: null, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception)
            => Error(operation, operationStatus, message: null, exception: exception, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details);
        public void Error(string message)
            => Error(operation: null, operationStatus: null, message: message);
        public void Error(Operation operation, string message)
            => Error(operation, operationStatus: null, message: message, details: null);
        public void Error(Operation operation, string message, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Error(Operation operation, string message, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Error(Operation operation, string message, Exception exception)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Error(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Error(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, string message)
            => Error(operation, operationStatus, message, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Error(operation, operationStatus, message, null, details);
        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message, null, details);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Error(operation, operationStatus, message, exception, null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message, exception, details?.ToList());
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Error, logMessage);
        }

        public void Fatal(Operation operation)
            => Fatal(operation, operationStatus: null, message: null, details: null);
        public void Fatal(Operation operation, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Fatal(Operation operation, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Fatal(Operation operation, Exception exception)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Fatal(Operation operation, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus)
            => Fatal(operation, operationStatus, message: null, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details);
        public void Fatal(string message)
            => Fatal(operation: null, operationStatus: null, message: message);
        public void Fatal(Operation operation, string message)
            => Fatal(operation, operationStatus: null, message: message, details: null);
        public void Fatal(Operation operation, string message, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Fatal(Operation operation, string message, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Fatal(Operation operation, string message, Exception exception)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Fatal(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message)
            => Fatal(operation, operationStatus, message, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, null, details);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message, null, details);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Fatal(operation, operationStatus, message, exception, null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, exception, details?.ToList());
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Fatal, logMessage);
        }

        protected abstract void WriteLog(LogLevel level, string logMessage);
    }
}
