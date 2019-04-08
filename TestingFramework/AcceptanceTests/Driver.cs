using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingFramework.AcceptanceTests
{
    public class Driver
    {
        //This function returns the current implementation of IServiceBridge
        public static IServiceBridge getBridge()
        {
            return new ServiceProxyBridge();
        }

    }
}
