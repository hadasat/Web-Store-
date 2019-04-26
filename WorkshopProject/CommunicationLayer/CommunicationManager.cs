using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkshopProject.ClientPages;
using WorkshopProject.CommunicationLayer.Server;

namespace WorkshopProject.CommunicationLayer
{
    class CommunicationManager : INewConnectionHandler, IWebScoketHandler
    {
        private IServer server;
        private ConcurrentDictionary<uint, UserConnectionInfo> connections;

        public CommunicationManager()
        {
            List<string> prefixes = new List<string>();
            prefixes.Add("http://localhost:8080/wot/");
            prefixes.Add("https://localhost:8443/wot/");
            connections = new ConcurrentDictionary<uint, UserConnectionInfo> ();
            using (server = new Server.Server())
            {
                new Task(() => { server.start(prefixes,this); }).Start();
                Thread.Sleep(150);
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
        public async Task<string> httpNewConnectionHandler(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;  
            bool isSecureConnection = request.IsSecureConnection; //get if connection is secured or not
            //get relevant html pgae code
            string requestedPage = request.RawUrl;

            return HtmlPageManager.findPageByName(isSecureConnection, requestedPage);
        }

        public async Task<IWebScoketHandler> webSocketNewConnectionHandler(WebSocketContext context, uint id)
        {
            bool isSecureConnection = context.IsSecureConnection;
            UserConnectionInfo userConnection = new UserConnectionInfo(isSecureConnection, id);
            bool addAns = connections.TryAdd(id, userConnection);
            if (!addAns)
            {
                Console.WriteLine("error in adding to dictionary in Communication manager");
                return null;
            }
            return this;
        }

        public void onClose(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId)
        {
            UserConnectionInfo userConnection;
            connections.TryRemove(myId, out userConnection);
            userConnection.onClose();
        }

        public void onCloseError(uint myId)
        {
            UserConnectionInfo userConnection;
            connections.TryRemove(myId, out userConnection);
            userConnection.onClose();
        }

        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult, uint myId)
        {
            UserConnectionInfo userConnection = connections[myId];
            userConnection.onMessage(bufferCollector, receiveResult);
        }

        #endregion      
    }
}
