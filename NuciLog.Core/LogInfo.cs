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
    }
}
