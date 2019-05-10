using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Communication.Server;
using WorkshopProject.Log;
using WorkshopProject.System_Service;

namespace WorkshopProject.Communication
{

    class UserConnectionInfo : IObserver
    {
        internal class JsonResponse
        {
            public string type { get; set; } = "";
            public string info { get; set; } = "";
            public string data { get; set; } = "";
        }

        private bool isSecureConnection;
        private LoginProxy user;
        private uint id;
        private IWebSocketMessageSender msgSender;

        private Dictionary<string, Action<JObject,string>> messageHandlers;

        public UserConnectionInfo(bool isSecureConnection, uint id, IWebSocketMessageSender msgSender)
        {
            this.isSecureConnection = isSecureConnection;
            this.id = id;
            user = new LoginProxy();
            this.msgSender = msgSender;
            messageHandlers = new Dictionary<string, Action<JObject, string>>()
            {
                { "signin",signInHandler}
            };
        }

        /// <summary>
        /// on message event activated when the user recieves message from server
        /// </summary>
        /// <param name="bufferCollector"></param>
        /// <param name="receiveResult"></param>
        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult) {
            string message = "";
            //convert to string
            for (int i = 0; i < bufferCollector.Count - 1; i++)
            {
                message += Encoding.UTF8.GetString(bufferCollector[i]);
            }
            message += Encoding.UTF8.GetString(bufferCollector[bufferCollector.Count - 1], 0, receiveResult.Count);

            JObject messageObj = JObject.Parse(message);

            string messageType =((string)messageObj["info"]).ToLower();
            if (messageHandlers.ContainsKey(messageType))
            {
                messageHandlers[messageType](messageObj, message);
            }
            else
            {
                Logger.Log("file", logLevel.WARN, "received an unknown type of message from client");
            }
            

        }
        /// <summary>
        /// on close event activated when the connection is closed
        /// </summary>
        public void onClose() { }


        public void update(List<string> messages)
        {
            if (messages == null) { return; }

            foreach(string currMsg in messages)
            {
                var notification = new { type = "notification", data = currMsg } ;
                string jsonMsg = JsonHandler.SerializeObject(notification);
                msgSender.sendMessageToUser(jsonMsg, id);
            }
        }

        // ***************** handlers ****************


        private void signInHandler(JObject msgObj, string message)
        {
            JsonResponse responseObj = new JsonResponse();
            responseObj.type = "action";
            string userName = (string)msgObj["data"]["name"];
            string password = (string)msgObj["data"]["password"];
            string ans = user.login(userName, password);
            if (ans == LoginProxy.successMsg)
            {
                responseObj.info = "success";
            }
            else
            {
                responseObj.info = "failure";
                responseObj.data = ans;
            }
            msgSender.sendMessageToUser(JsonHandler.SerializeObject(responseObj), id);
        }
    }
}
