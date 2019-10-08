using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LoggerTests2Debug
    {
        TestLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
        }

        [Test]
        public void Debug_OperationIsPopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Debug(operation);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, ex);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Debug(operation, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine = $"Operation={operation.Name},{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Debug(operation, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Debug(operation, status);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, ex);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Debug(operation, status, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Debug(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, status, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, status, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionAndLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_MessageIsPopulated_LogsCorrectly()
        {
            string message = "țestoasă";

            string expectedLogLine = $"Message={message}";
            
            logger.Debug(message);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_MessageAndExceptionArePopulated_LogsCorrectly()
        {
            string message = "țestoasă";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(message, ex);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},Message={message}";
            
            logger.Debug(operation, null, message);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Debug(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Debug(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Debug(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Debug(operation, null, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, null, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, null, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, null, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, null, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, null, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Debug(operation, status, message);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, message, ex);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Debug(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Debug(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Debug(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Debug(operation, status, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, status, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Debug(operation, status, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Debug(operation, status, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message},StackTrace=<NULL>";
            
            logger.Debug(operation, status, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Debug_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
            
            logger.Debug(operation, status, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Debug, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }
    }
}
