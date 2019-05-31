using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Communication.Server
{
    interface IWebSocketMessageSender
    {
        Task sendMessageToUser(string msg, uint id);

        Task sendHttpToExternal(Dictionary<string,string> info, externalRequestType requestType);
    }
}
