using WorkshopProject.Log;

namespace WorkshopProject.Examples
{
    class LogExample
    {
        //private static ILog myLogger = LogManager.GetLogger("file");

        public static void RunMe()
        {
            // Console.ReadLine();
            //myLogger.Info("somethingnewnew");
            Logger.Log("file", logLevel.INFO, "this message will be logged");
        }

    }
}
