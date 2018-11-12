namespace NuciLog.Core
{
    public class Operation
    {
        protected Operation(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public static Operation Unknown => new Operation("UNKNOWN");

        public static Operation StartUp => new Operation("STARTUP");

        public static Operation ShutDown => new Operation("SHUTDOWN");
    }
}
