using NuciLog.Core;

namespace NuciLog.Core.UnitTests.Helpers
{
    public sealed class TestLogInfoKey : LogInfoKey
    {
        protected TestLogInfoKey(string name)
            : base(name)
        {
            
        }
        
        protected TestLogInfoKey(string name, LogInfoValueFormat valueFormat)
            : base(name, valueFormat)
        {
            
        }

        public static LogInfoKey TestKey => new TestLogInfoKey(nameof(TestKey));

        public static LogInfoKey TestKey2 => new TestLogInfoKey(nameof(TestKey2));

        public static LogInfoKey TestKeyWithDefaultFormat => new TestLogInfoKey(nameof(TestKeyWithDefaultFormat), LogInfoValueFormat.Default);

        public static LogInfoKey TestKeyWithUppercaseFormat => new TestLogInfoKey(nameof(TestKeyWithUppercaseFormat), LogInfoValueFormat.UpperCase);

        public static LogInfoKey TestKeyWithLowercaseFormat => new TestLogInfoKey(nameof(TestKeyWithLowercaseFormat), LogInfoValueFormat.LowerCase);
    }
}