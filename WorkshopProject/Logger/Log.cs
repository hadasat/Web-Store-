using log4net;

namespace WorkshopProject.Log
{

    public enum logLevel { DEBUG, INFO, WARN, ERROR, FATAL }


    public class Logger
    {
        //TODO: Logger - make sure this does not cause any slowness due to long logger construction time
        public static void Log(string logger, logLevel loglevel, string message)
        {
            ILog loggerObj = LogManager.GetLogger(logger);

            switch (loglevel)
            {
                case logLevel.DEBUG:
                    loggerObj.Debug(message);
                    break;

                case logLevel.INFO:
                    loggerObj.Info(message);
                    break;

                case logLevel.WARN:
                    loggerObj.Warn(message);
                    break;

                case logLevel.ERROR:
                    loggerObj.Error(message);
                    break;

                case logLevel.FATAL:
                    loggerObj.Fatal(message);
                    break;
            }
        }


    }
}
