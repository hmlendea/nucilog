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
        public void Build_OperationAndOperationStatusArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            string actual = LogMessageBuilder.Build(operation, status, null, null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndExceptionArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndNullLogInfosArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            string actual = LogMessageBuilder.Build(operation, status, message: null, exception: null, logInfos: null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndDuplicatedLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndExceptionAndNullLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndExceptionAndLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndExceptionAndDuplicatedLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expected = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            string actual = LogMessageBuilder.Build(operation, status, message, null, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndExceptionArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndNullLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndDuplicatedLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosArePopulated_ReturnsTheCorrectValue()
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
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndDuplicatedLogInfosArePopulated_ReturnsTheCorrectValue()
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
