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
            => Build (operation, operationStatus, message, exception, details.ToList());

        public static string Build(
            Operation operation,
            OperationStatus operationStatus,
            string message,
            Exception exception,
            IEnumerable<LogInfo> details)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append($"{LogInfoKey.Operation.Name}={operation.Name},");

            if (operationStatus != null)
            {
                stringBuilder.AppendFormat($"{LogInfoKey.OperationStatus.Name}={operationStatus.Name},");
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                stringBuilder.AppendFormat($"{LogInfoKey.Message.Name}={message},");
            }

            if (details != null)
            {
                foreach (LogInfo detail in details)
                {
                    stringBuilder.Append($"{detail.Key.Name}={detail.Value},");
                }
            }

            if (exception != null)
            {
                stringBuilder.AppendFormat($"{LogInfoKey.Exception.Name}={exception.GetType()},");
                stringBuilder.AppendFormat($"{LogInfoKey.ExceptionMessage.Name}={exception.Message},");
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
    }
}
