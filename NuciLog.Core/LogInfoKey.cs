namespace NuciLog.Core
{
    public class LogInfoKey
    {
        protected LogInfoKey(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        internal static LogInfoKey Operation => new LogInfoKey(nameof(Operation));

        internal static LogInfoKey OperationStatus => new LogInfoKey(nameof(OperationStatus));

        internal static LogInfoKey Message => new LogInfoKey(nameof(Message));

        internal static LogInfoKey ExceptionMessage => new LogInfoKey(nameof(ExceptionMessage));

        internal static LogInfoKey Exception => new LogInfoKey(nameof(Exception));
    }
}
