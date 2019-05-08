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

        public void Verbose(string message)
            => Verbose(Operation.Unknown, operationStatus: null, message: message);

        public void Verbose(Operation operation, OperationStatus operationStatus)
            => Verbose(operation, operationStatus, message: null, details: null);

        public void Verbose(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);

        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);

        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: null);

        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details);

        public void Verbose(Operation operation, OperationStatus operationStatus, string message)
            => Verbose(operation, operationStatus, message, details: null);

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, null, details);

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message, null, details);

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Verbose(operation, operationStatus, message, exception, null);

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, exception, details.ToList());

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Verbose, logMessage);
        }

        public void Debug(string message)
            => Debug(Operation.Unknown, operationStatus: null, message: message);

        public void Debug(Operation operation, OperationStatus operationStatus)
            => Debug(operation, operationStatus, message: null, details: null);

        public void Debug(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);

        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);

        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception)
            => Debug(operation, operationStatus, message: null, exception: exception, details: null);

        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details);

        public void Debug(Operation operation, OperationStatus operationStatus, string message)
            => Debug(operation, operationStatus, message, details: null);

        public void Debug(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Debug(operation, operationStatus, message, null, details);

        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message, null, details);

        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Debug(operation, operationStatus, message, exception, null);

        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message, exception, details.ToList());

        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Debug, logMessage);
        }

        public void Info(string message)
            => Info(Operation.Unknown, operationStatus: null, message: message);

        public void Info(Operation operation, OperationStatus operationStatus)
            => Info(operation, operationStatus, message: null, details: null);

        public void Info(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);

        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);

        public void Info(Operation operation, OperationStatus operationStatus, Exception exception)
            => Info(operation, operationStatus, message: null, exception: exception, details: null);

        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details);

        public void Info(Operation operation, OperationStatus operationStatus, string message)
            => Info(operation, operationStatus, message, details: null);

        public void Info(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Info(operation, operationStatus, message, null, details);

        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message, null, details);

        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Info(operation, operationStatus, message, exception, null);

        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message, exception, details.ToList());

        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Info, logMessage);
        }
        
        public void Warn(string message)
            => Warn(Operation.Unknown, operationStatus: null, message: message);

        public void Warn(Operation operation, OperationStatus operationStatus)
            => Warn(operation, operationStatus, message: null, details: null);

        public void Warn(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);

        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);

        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception)
            => Warn(operation, operationStatus, message: null, exception: exception, details: null);

        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details);

        public void Warn(Operation operation, OperationStatus operationStatus, string message)
            => Warn(operation, operationStatus, message, details: null);

        public void Warn(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Warn(operation, operationStatus, message, null, details);

        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message, null, details);

        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Warn(operation, operationStatus, message, exception, null);

        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message, exception, details.ToList());

        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Warn, logMessage);
        }
        
        public void Error(string message)
            => Error(Operation.Unknown, operationStatus: null, message: message);

        public void Error(Operation operation, OperationStatus operationStatus)
            => Error(operation, operationStatus, message: null, details: null);

        public void Error(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);

        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);

        public void Error(Operation operation, OperationStatus operationStatus, Exception exception)
            => Error(operation, operationStatus, message: null, exception: exception, details: null);

        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details);

        public void Error(Operation operation, OperationStatus operationStatus, string message)
            => Error(operation, operationStatus, message, details: null);

        public void Error(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Error(operation, operationStatus, message, null, details);

        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message, null, details);

        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Error(operation, operationStatus, message, exception, null);

        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message, exception, details.ToList());

        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Error, logMessage);
        }

        public void Fatal(string message)
            => Fatal(Operation.Unknown, operationStatus: null, message: message);

        public void Fatal(Operation operation, OperationStatus operationStatus)
            => Fatal(operation, operationStatus, message: null, details: null);

        public void Fatal(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);

        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);

        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: null);

        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details.ToList());

        public void Fatal(Operation operation, OperationStatus operationStatus, string message)
            => Fatal(operation, operationStatus, message, details: null);

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, null, details);

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message, null, details);

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Fatal(operation, operationStatus, message, exception, null);

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, exception, details.ToList());

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, exception, details);
            WriteLog(LogLevel.Fatal, logMessage);
        }

        protected abstract void WriteLog(LogLevel level, string logMessage);
    }
}
