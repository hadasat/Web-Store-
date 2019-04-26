using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.CommunicationLayer.Server
{
    interface INewConnectionHandler
    {
        Task<string> httpNewConnectionHandler(HttpListenerContext context);
        Task<IWebScoketHandler> webSocketNewConnectionHandler(WebSocket ws, uint id);
    }
}
