using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LoggerTests6Fatal
    {
        TestLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
        }

        [Test]
        public void Fatal_OperationIsPopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Fatal(operation);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine = $"Operation={operation.Name},Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, ex);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine = $"Operation={operation.Name},{details.Key.Name}={details.Value}";
            
            logger.Fatal(operation, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Fatal(operation, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name}";
            
            logger.Fatal(operation, status);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, ex);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Fatal(operation, status, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Fatal(operation, status, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name}," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_MessageIsPopulated_LogsCorrectly()
        {
            string message = "țestoasă";

            string expectedLogLine = $"Message={message}";
            
            logger.Fatal(message);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_MessageAndExceptionArePopulated_LogsCorrectly()
        {
            string message = "țestoasă";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(message, ex);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},Message={message}";
            
            logger.Fatal(operation, null, message);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, null, message, ex);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Fatal(operation, null, message, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Fatal(operation, null, message, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, null, message, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndMessageAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, null, message, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name},Message={message}";
            
            logger.Fatal(operation, status, message);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, message, ex);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Fatal(operation, status, message, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Fatal(operation, status, message, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, message, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Fatal_OperationAndOperationStatusAndMessageAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Fatal(operation, status, message, ex, details);

            Assert.AreEqual(LogLevel.Fatal, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }
    }
}
