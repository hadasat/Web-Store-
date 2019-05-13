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
            public static readonly string successResponse = "success";
            public static readonly string errorResponse = "error";

            public string type { get; set; } = "";
            public string info { get; set; } = "";
            public string data { get; set; } = "";
            public string requestId { get; set; } = "";

            public static JsonResponse generateActionSucces(string requestId, string data = null)
            {
                JsonResponse responseObj = new JsonResponse();
                responseObj.type = "action";
                responseObj.info = JsonResponse.successResponse;
                responseObj.requestId = requestId;
                if (data != null)
                {
                    responseObj.data = data;
                }
                return responseObj;
            }

            public static JsonResponse generateActionError(string data)
            {
                JsonResponse responseObj = new JsonResponse();
                responseObj.info = JsonResponse.errorResponse;
                if (data == null)
                {
                    responseObj.data = "unknown error occured please contact support";
                }
                else
                {
                    responseObj.data = data;
                }

                return responseObj;
            }
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
                {"signin",signInHandler},
                {"signout",signOutHandler},
                {"register",registerHandler }
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
        public void onClose() {
            bool ans = user.unSubscribeAsObserver(this);
            if (!ans)
            {
                Logger.Log("file", logLevel.ERROR, "couldn't unsubscribe observer");
            }
        }



        public void update(List<string> messages)
        {
            if (messages != null)
            {
                foreach(string curr in messages)
                {
                    var notificationObj = new { type = "notification", data = curr };
                    string msgToSend = JsonHandler.SerializeObject(notificationObj);
                    msgSender.sendMessageToUser(msgToSend,id);
                }
            }
        }

        private void sendMyselfAMessage(string msg)
        {
            msgSender.sendMessageToUser(msg, id);
        }

        // ***************** handlers ****************

        #region requests handlers
        
        private void signInHandler(JObject msgObj, string message)
        {
            JsonResponse responseObj;
            string userName = (string)msgObj["data"]["name"];
            string password = (string)msgObj["data"]["password"];
            string ans = user.login(userName, password);
            if (ans == LoginProxy.successMsg)
            {
                responseObj = JsonResponse.generateActionSucces((string)msgObj["id"]);
                user.subscribeAsObserver(this);
            }
            else
            {
                responseObj = JsonResponse.generateActionError(ans);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(responseObj));
        }

        private void  signOutHandler(JObject msgObj, string message)
        {
            //remove observer
            user.unSubscribeAsObserver(this);
            //logout
            JsonResponse response;
            try {
                bool logoutAns = user.logout();
                if (!logoutAns)
                {
                    response = JsonResponse.generateActionError( "can't logout, due to unknow error. please contact support");
                }
                response = JsonResponse.generateActionSucces((string)msgObj["id"]);
            }
            catch(Exception e)
            {
                response = JsonResponse.generateActionError(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void registerHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            string userName = (string)msgObj["data"]["name"];
            string password = (string)msgObj["data"]["password"];
            string birthDateString = (string)msgObj["data"]["birthdate"];
            string country = (string)msgObj["data"]["country"];
            DateTime birthDate = DateTime.MaxValue ;
            if (birthDateString != null)
            {
                birthDate = DateTime.ParseExact(birthDateString, "dd-mm-yyyy", null);
            }
            try
            {
                bool registrAns;
                if (birthDate != DateTime.MaxValue)
                {
                    //has birth date
                    registrAns = user.Register(userName, password, birthDate, country);
                }
                else
                {
                    registrAns = user.Register(userName, password);
                }
                if (registrAns)
                {
                    response = JsonResponse.generateActionSucces((string)msgObj["id"]);
                }
                else
                {
                    response = JsonResponse.generateActionError("can't register due to an unknown reason");
                }
            }catch(Exception e)
            {
                response = JsonResponse.generateActionError(e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }
        #endregion

    }
}
