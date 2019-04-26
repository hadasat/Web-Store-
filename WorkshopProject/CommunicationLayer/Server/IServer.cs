using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.CommunicationLayer.Server
{
    interface IServer : IDisposable
    {
        void start(List<string> prefixes, INewConnectionHandler newConnectionHandler);
        /// <summary>
        /// get id of all currently active web socket connections
        /// </summary>
        /// <returns></returns>
        List<uint> getAllActiveConnectionsId();

        #region message sending
        /// <summary>
        /// sends message to client
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <returns>void</returns>
        Task sendMessageById(uint id, byte[] msg, WebSocketMessageType msgType);
        /// <summary>
        /// wrapper for sending strings
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <returns>void</returns>
        Task sendMessageById(uint id, string msg, WebSocketMessageType msgType = WebSocketMessageType.Text);

        /// <summary>
        /// wrapper for sending strings to all
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <returns>void</returns>
        Task sendMessageToAll(string msg, WebSocketMessageType msgType = WebSocketMessageType.Text);

        /// <summary>
        /// actually sends message to all users
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <returns>void</returns>
        Task sendMessageToAll(byte[] msg, WebSocketMessageType msgType);

        /// <summary>
        /// sends message to requested id list
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="msg"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        Task sendMessageToList(List<uint> idList, byte[] msg, WebSocketMessageType msgType);
        #endregion
    }
}
