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
        public void Verbose(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Verbose(Operation operation, Exception exception)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Verbose(Operation operation, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Verbose(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Verbose(Operation operation, OperationStatus operationStatus)
            => Verbose(operation, operationStatus, message: null, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Verbose(string message)
            => Verbose(operation: null, operationStatus: null, message: message);
        public void Verbose(string message, Exception exception)
            => Verbose(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Verbose(Operation operation, string message)
            => Verbose(operation, operationStatus: null, message: message, details: null);
        public void Verbose(Operation operation, string message, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Verbose(Operation operation, string message, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Verbose(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Verbose(Operation operation, string message, Exception exception)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Verbose(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Verbose(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Verbose(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message)
            => Verbose(operation, operationStatus, message, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Verbose(operation, operationStatus, message, exception: null, details: details);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Verbose(operation, operationStatus, message, exception, details: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Verbose(operation, operationStatus, message, exception, details?.ToList());
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Verbose, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Verbose(operation, operationStatus, message, exception, details.Concat(details2));

        public void Debug(Operation operation)
            => Debug(operation, operationStatus: null, message: null, details: null);
        public void Debug(Operation operation, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Debug(Operation operation, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Debug(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Debug(Operation operation, Exception exception)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Debug(Operation operation, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Debug(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Debug(Operation operation, OperationStatus operationStatus)
            => Debug(operation, operationStatus, message: null, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception)
            => Debug(operation, operationStatus, message: null, exception: exception, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Debug(string message)
            => Debug(operation: null, operationStatus: null, message: message);
        public void Debug(string message, Exception exception)
            => Debug(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Debug(Operation operation, string message)
            => Debug(operation, operationStatus: null, message: message, details: null);
        public void Debug(Operation operation, string message, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Debug(Operation operation, string message, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Debug(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Debug(Operation operation, string message, Exception exception)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Debug(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Debug(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Debug(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Debug(Operation operation, OperationStatus operationStatus, string message)
            => Debug(operation, operationStatus, message, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Debug(operation, operationStatus, message, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Debug(operation, operationStatus, message, exception: null, details: details);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Debug(operation, operationStatus, message, exception, details: null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Debug(operation, operationStatus, message, exception, details?.ToList());
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Debug, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Debug(operation, operationStatus, message, exception, details.Concat(details2));

        public void Info(Operation operation)
            => Info(operation, operationStatus: null, message: null, details: null);
        public void Info(Operation operation, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Info(Operation operation, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Info(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Info(Operation operation, Exception exception)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Info(Operation operation, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Info(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Info(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Info(Operation operation, OperationStatus operationStatus)
            => Info(operation, operationStatus, message: null, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception)
            => Info(operation, operationStatus, message: null, exception: exception, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message: null, exception: exception, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Info(string message)
            => Info(operation: null, operationStatus: null, message: message);
        public void Info(string message, Exception exception)
            => Info(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Info(Operation operation, string message)
            => Info(operation, operationStatus: null, message: message, details: null);
        public void Info(Operation operation, string message, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Info(Operation operation, string message, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Info(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Info(Operation operation, string message, Exception exception)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Info(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Info(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Info(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Info(Operation operation, OperationStatus operationStatus, string message)
            => Info(operation, operationStatus, message, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Info(operation, operationStatus, message, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Info(operation, operationStatus, message, exception: null, details: details);
        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Info(operation, operationStatus, message, exception, details: null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Info(operation, operationStatus, message, exception, details?.ToList());
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Info, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Info(operation, operationStatus, message, exception, details.Concat(details2));

        public void Warn(Operation operation)
            => Warn(operation, operationStatus: null, message: null, details: null);
        public void Warn(Operation operation, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Warn(Operation operation, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Warn(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Warn(Operation operation, Exception exception)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Warn(Operation operation, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Warn(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Warn(Operation operation, OperationStatus operationStatus)
            => Warn(operation, operationStatus, message: null, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception)
            => Warn(operation, operationStatus, message: null, exception: exception, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Warn(string message)
            => Warn(operation: null, operationStatus: null, message: message);
        public void Warn(string message, Exception exception)
            => Warn(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Warn(Operation operation, string message)
            => Warn(operation, operationStatus: null, message: message, details: null);
        public void Warn(Operation operation, string message, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Warn(Operation operation, string message, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Warn(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Warn(Operation operation, string message, Exception exception)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Warn(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Warn(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Warn(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Warn(Operation operation, OperationStatus operationStatus, string message)
            => Warn(operation, operationStatus, message, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Warn(operation, operationStatus, message, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Warn(operation, operationStatus, message, exception: null, details: details);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Warn(operation, operationStatus, message, exception, details: null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Warn(operation, operationStatus, message, exception, details?.ToList());
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Warn, ()=> LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Warn(operation, operationStatus, message, exception, details.Concat(details2));

        public void Error(Operation operation)
            => Error(operation, operationStatus: null, message: null, details: null);
        public void Error(Operation operation, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Error(Operation operation, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Error(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Error(Operation operation, Exception exception)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Error(Operation operation, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Error(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Error(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Error(Operation operation, OperationStatus operationStatus)
            => Error(operation, operationStatus, message: null, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception)
            => Error(operation, operationStatus, message: null, exception: exception, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message: null, exception: exception, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Error(string message)
            => Error(operation: null, operationStatus: null, message: message);
        public void Error(string message, Exception exception)
            => Error(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Error(Operation operation, string message)
            => Error(operation, operationStatus: null, message: message, details: null);
        public void Error(Operation operation, string message, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Error(Operation operation, string message, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Error(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Error(Operation operation, string message, Exception exception)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Error(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Error(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Error(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Error(Operation operation, OperationStatus operationStatus, string message)
            => Error(operation, operationStatus, message, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Error(operation, operationStatus, message, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Error(operation, operationStatus, message, exception: null, details: details);
        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Error(operation, operationStatus, message, exception, details: null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Error(operation, operationStatus, message, exception, details?.ToList());
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Error, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Error(operation, operationStatus, message, exception, details.Concat(details2));

        public void Fatal(Operation operation)
            => Fatal(operation, operationStatus: null, message: null, details: null);
        public void Fatal(Operation operation, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Fatal(Operation operation, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: null, exception: null, details: details);
        public void Fatal(Operation operation, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus: null, message: null, exception: null, details: details, details2: details2);
        public void Fatal(Operation operation, Exception exception)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: null);
        public void Fatal(Operation operation, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: details);
        public void Fatal(Operation operation, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, details: details, details2: details2);
        public void Fatal(Operation operation, OperationStatus operationStatus)
            => Fatal(operation, operationStatus, message: null, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus, message: null, exception: null, details: details, details2: details2);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus, message: null, exception: exception, details: details, details2: details2);
        public void Fatal(string message)
            => Fatal(operation: null, operationStatus: null, message: message);
        public void Fatal(string message, Exception exception)
            => Fatal(operation: null, operationStatus: null, message: message, exception: exception, details: null);
        public void Fatal(Operation operation, string message)
            => Fatal(operation, operationStatus: null, message: message, details: null);
        public void Fatal(Operation operation, string message, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Fatal(Operation operation, string message, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: message, exception: null, details: details);
        public void Fatal(Operation operation, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus: null, message: message, exception: null, details: details, details2: details2);
        public void Fatal(Operation operation, string message, Exception exception)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: null);
        public void Fatal(Operation operation, string message, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: details?.ToList());
        public void Fatal(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: details);
        public void Fatal(Operation operation, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, details: details, details2: details2);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message)
            => Fatal(operation, operationStatus, message, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details)
            => Fatal(operation, operationStatus, message, exception: null, details: details);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus, message, exception: null, details: details, details2: details2);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Fatal(operation, operationStatus, message, exception, details: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details)
            => Fatal(operation, operationStatus, message, exception, details?.ToList());
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details)
        {
            WriteLog(LogLevel.Fatal, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, details));
        }
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details, params LogInfo[] details2)
            => Fatal(operation, operationStatus, message, exception, details.Concat(details2));

        protected abstract void WriteLog(LogLevel level, Func<string> logMessage);
    }
}
