namespace NuciLog.Core
{
    using System;
    using System.Collections.Generic;

    public interface INuciLogger
    {
        Type SourceContext { get; }

        void SetSourceContext<T>();

        void SetSourceContext(Type type);

        void Verbose(Operation operation, OperationStatus operationStatus, string message);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Verbose(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);

        void Debug(Operation operation, OperationStatus operationStatus, string message);
        void Debug(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Debug(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);

        void Info(Operation operation, OperationStatus operationStatus, string message);
        void Info(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Info(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);

        void Warning(Operation operation, OperationStatus operationStatus, string message);
        void Warning(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Warning(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Warning(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);

        void Error(Operation operation, OperationStatus operationStatus, string message);
        void Error(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Error(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);

        void Fatal(Operation operation, OperationStatus operationStatus, string message);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception);
        void Fatal(Operation operation, OperationStatus operationStatus, string message, IDictionary<LogInfoKey, string> details, Exception exception);
    }
}
