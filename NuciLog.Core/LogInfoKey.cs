using System;

namespace NuciLog.Core
{
    public class LogInfoKey : IEquatable<LogInfoKey>
    {
        public string Name { get; protected set; }

        public LogInfoValueFormat ValueFormat { get; protected set; }

        protected LogInfoKey(string name)
        {
            Name = name;
        }

        protected LogInfoKey(string name, LogInfoValueFormat valueFormat)
            : this(name)
        {
            ValueFormat = valueFormat;
        }

        public bool Equals(LogInfoKey other)
            => Name == other.Name;
        
        public override bool Equals(object other)
        {
            if (other is LogInfoKey)
            {
                return Equals(other as LogInfoKey);
            }

            return false;
        }

        public override int GetHashCode()
            => Name.GetHashCode();

        internal static LogInfoKey SourceContext => new LogInfoKey(nameof(SourceContext));

        internal static LogInfoKey Operation => new LogInfoKey(nameof(Operation));

        internal static LogInfoKey OperationStatus => new LogInfoKey(nameof(OperationStatus));

        internal static LogInfoKey Message => new LogInfoKey(nameof(Message));

        internal static LogInfoKey Exception => new LogInfoKey(nameof(Exception));

        internal static LogInfoKey ExceptionMessage => new LogInfoKey(nameof(ExceptionMessage));

        internal static LogInfoKey StackTrace => new LogInfoKey(nameof(StackTrace));
    }
}
