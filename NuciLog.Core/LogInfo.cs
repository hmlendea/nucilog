using System;

namespace NuciLog.Core
{
    public sealed class LogInfo
    {
        public LogInfoKey Key { get; }

        public string Value { get; }

        public LogInfo(LogInfoKey key, string value)
        {
            Key = key;
            Value = value;
        }

        public LogInfo(LogInfoKey key, DateTime value, string format)
        {
            Key = key;
            Value = value.ToString(format);
        }

        public LogInfo(LogInfoKey key, object value)
        {
            Key = key;
            Value = value.ToString();
        }
    }
}
