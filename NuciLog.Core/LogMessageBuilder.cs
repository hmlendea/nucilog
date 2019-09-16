using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciLog.Core
{
    public static class LogMessageBuilder
    {
        public static string Build(
            Operation operation,
            OperationStatus operationStatus,
            string message,
            Exception exception,
            params LogInfo[] details)
            => Build (operation, operationStatus, message, exception, details?.ToList());

        public static string Build(
            Operation operation,
            OperationStatus operationStatus,
            string message,
            Exception exception,
            IEnumerable<LogInfo> details)
        {
            string logMessage = string.Empty;
            
            if (!(operation is null))
            {
                logMessage += $"{LogInfoKey.Operation.Name}={operation.Name},";
            }

            if (!(operationStatus is null))
            {
                logMessage += $"{LogInfoKey.OperationStatus.Name}={operationStatus.Name.ToUpper()},";
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                logMessage += $"{LogInfoKey.Message.Name}={message},";
            }

            if (!(details is null))
            {
                foreach (LogInfo detail in details)
                {
                    logMessage += $"{detail.Key.Name}={detail.Value},";
                }
            }

            if (!(exception is null))
            {
                logMessage += $"{LogInfoKey.Exception.Name}={exception.GetType()},";
                logMessage += $"{LogInfoKey.ExceptionMessage.Name}={exception.Message},";
                logMessage += $"{LogInfoKey.StackTrace.Name}={exception.StackTrace},";
            }

            if (logMessage.EndsWith(","))
            {
                return logMessage.Substring(0, logMessage.Length - 1);
            }

            return logMessage;
        }
    }
}
