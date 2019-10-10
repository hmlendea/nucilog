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

        public LogInfo(LogInfoKey key, object value)
        {
            Key = key;
            Value = value?.ToString();
        }

        public LogInfo(LogInfoKey key, DateTime value, string format)
        {
            Key = key;
            Value = value.ToString(format);
        }

        public override string ToString()
        {
            string str = $"{Key.Name}=";

            if (Value is null)
            {
                return str;
            }

            if (Key.ValueFormat == LogInfoValueFormat.UpperCase)
            {
                str += Value.ToUpper();
            }
            else if (Key.ValueFormat == LogInfoValueFormat.LowerCase)
            {
                str += Value.ToLower();
            }
            else
            {
                str += Value;
            }

            return str;
        }
    }
}
