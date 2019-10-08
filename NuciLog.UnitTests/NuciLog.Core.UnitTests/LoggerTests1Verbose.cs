using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciLog.Core;
using NuciLog.Core.UnitTests.Helpers;

namespace NuciLog.Core.UnitTests
{
    public sealed class LoggerTests1Verbose
    {
        TestLogger logger;

        [SetUp]
        public void SetUp()
        {
            logger = new TestLogger();
        }

        [Test]
        public void Verbose_OperationIsPopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Verbose(operation);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;

            string expectedLogLine = $"Operation={operation.Name}";
            
            logger.Verbose(operation, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine = $"Operation={operation.Name},{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Verbose(operation, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine = $"Operation={operation.Name},{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Verbose(operation, status);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}";
            
            logger.Verbose(operation, status, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Verbose(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, status, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, status, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, status, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()}," +
                $"Message=An exception has occurred," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_MessageIsPopulated_LogsCorrectly()
        {
            string message = "țestoasă";

            string expectedLogLine = $"Message={message}";
            
            logger.Verbose(message);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_MessageAndExceptionArePopulated_LogsCorrectly()
        {
            string message = "țestoasă";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(message, ex);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},Message={message}";
            
            logger.Verbose(operation, null, message);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Verbose(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Verbose(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Verbose(operation, null, message, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, null, message, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}";
            
            logger.Verbose(operation, null, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, null, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, null, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, null, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, null, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine = $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Verbose(operation, status, message);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Verbose(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}";
            
            logger.Verbose(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Verbose(operation, status, message, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, status, message, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}";
            
            logger.Verbose(operation, status, message, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, status, message, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest";
            
            logger.Verbose(operation, status, message, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest,{TestLogInfoKey.TestKey2.Name}=teeest2";
            
            logger.Verbose(operation, status, message, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo logInfos = new LogInfo(TestLogInfoKey.TestKey, "teeest");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{logInfos.Key.Name}={logInfos.Value}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos: null, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndNullLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            LogInfo extraLogInfos = new LogInfo(TestLogInfoKey.TestKey2, "teeest2");

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey2.Name}=teeest2," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos: null, extraLogInfos: extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndNullExtraLogInfosArePopulated_LogsCorrectly()
        {
            Operation operation = Operation.StartUp;
            OperationStatus status = OperationStatus.Started;
            string message = "testudo";
            Exception ex = new Exception();
            IEnumerable<LogInfo> logInfos = new List<LogInfo> { new LogInfo(TestLogInfoKey.TestKey, "teeest") };

            string expectedLogLine =
                $"Operation={operation.Name},OperationStatus={status.Name.ToUpper()},Message={message}," +
                $"{TestLogInfoKey.TestKey.Name}=teeest," +
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos, extraLogInfos: null);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }

        [Test]
        public void Verbose_OperationAndOperationStatusAndMessageAndExceptionAndLogInfosAndExtraLogInfosArePopulated_LogsCorrectly()
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
                $"Exception={ex.GetType()},ExceptionMessage={ex.Message}";
            
            logger.Verbose(operation, status, message, ex, logInfos, extraLogInfos);

            Assert.AreEqual(LogLevel.Verbose, logger.LastLogLevel);
            Assert.AreEqual(expectedLogLine, logger.LastLogLine);
        }
    }
}
