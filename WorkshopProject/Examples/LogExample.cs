using log4net;
using System;

namespace WorkshopProject.Examples
{
    class LogExample
    {
        private static ILog myLogger = LogManager.GetLogger("file");

        public static void RunMe()
        {
           // Console.ReadLine();
            myLogger.Info("somethingnewnew");
            
        }

    }
}
