using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Users;
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

            public string type { get; set; }
            public string info { get; set; }
            public string data { get; set; }
            public int requestId { get; set; }

            private JsonResponse(string type, string info, string data, int requestId)
            {
                this.type = type;
                this.info = info;
                this.requestId = requestId;
                if (data != null)
                {
                    this.data = data;
                }
                else
                {
                    this.data = "";
                }
            }
            public static JsonResponse generateActionSucces(int requestId, string data = null)
            {
                return new JsonResponse("action", successResponse, data, requestId);
            }

            public static JsonResponse generateActionError(int requestId, string data)
            {
                if (data == null || data == "")
                {
                    return new JsonResponse("action", errorResponse, "unknown action error occured, please contact suuport", requestId);
                }
                return new JsonResponse("action", errorResponse, data, requestId);
            }

            public static JsonResponse generateDataSuccess(int requestId, string data)
            {
                return new JsonResponse("data", successResponse, data, requestId);
            }
            public static JsonResponse generateDataFailure(int requestId, string data)
            {
                if (data == null || data == "")
                {
                    return new JsonResponse("data", errorResponse, "unknown data error occured, please contact suuport", requestId);
                }
                return new JsonResponse("data", errorResponse, data, requestId);
            }
        }

        private bool isSecureConnection;
        private LoginProxy user;
        private uint id;
        private IWebSocketMessageSender msgSender;

        private Dictionary<string, Action<JObject, string>> messageHandlers;

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
                {"addproducttostock",addProductToStock},
                {"getallstores",getAllStoresHandler},
                {"addproducttobasket",addProductToBaksetHandler },
                {"getshoppingbasket",getShoppingBasketHandler},
                {"removeproductfromstore",removeProductFromStoreHandler },
                {"changeproductinfo",changeProductInfoHandler },   /// didn't test yet
                {"closestore",closeStoreHandler },
                {"searchproducts",searchProductsHandler },
                {"getallmembers",getAllMembersHandler },
                {"removeuser",removeUserHandler },
                {"getallmanagers",getAllManagersHandler },
                {"approveowenrshiprequest",approveOwnershipResponseHandler },
                {"disapproveowenrshiprequest",disapproveOwnershipResponseHandler },
                {"buyshoppingbasket",buyShoppingBasketHandler }

            };
        }

        /// <summary>
        /// on message event activated when the user recieves message from server
        /// </summary>
        /// <param name="bufferCollector"></param>
        /// <param name="receiveResult"></param>
        public void onMessage(List<byte[]> bufferCollector, WebSocketReceiveResult receiveResult)
        {
            string message = "";
            //convert to string
            for (int i = 0; i < bufferCollector.Count - 1; i++)
            {
                message += Encoding.UTF8.GetString(bufferCollector[i]);
            }
            message += Encoding.UTF8.GetString(bufferCollector[bufferCollector.Count - 1], 0, receiveResult.Count);

            JObject messageObj = JObject.Parse(message);

            string messageType = ((string)messageObj["info"]).ToLower();
            if (messageHandlers.ContainsKey(messageType))
            {
                messageHandlers[messageType](messageObj, message);
            }
            else
            {
                Logger.Log("event", logLevel.WARN, "received an unknown type of message from client");
            }


        }
        /// <summary>
        /// on close event activated when the connection is closed
        /// </summary>
        public void onClose()
        {
            bool ans = user.unSubscribeAsObserver(this);
            if (!ans)
            {
                Logger.Log("file", logLevel.ERROR, "couldn't unsubscribe observer");
            }
        }



        public void update(List<Notification> notifications)
        {
            if (notifications != null)
            {
                foreach (Notification currNotification in notifications)
                {
                    string msgToSend;
                    switch (currNotification.notificationType)
                    {
                        case Notification.NotificationType.NORMAL:
                            var notificationObj = new { type = "notification", info = "message", data = currNotification.msg, requestId = -1 };
                            msgToSend = JsonHandler.SerializeObject(notificationObj);
                            break;

                        case Notification.NotificationType.CREATE_OWNER:
                            var dataObj = new { message = currNotification.msg, requestId = currNotification.createOwnerReqeustId };
                            var notificationObj2 = new { type = "notification", info = "addManagerConfirmation", data = dataObj, requsetId = -1 };
                            msgToSend = JsonHandler.SerializeObject(notificationObj2);
                            break;
                        default:
                            continue; 
                    }
                    sendMyselfAMessage(msgToSend);

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
            int requestId = (int)msgObj["id"];
            string ans = user.login(userName, password);
            if (ans == LoginProxy.successMsg)
            {
                responseObj = JsonResponse.generateActionSucces(requestId);
                user.subscribeAsObserver(this);
            }
            else
            {
                responseObj = JsonResponse.generateActionError(requestId, ans);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(responseObj));
        }

        private void getShoppingBasketHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            try
            {
                string jsonShoppingBasket = user.GetShoppingBasket();
                response = JsonResponse.generateDataSuccess(requestId, jsonShoppingBasket);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void signOutHandler(JObject msgObj, string message)
        {
            //remove observer
            user.unSubscribeAsObserver(this);
            //logout
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            try
            {
                bool logoutAns = user.logout();
                if (!logoutAns)
                {
                    response = JsonResponse.generateActionError(requestId, "can't logout, due to unknow error. please contact support");
                }
                response = JsonResponse.generateActionSucces(requestId);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void registerHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            string userName = (string)msgObj["data"]["name"];
            string password = (string)msgObj["data"]["password"];
            string birthDateString = (string)msgObj["data"]["birthdate"];
            string country = (string)msgObj["data"]["country"];

            DateTime birthDate = DateTime.MaxValue;
            if (birthDateString != null)
            {
                birthDate = DateTime.ParseExact(birthDateString, "dd-mm-yyyy", null);
            }
            try
            {
                response = user.Register(userName, password, birthDate, country) ?
                    JsonResponse.generateActionSucces(requestId) : JsonResponse.generateActionError(requestId, "can't register due to an unknown reason");
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void addStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            string storeName = (string)msgObj["data"];
            try
            {
                int ans = user.AddStore(storeName);
                response = JsonResponse.generateActionSucces(requestId, ans.ToString());
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"];
            try
            {
                string jsonStore = user.GetStore(storeId);
                response = JsonResponse.generateDataSuccess(requestId, jsonStore);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getProductHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int productId = (int)msgObj["data"]["productId"];
            try
            {
                string jsonProduct = user.GetProductInfo(productId);
                response = JsonResponse.generateDataSuccess(requestId, jsonProduct);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void addProductToStore(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"]["storeId"];
            string productName = (string)msgObj["data"]["name"];
            string description = (string)msgObj["data"]["description"];
            double price = (double)msgObj["data"]["price"];
            string category = (string)msgObj["data"]["category"];
            try
            {
                int ans = user.AddProductToStore(storeId, productName, description, price, category);
                response = JsonResponse.generateActionSucces(requestId, ans.ToString());
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }
        private void addProductToStock(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"]["storeId"];
            int productId = (int)msgObj["data"]["productId"];
            int amount = (int)msgObj["data"]["amount"];

            try
            {
                bool ans = user.AddProductToStock(storeId, productId, amount);
                if (ans)
                {
                    response = JsonResponse.generateActionSucces(requestId);
                }
                else
                {
                    response = JsonResponse.generateActionError(requestId, "can't add product to stock");
                }
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getAllStoresHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            try
            {
                string allStoreJson = user.GetAllStores();
                response = JsonResponse.generateDataSuccess(requestId, allStoreJson);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }


            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void addProductToBaksetHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"]["storeId"];
            int productId = (int)msgObj["data"]["productId"];
            int amount = (int)msgObj["data"]["amount"];
            try
            {
                bool addAns = user.AddProductToBasket(storeId, productId, amount);
                if (addAns)
                {
                    response = JsonResponse.generateActionSucces(requestId);
                }
                else
                {
                    response = JsonResponse.generateActionError(requestId, "unknow error occured while adding product to basket");
                }

            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }


            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void removeProductFromStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"]["storeId"];
            int productId = (int)msgObj["data"]["productId"];

            try
            {
                response = user.RemoveProductFromStore(storeId, productId) ?
                    JsonResponse.generateActionSucces(requestId) : JsonResponse.generateActionError(requestId, "failed to remove product");
            }catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void changeProductInfoHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"]["storeId"];
            int productId = (int)msgObj["data"]["productId"];
            string name = (string)msgObj["data"]["name"];
            string desc = (string)msgObj["data"]["desc"];
            string category = (string)msgObj["data"]["category"];
            ///will probalby crush
            double price = (double)msgObj["data"]["price"];
            int amount = (int)msgObj["data"]["amount"];

            try
            {
                response = user.ChangeProductInfo(storeId, productId, name, desc, price, category, amount) ?
                    JsonResponse.generateActionSucces(requestId) : JsonResponse.generateActionError(requestId, "failed to change product info");
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }


        private void closeStoreHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"];

            try
            {
                response = user.closeStore(storeId) ?
                    JsonResponse.generateActionSucces(requestId) : JsonResponse.generateActionError(requestId, "failed to close store");
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }


        private void searchProductsHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            string name = (string)msgObj["data"]["name"];
            string category = (string)msgObj["data"]["category"];
            string keyword = (string)msgObj["data"]["keyword"];

            string stringStartPrice = (string)msgObj["data"]["startPrice"];
            string stringEndPrice = (string)msgObj["data"]["endPrice"];
            string stringProductRank = (string)msgObj["data"]["productRank"];
            string stringStoreRank = (string)msgObj["data"]["storeRank"];


            double startPrice = (stringStartPrice == "") ? -1 : Convert.ToDouble(stringStartPrice);
            double endPrice = (stringEndPrice == "") ? -1 : Convert.ToDouble(stringEndPrice);
            int productRank = (stringProductRank == "") ? -1 : Convert.ToInt32(stringProductRank);
            int storeRank = (stringStoreRank == "") ? -1 : Convert.ToInt32(stringStoreRank);

            try
            {
                String data_ans = JsonHandler.SerializeObject(user.SearchProducts(name, category, keyword, startPrice, endPrice, productRank, storeRank));
                response = JsonResponse.generateDataSuccess(requestId, data_ans);
            }catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getAllMembersHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];


            try
            {    
                String data_ans = JsonHandler.SerializeObject(user.GetAllMembers());
                response = JsonResponse.generateDataSuccess(requestId, data_ans);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            
            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void removeUserHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            string userName = (string)msgObj["data"];

            try
            {
                response = user.RemoveUser(userName) ?
                    JsonResponse.generateActionSucces(requestId) : JsonResponse.generateActionError(requestId,"can't remove user");
            }
            catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void getAllManagersHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            int storeId = (int)msgObj["data"];

            try
            {

                String data_ans = JsonHandler.SerializeObject(user.GetAllManagers(storeId));
                response = JsonResponse.generateDataSuccess(requestId, data_ans);
            }
            catch (Exception e)
            {
                response = JsonResponse.generateDataFailure(requestId, e.Message);
            }
            

            sendMyselfAMessage(JsonHandler.SerializeObject(response));
        }

        private void approveOwnershipResponseHandler(JObject msgObj, string message)
        {
            int ownerRequestId = (int)msgObj["requestId"];
            try
            {
                user.ApproveOwnershipRequest(ownerRequestId);
            }
            catch (Exception e)
            {
                Logger.Log("error", logLevel.ERROR, "from approve response " + e.Message);
            }
            
        }

        private void disapproveOwnershipResponseHandler(JObject msgObj, string message)
        {
            int ownerRequestId = (int)msgObj["requestId"];
            try
            {
                user.DisApproveOwnershipRequest(ownerRequestId);
            }
            catch (Exception e)
            {
                Logger.Log("error", logLevel.ERROR, "from disapprove response " + e.Message);
            }
        }


        private void buyShoppingBasketHandler(JObject msgObj, string message)
        {
            JsonResponse response;
            int requestId = (int)msgObj["id"];
            //{int cardNumber, int month,int year, string holder, int ccv, int id, string name, string address, string city, string country, string zip}
            int cardNumber = (int)msgObj["data"]["cardNumber"];
            int month = (int)msgObj["data"]["month"];
            int year = (int)msgObj["data"]["year"];
            string holder = (string)msgObj["data"]["holder"];
            int ccv = (int)msgObj["data"]["cvv"];
            int id = (int)msgObj["data"]["id"];
            string name = (string)msgObj["data"]["name"];
            string address = (string)msgObj["data"]["address"];
            string city = (string)msgObj["data"]["city"];
            string country = (string)msgObj["data"]["country"];
            string zip = (string)msgObj["data"]["zip"];

            try
            {
                user.BuyShoppingBasket(cardNumber, month, year, holder, ccv, id, name, address, city, country, zip);
                response = JsonResponse.generateActionSucces(requestId);
            }catch (Exception e)
            {
                response = JsonResponse.generateActionError(requestId, e.Message);
            }

            sendMyselfAMessage(JsonHandler.SerializeObject(response));

        }

    /*
            private void --(JObject msgObj, string message)
            {
                JsonResponse response;
                int requestId = (int)msgObj["id"];

                sendMyselfAMessage(JsonHandler.SerializeObject(response));

            }
     */



    #endregion

}
}
