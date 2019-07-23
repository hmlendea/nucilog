using NuciLog.Core;

namespace NuciLog.Core.UnitTests.Helpers
{
    public sealed class TestLogInfoKey : LogInfoKey
    {
        protected TestLogInfoKey(string name)
            : base(name)
        {
            
        }

        public static LogInfoKey TestKey => new TestLogInfoKey(nameof(TestKey));

        public static LogInfoKey TestKey2 => new TestLogInfoKey(nameof(TestKey2));
    }
}