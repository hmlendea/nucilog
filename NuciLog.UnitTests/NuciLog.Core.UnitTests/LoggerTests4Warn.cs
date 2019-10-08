using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LoggerTests4Warn
    {
        TestLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
        }

        [Test]
        public void Warn_OperationIsPopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Warn(operation);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Warn(operation, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine = $"Operation={operation.Name},{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Warn(operation, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Warn(operation, status);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Warn(operation, status, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Warn(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, status, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, status, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_MessageIsPopulated_LogsCorrectly()
        {
            string message = "țestoasă";

            string expectedLogLine = $"Message={message}";
            
            logger.Warn(message);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_MessageAndExceptionArePopulated_LogsCorrectly()
        {
            string message = "țestoasă";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(message, ex);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},Message={message}";
            
            logger.Warn(operation, null, message);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Warn(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Warn(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Warn(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Warn(operation, null, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, null, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, null, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, null, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, null, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Warn(operation, status, message);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Warn(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Warn(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Warn(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Warn(operation, status, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, status, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Warn(operation, status, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Warn(operation, status, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Warn_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Warn(operation, status, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Warn, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }
    }
}
