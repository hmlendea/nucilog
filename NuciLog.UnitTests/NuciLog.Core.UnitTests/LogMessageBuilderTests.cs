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
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, null, ex, null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndDetailsArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            string actual = LogMessageBuilder.Build(operation, status, null, null, details);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndExceptionAndDetailsArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, null, ex, details);

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
        public void Build_OperationAndOperationStatusAndMessageAndDetailsArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            string actual = LogMessageBuilder.Build(operation, status, message, null, details);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Build_OperationAndOperationStatusAndMessageAndExceptionAndDetailsArePopulated_ReturnsTheCorrectValue()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expected =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            string actual = LogMessageBuilder.Build(operation, status, message, ex, details);

            Assert.AreEqual(expected, actual);
        }
    }
}
