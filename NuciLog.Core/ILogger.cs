namespace NuciLog.Core
{
    using System;
    using System.Collections.Generic;

    public interface ILogger
    {
        Type SourceContext { get; }

        void SetSourceContext<T>();
        void SetSourceContext(Type type);

        void Verbose(string message);
        void Verbose(Operation operation, OperationStatus operationStatus, string message);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);

        void Debug(string message);
        void Debug(Operation operation, OperationStatus operationStatus, string message);
        void Debug(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);

        void Info(string message);
        void Info(Operation operation, OperationStatus operationStatus, string message);
        void Info(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);

        void Warn(string message);
        void Warn(Operation operation, OperationStatus operationStatus, string message);
        void Warn(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);

        void Error(string message);
        void Error(Operation operation, OperationStatus operationStatus, string message);
        void Error(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);

        void Fatal(string message);
        void Fatal(Operation operation, OperationStatus operationStatus, string message);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] details);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> details);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] details);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> details);
    }
}
