using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            
            if (operation is null)
            {
                logMessage += $"{LogInfoKey.Operation.Name}={Operation.Unknown.Name},";
            }
            else
            {
                logMessage += $"{LogInfoKey.Operation.Name}={operation.Name},";
            }

            if (operationStatus != null)
            {
                logMessage += $"{LogInfoKey.OperationStatus.Name}={operationStatus.Name},";
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                logMessage += $"{LogInfoKey.Message.Name}={message},";
            }

            if (details != null)
            {
                foreach (LogInfo detail in details)
                {
                    logMessage += $"{detail.Key.Name}={detail.Value},";
                }
            }

            if (exception != null)
            {
                logMessage += $"{LogInfoKey.Exception.Name}={exception.GetType()},";
                logMessage += $"{LogInfoKey.ExceptionMessage.Name}={exception.Message},";
            }
            
            return logMessage.Substring(0, logMessage.Length - 1);
        }
    }
}
