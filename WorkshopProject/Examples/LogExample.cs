using WorkshopProject.Log;

namespace WorkshopProject.Examples
{
    class LogExample
    {
        public static void RunMe()
        {
            //To log simply 

            /* To Log, simply import the "WorkshopProject.Log" namespace and call the static method:
             * static void Log(string logger, logLevel loglevel, string message)
             * 
             * Parameters
             * logger : we idenitfy different loggers by the name (as a string). Usually it will be "file"
             * logLevel : here we decide what level the message will be (for filtering options):
             *       logLevel.INFO
             *       logLevel.DEBUG
             *       logLevel.WARN
             *       logLevel.ERROR
             *       logLevel.FATAL
             *  message : the string we want to add the log
             */

            Logger.Log("file", logLevel.INFO, "this message will be logged");
        }

    }
}
