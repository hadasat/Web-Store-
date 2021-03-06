﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Log;

namespace WorkshopProject.Communication.Server
{
    //  https://github.com/paulbatum/WebSocket-Samples 
    //  https://github.com/paulbatum/WebSocket-Samples
    // http://www.bouncycastle.org/csharp/
    // https://benjii.me/2017/06/creating-self-signed-certificate-identity-server-azure/
    //https://stackoverflow.com/a/33905011/9608607
    //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-configure-a-port-with-an-ssl-certificate
    //https://gist.github.com/NickCraver/2f004349401822b74db5



    public class Server : IServer
    {

        #region fields
        private static int mAX_BUFFER_SIZE = 1024;
        public static int MAX_BUFFER_SIZE { get => mAX_BUFFER_SIZE; set => mAX_BUFFER_SIZE = value; }

        private ConcurrentDictionary<uint, WebSocket> activeConections;
        private int connectionCounter;
        private HttpListener listener;
        private bool disposed;
        private INewConnectionHandler newConnectionHandler;



        #endregion

        #region constructor
        public Server()
        {
            connectionCounter = 0;
            listener = null;
            disposed = false;
            activeConections = new ConcurrentDictionary<uint, WebSocket>();

        }
        #endregion

        #region public methods

        public void start(List<string> prefixes, INewConnectionHandler newConnectionHandler)
        {
            if (disposed) return;

            this.newConnectionHandler = newConnectionHandler;
            listener = new HttpListener();
            string allUrl = "";
            foreach (string url in prefixes)
            {
                listener.Prefixes.Add(url);
                allUrl += url + " & ";
            }
            allUrl = allUrl.Substring(0, allUrl.Length - 2);
            listener.Start();
            Console.WriteLine("server listening on: {0}", allUrl);

            while (listener.IsListening)
            {
                var context = listener.BeginGetContext(newConnectionCallback, null);
                context.AsyncWaitHandle.WaitOne();
            }
        }

        public async Task sendMessageById(uint id, byte[] msg, WebSocketMessageType msgType)
        {
            if (disposed) return;

            WebSocket ws = activeConections[id];
            await ws.SendAsync(new ArraySegment<byte>(msg), msgType, true, CancellationToken.None);
        }


        public async Task sendMessageById(uint id, string msg, WebSocketMessageType msgType = WebSocketMessageType.Text)
        {
            await sendMessageById(id, Encoding.GetEncoding("UTF-8").GetBytes(msg), msgType);
        }

        public async Task sendMessageToAll(string msg, WebSocketMessageType msgType = WebSocketMessageType.Text)
        {
            await sendMessageToAll(Encoding.GetEncoding("UTF-8").GetBytes(msg), msgType);
        }

        public async Task sendMessageToAll(byte[] msg, WebSocketMessageType msgType)
        {
            foreach (var currPair in activeConections)
            {
                uint id = currPair.Key;
                await sendMessageById(id, msg, msgType);
            }
        }


        public async Task sendMessageToList(List<uint> idList, byte[] msg, WebSocketMessageType msgType)
        {
            foreach (uint id in idList)
            {
                await sendMessageById(id, msg, msgType);
            }
        }

        public async Task sendMessageToList(List<uint> idList, string msg, WebSocketMessageType msgType = WebSocketMessageType.Text)
        {
            await sendMessageToList(idList, Encoding.GetEncoding("UTF-8").GetBytes(msg), msgType);
        }

        public void Dispose()
        {
            if (disposed) return;
            closeServer();
        }

        public List<uint> getAllActiveConnectionsId()
        {
            List<uint> ans = new List<uint>();
            foreach (var currPair in activeConections)
            {
                uint id = currPair.Key;
                ans.Add(id);
            }
            return ans;
        }



        #endregion

        #region private methods

        /// <summary>
        /// callback when new connection is established
        /// handles the creattion of http and ws connections
        /// </summary>
        /// <param name="ar"></param>
        private async void newConnectionCallback(IAsyncResult ar)
        {
            if (disposed) return;

            HttpListenerContext context;
            bool isNewCoonection;
            try
            {
                context = listener.EndGetContext(ar);
            }
            catch (WorkShopDbException dbExc)
            {
                throw dbExc;
            }
            catch (Exception ignore)
            {
                //Console.WriteLine("test - problem in connecting \n");
                Logger.Log("event", logLevel.WARN, "listener couldn't get end of context");
                return;
            }
            if (context.Request.IsWebSocketRequest)
            {
                // get new connection
                HttpListenerWebSocketContext wsContext;
                WebSocket ws;
                uint newConnectionId;

                try
                {
                    //get new connection
                    wsContext = await context.AcceptWebSocketAsync(subProtocol: "new");
                    //Console.WriteLine("new connection");
                    isNewCoonection = true;
                    newConnectionId = incermentCounter();
                    ws = wsContext.WebSocket;
                    //add connection to dictionary
 
                }
                catch (WorkShopDbException dbExc)
                {
                    throw dbExc;
                }
                catch (WebSocketException ignoreWSE)
                {
                    //get old connection
                    wsContext = await context.AcceptWebSocketAsync(subProtocol: "old");
                    //Console.WriteLine("old connection");
                    isNewCoonection = false;
                    ws = wsContext.WebSocket;
                    byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
                    try
                    {
                        WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(recvBuffer), CancellationToken.None);
                        string oldConnectionInfo = Encoding.UTF8.GetString(recvBuffer, 0, receiveResult.Count);
                        Regex pattern = new Regex(@"\d+");
                        Match match = pattern.Match(oldConnectionInfo);
                        if (match.Success)
                        {
                            newConnectionId = Convert.ToUInt32(match.Value);
                        }
                        else
                        {
                            Logger.Log("event", logLevel.DEBUG, "error getting old id establishing new");
                            newConnectionId = incermentCounter();
                        }
                    }
                    catch (WorkShopDbException dbExc)
                    {
                        throw dbExc;
                    }
                    catch (Exception ignore)
                    {
                        Logger.Log("event", logLevel.DEBUG, "error receiveing connection id for old connections");
                        return;
                    }
                }


                //handle new connection
                bool ans = activeConections.TryAdd(newConnectionId, ws);
                if (!ans)
                {
                    //Console.WriteLine("couldn't add new web socket connection to connection dictionary");
                    Logger.Log("error", logLevel.ERROR, "couldn't add new web socket connection to connection dictionary");
                    return;
                }

                IWebScoketHandler wsh = newConnectionHandler.webSocketNewConnectionHandler(wsContext, newConnectionId);

                if (isNewCoonection) //if we establish a connection to a new user we need to send him is id
                {
                    var infoToSend = new { type = "setId", data = newConnectionId ,requestId = -2};
                    await sendMessageById(newConnectionId, JsonHandler.SerializeObject(infoToSend));
                }


                if (wsh != null)
                {
                    webSocketConnectionLoop(ws, wsh, newConnectionId);
                }
                else
                {
                    activeConections.TryRemove(newConnectionId, out ws);
                    //Console.WriteLine("unsuccessful creatio of new web socket handler");
                    Logger.Log("error", logLevel.ERROR, "unsuccessful creation of new web socket handler, recievd null handler");
                }
            }
            else
            {
                string htmlPageToSend = newConnectionHandler.httpNewConnectionHandler(context);
                //return the html page received
                context.Response.ContentType = "text/html";
                try
                {
                    using (Stream output = context.Response.OutputStream)
                    {
                        //convert the html page to bytes and send back
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(htmlPageToSend);
                        output.Write(buffer, 0, buffer.Length);
                        output.Flush();
                    }
                }
                catch
                {
                    //ignore
                }
            }
        }

        private async void webSocketConnectionLoop(WebSocket ws, IWebScoketHandler wsHandler, uint myId)
        {
            if (disposed) return;

            while (ws.State == WebSocketState.Open)
            {
                WebSocketReceiveResult receiveResult = null;
                List<byte[]> bufferCollector = new List<byte[]>();
                while (receiveResult == null || receiveResult.EndOfMessage == false)
                {
                    byte[] recvBuffer = new byte[MAX_BUFFER_SIZE];
                    try
                    {
                        receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(recvBuffer), CancellationToken.None);
                    }
                    catch (WorkShopDbException dbExc)
                    {
                        throw dbExc;
                    }
                    catch (Exception ignore)
                    {
                        //Console.WriteLine("test 1");
                        Logger.Log("event", logLevel.DEBUG, "closed the server while some websockets are open");
                        wsHandler.onCloseError(myId);
                        return;
                    }
                    bufferCollector.Add(recvBuffer);
                }

                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    wsHandler.onClose(bufferCollector, receiveResult, myId);   
                    closeWebSocketConnection(myId);
                }
                else
                {
                    wsHandler.onMessage(bufferCollector, receiveResult, myId);
                }
            }
        }

        private void closeWebSocketConnection(uint myId)
        {
            WebSocket removed;
            bool removeAns = activeConections.TryRemove(myId, out removed);
            if (removed == null || !removeAns)
            {
                //Console.WriteLine("error removing form connection list, id:{0}", myId);
                Logger.Log("error", logLevel.ERROR, "error removing form connection list, id: " + myId);
                return;
            }
            removed.Dispose();
        }

        private void closeServer()
        {
            if (disposed)
            {
                return;
            }
            listener.Stop();
            listener.Close();
            foreach (var currPair in activeConections)
            {
                WebSocket currWs = currPair.Value;
                currWs.CloseAsync(WebSocketCloseStatus.NormalClosure, "server closed", CancellationToken.None);
                //currWs.Dispose();
            }
            disposed = true;
        }

        private uint incermentCounter()
        {
            int newValue = Interlocked.Increment(ref connectionCounter);
            return unchecked((uint)newValue);
        }
        #endregion
    }
}
