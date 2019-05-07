using System;
using System.Collections.Generic;

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
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Verbose(Operation operation, OperationStatus operationStatus, string message)
        {
            Verbose(operation, operationStatus, message, details: null);
        }

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Verbose(operation, operationStatus, message, details, null);
        }

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Verbose(operation, operationStatus, message, null, exception);
        }

        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Verbose, logMessage);
        }

        public void Debug(string message)
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Debug(Operation operation, OperationStatus operationStatus, string message)
        {
            Debug(operation, operationStatus, message, details: null);
        }

        public void Debug(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Debug(operation, operationStatus, message, details, null);
        }

        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Debug(operation, operationStatus, message, null, exception);
        }

        public void Debug(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Debug, logMessage);
        }

        public void Info(string message)
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Info(Operation operation, OperationStatus operationStatus, string message)
        {
            Info(operation, operationStatus, message, details: null);
        }

        public void Info(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Info(operation, operationStatus, message, details, null);
        }

        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Info(operation, operationStatus, message, null, exception);
        }

        public void Info(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Information, logMessage);
        }

        public void Warning(string message)
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Warning(Operation operation, OperationStatus operationStatus, string message)
        {
            Warning(operation, operationStatus, message, details: null);
        }

        public void Warning(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Warning(operation, operationStatus, message, details, null);
        }

        public void Warning(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Warning(operation, operationStatus, message, null, exception);
        }

        public void Warning(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Warning, logMessage);
        }

        public void Error(string message)
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Error(Operation operation, OperationStatus operationStatus, string message)
        {
            Error(operation, operationStatus, message, details: null);
        }

        public void Error(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Error(operation, operationStatus, message, details, null);
        }

        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Error(operation, operationStatus, message, null, exception);
        }

        public void Error(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Error, logMessage);
        }

        public void Fatal(string message)
        {
            Verbose(Operation.Unknown, operationStatus: null, message: message);
        }

        public void Fatal(Operation operation, OperationStatus operationStatus, string message)
        {
            Fatal(operation, operationStatus, message, details: null);
        }

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details)
        {
            Fatal(operation, operationStatus, message, details, null);
        }

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception)
        {
            Fatal(operation, operationStatus, message, null, exception);
        }

        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception)
        {
            string logMessage = LogMessageBuilder.Build(operation, operationStatus, message, details, exception);
            WriteLog(LogLevel.Fatal, logMessage);
        }

        protected abstract void WriteLog(LogLevel level, string logMessage);
    }
}
