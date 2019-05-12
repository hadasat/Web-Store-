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

            private JsonResponse (string type,string info,string data)
            {
                this.type = type;
                this.info = info;
                if (data != null)
                {
                    this.data = data;
                }
                else
                {
                    this.data = "";
                }
            }

            public static JsonResponse generateActionSucces(string data = null)
            {
                return new JsonResponse("action", successResponse, data);
            }

            public static JsonResponse generateActionError(string data)
            {
                if (data == null || data == "")
                {
                    return new JsonResponse("action", errorResponse, "unknown action error occured, please contact suuport");
                }
                return new JsonResponse("action", errorResponse, data);
            }

            public static JsonResponse generateDataSuccess (string data)
            {
                return new JsonResponse("data", successResponse, data);
            }
            public static JsonResponse generateDataFailure(string data)
            {
                if (data == null || data == "")
                {
                    return new JsonResponse("data", errorResponse, "unknown data error occured, please contact suuport");
                }
                return new JsonResponse("data", errorResponse, data);
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
                {"register",registerHandler},
                {"addstore",addStoreHandler},
                {"getstore",getStoreHandler},
                {"getproduct",getProductHandler},
                {"addproducttostore",addProductToStore},
                { "addproducttostock",addProductToStock}
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
                responseObj = JsonResponse.generateActionSucces();
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
                response = JsonResponse.generateActionSucces();
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
                    //register without birthdate
                    registrAns = user.Register(userName, password);
                }
                if (registrAns)
                {
                    response = JsonResponse.generateActionSucces();
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

        private void addStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            string storeName = (string)msgObj["data"]["name"];
            try
            {
                int ans = user.AddStore(storeName);
                response = JsonResponse.generateActionSucces(ans.ToString());
            }catch (Exception e)
            {
                response = JsonResponse.generateActionError(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int storeId = (int)msgObj["data"]["storeId"];
            try
            {
                string jsonStore = user.GetStore(storeId);
                response = JsonResponse.generateDataSuccess(jsonStore);
            } catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getProductHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int productId = (int)msgObj["data"]["productId"];
            try
            {
                string jsonProduct = user.GetProductInfo(productId);
                response = JsonResponse.generateDataSuccess(jsonProduct);
            }
            catch(Exception e)
            {
                response = JsonResponse.generateDataFailure(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void addProductToStore(JObject msgObj, string message)
        {
            JsonResponse response;
            int storeId = (int)msgObj["data"]["storeId"];
            string productName = (string)msgObj["data"]["name"];
            string description = (string)msgObj["data"]["description"];
            double price = (double)msgObj["data"]["price"];
            string category = (string)msgObj["data"]["category"];
            try
            {
                int ans = user.AddProductToStore(storeId, productName, description, price, category);
                response = JsonResponse.generateActionSucces(ans.ToString());
            }catch(Exception e)
            {
                response = JsonResponse.generateActionError(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }
        private void addProductToStock(JObject msgObj, string message)
        {
            JsonResponse response;
            int storeId = (int)msgObj["data"]["storeId"];
            int productId = (int)msgObj["data"]["productId"];
            int amount = (int)msgObj["data"]["amount"];

            try
            {
                bool ans = user.AddProductToStock(storeId, productId, amount);
                if (ans)
                {
                    response = JsonResponse.generateActionSucces();
                }
                else
                {
                    response = JsonResponse.generateActionError("can't add product to stock");
                }
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }


        #endregion

    }
}
