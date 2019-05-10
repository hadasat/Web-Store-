using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkshopProject.Client;
using WorkshopProject.Communication.Server;
using WorkshopProject.Log;

namespace WorkshopProject.Communication
{
    class CommunicationManager : INewConnectionHandler, IWebScoketHandler, IWebSocketMessageSender
    {
        private IServer server;
        private ConcurrentDictionary<uint, UserConnectionInfo> activeConnections;
        private ConcurrentDictionary<uint, UserConnectionInfo> oldConnections;

        public CommunicationManager()
        {
            List<string> prefixes = new List<string>();
            prefixes.Add("http://localhost:8080/wot/");
            prefixes.Add("https://localhost:8443/wot/");
            activeConnections = new ConcurrentDictionary<uint, UserConnectionInfo>();
            oldConnections = new ConcurrentDictionary<uint, UserConnectionInfo>();
            using (server = new Server.Server())
            {
                new Task(() => { server.start(prefixes, this); }).Start();
                //Thread.Sleep(150);
                string send = null;
                while (send != "exit")
                {
                    send = Console.ReadLine();
                    if (send == "exit")
                    {
                        server.Dispose();
                    }
                }
            }
        }

        #region interfaces implementation
        public string httpNewConnectionHandler(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            bool isSecureConnection = request.IsSecureConnection; //get if connection is secured or not
            //get relevant html pgae code
            string requestedPage = request.RawUrl;

            return HtmlPageManager.findPageByName(requestedPage);
        }

        public IWebScoketHandler webSocketNewConnectionHandler(WebSocketContext context, uint id)
        {
            bool isSecureConnection = context.IsSecureConnection;
            UserConnectionInfo userConnection;
            oldConnections.TryRemove(id, out userConnection);             //try to get old connection
            if (userConnection == null) // if no old connection found create new object
            {
                userConnection = new UserConnectionInfo(isSecureConnection, id, this);
            }
            bool addAns = activeConnections.TryAdd(id, userConnection);
            if (!addAns)
            {
                //Console.WriteLine("error in adding to dictionary in Communication manager");
                Logger.Log("file", logLevel.ERROR, "error in adding new connection to dictionary in Communication manager");
                return null;
            }

            return this;
        }

        public void onClose(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId)
        {
            onCloseShared(myId);
        }

        public void onCloseError(uint myId)
        {
            onCloseShared(myId);
        }

        private void onCloseShared(uint myId)
        {
            UserConnectionInfo userConnection;
            activeConnections.TryRemove(myId, out userConnection);
            oldConnections.TryAdd(myId, userConnection);
            userConnection.onClose();
        }

        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId)
        {
            UserConnectionInfo userConnection = activeConnections[myId];
            userConnection.onMessage(bufferCollector, receiveResult);
        }

        public async Task sendMessageToUser(string msg, uint id)
        {
            await server.sendMessageById(id, msg);
        }

        #endregion
    }
}
