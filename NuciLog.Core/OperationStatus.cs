namespace NuciLog.Core
{
    public class OperationStatus
    {
        protected OperationStatus(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public static OperationStatus Unknown => new OperationStatus("UNKNOWN");

        public static OperationStatus Started => new OperationStatus("STARTED");

        public static OperationStatus Success => new OperationStatus("FINISHED_SUCCESS");

        public static OperationStatus Failure => new OperationStatus("FINISHED_FAILURE");
    }
}
