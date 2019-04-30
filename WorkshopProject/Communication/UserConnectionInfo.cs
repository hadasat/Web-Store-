using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Communication.Server;
using WorkshopProject.System_Service;

namespace WorkshopProject.Communication
{
    class UserConnectionInfo
    {
        bool isSecureConnection;
        UserInterface user;
        uint id;
        IWebScoketHandler msgSender;

        public UserConnectionInfo(bool isSecureConnection, uint id, IWebScoketHandler msgSender)
        {
            this.isSecureConnection = isSecureConnection;
            this.id = id;
            user = new SystemServiceImpl();
            this.msgSender = msgSender;
        }

        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult) { }
        public void onClose() { }
    }
}
