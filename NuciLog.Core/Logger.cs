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
            => Verbose(operation, operationStatus: null, message: null, logInfos: null);
        public void Verbose(Operation operation, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, Exception exception)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Verbose(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Verbose(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Verbose(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus)
            => Verbose(operation, operationStatus, message: null, logInfos: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception)
            => Verbose(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(string message)
            => Verbose(operation: null, operationStatus: null, message: message);
        public void Verbose(string message, Exception exception)
            => Verbose(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Verbose(Operation operation, string message)
            => Verbose(operation, operationStatus: null, message: message, logInfos: null);
        public void Verbose(Operation operation, string message, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, string message, Exception exception)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Verbose(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Verbose(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Verbose(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message)
            => Verbose(operation, operationStatus, message, logInfos: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Verbose(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Verbose(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Verbose(operation, operationStatus, message, exception, logInfos: null);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Verbose(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Verbose, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Verbose(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Verbose(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Verbose(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Verbose(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        public void Debug(Operation operation)
            => Debug(operation, operationStatus: null, message: null, logInfos: null);
        public void Debug(Operation operation, params LogInfo[] logInfos)
            => Debug(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, Exception exception)
            => Debug(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Debug(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Debug(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Debug(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Debug(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, OperationStatus operationStatus)
            => Debug(operation, operationStatus, message: null, logInfos: null);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception)
            => Debug(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Debug(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Debug(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Debug(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(string message)
            => Debug(operation: null, operationStatus: null, message: message);
        public void Debug(string message, Exception exception)
            => Debug(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Debug(Operation operation, string message)
            => Debug(operation, operationStatus: null, message: message, logInfos: null);
        public void Debug(Operation operation, string message, params LogInfo[] logInfos)
            => Debug(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, string message, Exception exception)
            => Debug(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Debug(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Debug(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Debug(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Debug(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, string message)
            => Debug(operation, operationStatus, message, logInfos: null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Debug(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Debug(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Debug(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Debug(operation, operationStatus, message, exception, logInfos: null);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Debug(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Debug, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Debug(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Debug(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Debug(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Debug(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        public void Info(Operation operation)
            => Info(operation, operationStatus: null, message: null, logInfos: null);
        public void Info(Operation operation, params LogInfo[] logInfos)
            => Info(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Info(Operation operation, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Info(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, Exception exception)
            => Info(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Info(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Info(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Info(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Info(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, OperationStatus operationStatus)
            => Info(operation, operationStatus, message: null, logInfos: null);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception)
            => Info(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Info(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Info(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Info(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Info(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Info(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(string message)
            => Info(operation: null, operationStatus: null, message: message);
        public void Info(string message, Exception exception)
            => Info(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Info(Operation operation, string message)
            => Info(operation, operationStatus: null, message: message, logInfos: null);
        public void Info(Operation operation, string message, params LogInfo[] logInfos)
            => Info(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Info(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Info(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, string message, Exception exception)
            => Info(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Info(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Info(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Info(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Info(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, OperationStatus operationStatus, string message)
            => Info(operation, operationStatus, message, logInfos: null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Info(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Info(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Info(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Info(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Info(operation, operationStatus, message, exception, logInfos: null);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Info(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Info, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Info(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Info(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Info(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Info(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        public void Warn(Operation operation)
            => Warn(operation, operationStatus: null, message: null, logInfos: null);
        public void Warn(Operation operation, params LogInfo[] logInfos)
            => Warn(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, Exception exception)
            => Warn(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Warn(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Warn(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Warn(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Warn(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, OperationStatus operationStatus)
            => Warn(operation, operationStatus, message: null, logInfos: null);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception)
            => Warn(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Warn(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Warn(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Warn(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(string message)
            => Warn(operation: null, operationStatus: null, message: message);
        public void Warn(string message, Exception exception)
            => Warn(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Warn(Operation operation, string message)
            => Warn(operation, operationStatus: null, message: message, logInfos: null);
        public void Warn(Operation operation, string message, params LogInfo[] logInfos)
            => Warn(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, string message, Exception exception)
            => Warn(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Warn(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Warn(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Warn(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Warn(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, string message)
            => Warn(operation, operationStatus, message, logInfos: null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Warn(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Warn(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Warn(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Warn(operation, operationStatus, message, exception, logInfos: null);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Warn(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Warn, ()=> LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Warn(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Warn(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Warn(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Warn(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        public void Error(Operation operation)
            => Error(operation, operationStatus: null, message: null, logInfos: null);
        public void Error(Operation operation, params LogInfo[] logInfos)
            => Error(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Error(Operation operation, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Error(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, Exception exception)
            => Error(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Error(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Error(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Error(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Error(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, OperationStatus operationStatus)
            => Error(operation, operationStatus, message: null, logInfos: null);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception)
            => Error(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Error(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Error(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Error(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Error(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Error(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(string message)
            => Error(operation: null, operationStatus: null, message: message);
        public void Error(string message, Exception exception)
            => Error(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Error(Operation operation, string message)
            => Error(operation, operationStatus: null, message: message, logInfos: null);
        public void Error(Operation operation, string message, params LogInfo[] logInfos)
            => Error(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Error(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Error(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, string message, Exception exception)
            => Error(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Error(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Error(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Error(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Error(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, OperationStatus operationStatus, string message)
            => Error(operation, operationStatus, message, logInfos: null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Error(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Error(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Error(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Error(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Error(operation, operationStatus, message, exception, logInfos: null);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Error(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Error, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Error(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Error(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Error(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Error(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        public void Fatal(Operation operation)
            => Fatal(operation, operationStatus: null, message: null, logInfos: null);
        public void Fatal(Operation operation, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, Exception exception)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, logInfos: null);
        public void Fatal(Operation operation, Exception exception, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Fatal(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos);
        public void Fatal(Operation operation, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus: null, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus)
            => Fatal(operation, operationStatus, message: null, logInfos: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception)
            => Fatal(operation, operationStatus, message: null, exception: exception, logInfos: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus, message: null, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus, message: null, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus, message: null, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus, message: null, exception: exception, logInfos: logInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus, message: null, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(string message)
            => Fatal(operation: null, operationStatus: null, message: message);
        public void Fatal(string message, Exception exception)
            => Fatal(operation: null, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Fatal(Operation operation, string message)
            => Fatal(operation, operationStatus: null, message: message, logInfos: null);
        public void Fatal(Operation operation, string message, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, string message, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, string message, Exception exception)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, logInfos: null);
        public void Fatal(Operation operation, string message, Exception exception, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos as IEnumerable<LogInfo>);
        public void Fatal(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos);
        public void Fatal(Operation operation, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus: null, message: message, exception: exception, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message)
            => Fatal(operation, operationStatus, message, logInfos: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos)
            => Fatal(operation, operationStatus, message, exception: null, logInfos: logInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
            => Fatal(operation, operationStatus, message, exception: null, logInfos: logInfos, extraLogInfos: extraLogInfos);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception)
            => Fatal(operation, operationStatus, message, exception, logInfos: null);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, params LogInfo[] logInfos)
            => Fatal(operation, operationStatus, message, exception, logInfos as IEnumerable<LogInfo>);
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos)
        {
            WriteLog(LogLevel.Fatal, () => LogMessageBuilder.Build(operation, operationStatus, message, exception, logInfos));
        }
        public void Fatal(Operation operation, OperationStatus operationStatus, string message, Exception exception, IEnumerable<LogInfo> logInfos, params LogInfo[] extraLogInfos)
        {
            if (logInfos is null)
            {
                Fatal(operation, operationStatus, message, exception, extraLogInfos);
            }
            else if (extraLogInfos is null)
            {
                Fatal(operation, operationStatus, message, exception, logInfos);
            }
            else
            {
                Fatal(operation, operationStatus, message, exception, logInfos.Union(extraLogInfos));
            }
        }

        protected abstract void WriteLog(LogLevel level, Func<string> logMessage);
    }
}
