using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LoggerTests5Error
    {
        TestLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
        }

        [Test]
        public void Error_OperationIsPopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Error(operation);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, ex);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine = $"Operation={operation.Name},{details.Key.Name}={details.Value}";
            
            logger.Error(operation, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Error(operation, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Error(operation, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndExceptionAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, ex, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Error(operation, status);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, ex);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Error(operation, status, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Error(operation, status, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Error(operation, status, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndExceptionAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, ex, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_MessageIsPopulated_LogsCorrectly()
        {
            string message = "țestoasă";

            string expectedLogLine = $"Message={message}";
            
            logger.Error(message);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_MessageAndExceptionArePopulated_LogsCorrectly()
        {
            string message = "țestoasă";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(message, ex);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},Message={message}";
            
            logger.Error(operation, null, message);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, null, message, ex);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Error(operation, null, message, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Error(operation, null, message, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Error(operation, null, message, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, null, message, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, null, message, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndMessageAndExceptionAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, null, message, ex, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Error(operation, status, message);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, message, ex);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{details.Key.Name}={details.Value}";
            
            logger.Error(operation, status, message, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Error(operation, status, message, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Error(operation, status, message, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndExceptionAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo details = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{details.Key.Name}={details.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, message, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndExceptionAndIEnumerableDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, message, ex, details);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Error_OperationAndOperationStatusAndMessageAndExceptionAndIEnumerableDetailsAndParamsDetailsArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> details = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo details2 = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace={ex.StackTrace}";
            
            logger.Error(operation, status, message, ex, details, details2);

            Assert.AreEqual(LogLevel.Error, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }
    }
}
