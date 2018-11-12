namespace NuciLog.Core
{
    using System;
    using System.Collections.Generic;

    using Serilog;

    public sealed class NuciLogger : INuciLogger
    {
        ILogger logger;

        public NuciLogger()
        {
            this.logger = Log.Logger;
        }

        public Type SourceContext { get; private set; }

        public void SetSourceContext<T>()
        {
            SetSourceContext(typeof(T));
        }

        public void SetSourceContext(Type type)
        {
            this.logger = Log.Logger.ForContext(type);
            SourceContext = type;
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

            this.logger.Verbose(logMessage);
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

            this.logger.Debug(logMessage);
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

            this.logger.Information(logMessage);
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

            this.logger.Warning(logMessage);
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

            this.logger.Error(logMessage);
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

            this.logger.Fatal(logMessage);
        }
    }
}
