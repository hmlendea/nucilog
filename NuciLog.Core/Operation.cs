namespace NuciLog.Core
{
    public class Operation
    {
        protected Operation(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public static Operation Unknown => new Operation(nameof(Unknown));

        public static Operation StartUp => new Operation(nameof(StartUp));

        public static Operation ShutDown => new Operation(nameof(ShutDown));
    }
}
