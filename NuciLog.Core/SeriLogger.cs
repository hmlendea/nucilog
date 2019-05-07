using System;
using System.Collections.Generic;

using Serilog;

namespace NuciLog.Core
{
    public sealed class SeriLogger : Logger
    {
        Serilog.ILogger logger;

        public SeriLogger()
        {
            this.logger = Log.Logger;
        }

        public override void SetSourceContext(Type type)
        {
            this.logger = Log.Logger.ForContext(type);
            base.SetSourceContext(type);
        }

        protected override void WriteLog(LogLevel level, string logMessage)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    logger.Verbose(logMessage);
                    break;

                case LogLevel.Debug:
                    logger.Debug(logMessage);
                    break;

                case LogLevel.Information:
                    logger.Information(logMessage);
                    break;

                case LogLevel.Warning:
                    logger.Warning(logMessage);
                    break;

                case LogLevel.Error:
                    logger.Error(logMessage);
                    break;

                case LogLevel.Fatal:
                    logger.Fatal(logMessage);
                    break;
            }
        }
    }
}
