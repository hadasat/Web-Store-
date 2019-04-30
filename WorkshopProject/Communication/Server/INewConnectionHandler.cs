using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Communication.Server
{
    public interface INewConnectionHandler
    {
        Task<string> httpNewConnectionHandler(HttpListenerContext context);
        Task<IWebScoketHandler> webSocketNewConnectionHandler(WebSocketContext context, uint id);
    }
}
