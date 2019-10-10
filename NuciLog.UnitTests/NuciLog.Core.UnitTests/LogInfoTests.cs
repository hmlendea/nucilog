using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LogInfoTests
    {
        [Test]
        public void ToString_ValueFormatIsDefault_ReturnsStringWithValueAsIs()
        {
            LogInfo logInfo = new LogInfo(TestLogInfoKey.TestKeyWithDefaultFormat, "TeSt");
            Assert.AreEqual("TestKeyWithDefaultFormat=TeSt", logInfo.ToString());
        }

        [Test]
        public void ToString_ValueFormatIsUpperCase_ReturnsStringWithValueAsUpperCase()
        {
            LogInfo logInfo = new LogInfo(TestLogInfoKey.TestKeyWithUppercaseFormat, "TeSt");
            Assert.AreEqual("TestKeyWithUppercaseFormat=TEST", logInfo.ToString());
        }

        [Test]
        public void ToString_ValueFormatIsLowerCase_ReturnsStringWithValueAsLowerCase()
        {
            LogInfo logInfo = new LogInfo(TestLogInfoKey.TestKeyWithLowercaseFormat, "TeSt");
            Assert.AreEqual("TestKeyWithLowercaseFormat=test", logInfo.ToString());
        }
    }
}
