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
            params LogInfo[] logInfos)
            => Build (operation, operationStatus, message, exception, logInfos?.ToList());

        public static string Build(
            Operation operation,
            OperationStatus operationStatus,
            string message,
            Exception exception,
            IEnumerable<LogInfo> logInfos)
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

            IEnumerable<LogInfo> processedDetails = GetProcessedLogInfoList(message, logInfos, exception);

            if (!(processedDetails is null))
            {
                foreach (LogInfo logInfo in processedDetails)
                {
                    logMessage += logInfo.ToString() + ",";
                }
            }

            if (logMessage.EndsWith(","))
            {
                return logMessage.Substring(0, logMessage.Length - 1);
            }

            return logMessage;
        }

        static IEnumerable<LogInfo> GetProcessedLogInfoList(string message, IEnumerable<LogInfo> logInfos, Exception ex)
        {
            List<LogInfo> processedLogInfos = new List<LogInfo>();

            if (!string.IsNullOrWhiteSpace(message))
            {
                processedLogInfos.Add(new LogInfo(LogInfoKey.Message, message));
            }
            else if (!(ex is null))
            {
                processedLogInfos.Add(new LogInfo(LogInfoKey.Message, "An exception has occurred"));
            }

            if (!(logInfos is null))
            {
                processedLogInfos.AddRange(logInfos);
            }

            if (!(ex is null))
            {
                processedLogInfos.Add(new LogInfo(LogInfoKey.Exception, ex.GetType()));
                processedLogInfos.Add(new LogInfo(LogInfoKey.ExceptionMessage, ex.Message));
                processedLogInfos.Add(new LogInfo(LogInfoKey.StackTrace, ex.StackTrace));
            }

            return processedLogInfos
                .GroupBy(x => x.Key)
                .Select(g => new LogInfo(g.First().Key, g.Last().Value))
                .Where(x => !string.IsNullOrWhiteSpace(x.Value));
        }
    }
}
