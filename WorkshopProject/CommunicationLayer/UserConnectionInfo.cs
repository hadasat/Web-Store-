using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.CommunicationLayer.Server;
using WorkshopProject.System_Service;

namespace WorkshopProject.CommunicationLayer
{
    class UserConnectionInfo
    {
        bool isSecureConnection;
        UserInterface user;
        uint id;

        public UserConnectionInfo (bool isSecureConnection, uint id)
        {
            this.isSecureConnection = isSecureConnection;
            this.id = id;
            user = new SystemServiceImpl();
        }

        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult) { }
        public void onClose() { }
    }
}
