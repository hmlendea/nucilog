using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LogMessageBuilderTests
    {
        [Test]
        public void Build_OperationIsNull_ValueContainsUnknownOperation()
        {
            string actual = LogMessageBuilder.Build(null, null, null, null, null);

            Assert.AreEqual(string.Empty, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatus_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            string actual = LogMessageBuilder.Build(operation, status, null, null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndException_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: ex, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndNullLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: null, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            string actual = LogMessageBuilder.Build(operation, status, null, null, logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndDuplicatedLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo>
            {
                new LogInfo(TestLogInfoKey.TestKey, "teeest"),
                new LogInfo(TestLogInfoKey.TestKey, "teeest2")
            };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest2";
            string actual = LogMessageBuilder.Build(operation, status, null, null, logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndExceptionAndNullLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: ex, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndExceptionAndLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: ex, logInfos: logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndExceptionAndDuplicatedLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo>
            {
                new LogInfo(TestLogInfoKey.TestKey, "teeest"),
                new LogInfo(TestLogInfoKey.TestKey, "teeest2")
            };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: ex, logInfos: logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_MessageIsPopulated_ReturnsTheCorrectValue()
        {
            string message = "țestoasă";
            string expected = $"Message={message}";
            string actual = LogMessageBuilder.Build(null, null, message, null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessage_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            string actual = LogMessageBuilder.Build(operation, status, message, null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndException_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message, ex, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndNullLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            string actual = LogMessageBuilder.Build(operation, status, message, exception: null, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            string actual = LogMessageBuilder.Build(operation, status, message, exception: null, logInfos: logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndDuplicatedLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo>
            {
                new LogInfo(TestLogInfoKey.TestKey, "teeest"),
                new LogInfo(TestLogInfoKey.TestKey, "teeest2")
            };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest2";
            string actual = LogMessageBuilder.Build(operation, status, message, exception: null, logInfos: logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message, ex, logInfos);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndDuplicatedLogInfos_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo>
            {
                new LogInfo(TestLogInfoKey.TestKey, "teeest"),
                new LogInfo(TestLogInfoKey.TestKey, "teeest2")
            };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message, ex, logInfos);

            Assert.AreEqual(expected, actual);
        }
    }
}
