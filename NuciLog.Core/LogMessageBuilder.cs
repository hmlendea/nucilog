namespace NuciLog.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class LogMessageBuilder
    {
        public static string Build(
            Operation operation,
            OperationStatus operationStatus,
            string message,
            IDictionary<LogInfoKey, string> details,
            Exception exception)
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
                foreach (KeyValuePair<LogInfoKey, string> detail in details)
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
