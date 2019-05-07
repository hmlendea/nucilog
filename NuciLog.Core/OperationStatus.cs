namespace NuciLog.Core
{
    public class OperationStatus
    {
        protected OperationStatus(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public static OperationStatus Unknown => new OperationStatus(nameof(Unknown));

        public static OperationStatus Started => new OperationStatus(nameof(Started));

        public static OperationStatus Success => new OperationStatus(nameof(Success));

        public static OperationStatus Failure => new OperationStatus(nameof(Failure));
    }
}
