using WorkshopProject.Log;

namespace WorkshopProject.Examples
{
    public static class LogExample
    {
        public static void RunMe()
        {
            //To log simply 

            /* To Log, simply import the "WorkshopProject.Log" namespace and call the static method:
             * static void Log(string logger, logLevel loglevel, string message)
             * 
             * Parameters
             * logger : we idenitfy different loggers by the name (as a string).
             * logLevel : here we decide what level the message will be (for filtering options):
             *       logLevel.INFO
             *       logLevel.DEBUG
             *       logLevel.WARN
             *       logLevel.ERROR
             *       logLevel.FATAL
             *  message : the string we want to add the log
             */

            //this message will be logged in the basic log ->    \bin\Log\WorskhopProject.log
            Logger.Log("file", logLevel.INFO, "this message will be logged in the basic log");

            //this message will be logged in the event log ->    \bin\Log\Events.log
            Logger.Log("event", logLevel.INFO, "this message will be logged under event");

            //this message will be logged in the error log ->    \bin\Log\Errors.log
            Logger.Log("error", logLevel.ERROR, "this message will be logged under error");
        }

    }
}
