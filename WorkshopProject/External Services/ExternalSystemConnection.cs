using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Log;

namespace WorkshopProject.External_Services
{
    public class ExternalSystemConnection : ISupply, IPayment,IDisposable
    {
        HttpClient client;

        public ExternalSystemConnection()
        {
            client = new HttpClient();
        }

        public async Task<bool> cancelSupply(int transactionId)
        {
            //create request info
            Dictionary<string, string> requestInfo = new Dictionary<string, string> {
                {"action_type","cancel_supply" },
                {"transaction_id",Convert.ToString(transactionId) },
            };

            int ans = await request(requestInfo);
            if (ans == -1)
                return false;
            return true;
        }

        public async Task<int> payment(string cardNumber, int month, int year, string holder, int ccv, int id)
        {
            //create request info
            Dictionary<string, string> requestInfo = new Dictionary<string, string> {
                {"action_type","pay" },
                {"card_number",cardNumber },
                {"month",Convert.ToString(month) },
                {"year",Convert.ToString(year) },
                {"holder",holder },
                {"ccv",Convert.ToString(ccv) },
                {"id",Convert.ToString(id) }
            };

            return await request(requestInfo);
        }

        public async Task<bool> cancelPayment(int transactionId)
        {
            //create request info
            Dictionary<string, string> requestInfo = new Dictionary<string, string> {
                {"action_type","cancel_pay" },
                {"transaction_id",Convert.ToString(transactionId) },
            };

            int ans = await request(requestInfo);
            if (ans == -1)
                return false;
            return true;
        }

        public async Task<int> supply(string name, string address, string city, string country, string zip)
        {
            //create request info
            Dictionary<string, string> requestInfo = new Dictionary<string, string> {
                {"action_type","supply" },
                {"name",name },
                {"address",address },
                {"city",city },
                {"country",country },
                {"zip",zip }
            };

            return await request(requestInfo);
        }

        /// <summary>
        /// creates handshake with external system
        /// throws excetion if can't create connection
        /// </summary>
        private async Task handshake()
        {
            Dictionary<string, string> handshakedDictionary = new Dictionary<string, string> {
                {"action_type","handshake" }
            };

            var handSahekecontent = new FormUrlEncodedContent(handshakedDictionary);
            HttpResponseMessage handShakeResponse = await client.PostAsync("https://c-bgu-wsep.herokuapp.com", handSahekecontent);
            string handshakeResposeString = await handShakeResponse.Content.ReadAsStringAsync();

            if (handshakeResposeString != "OK")
            {
                Logger.Log("event", logLevel.ERROR, "Can't handshake with external");
                throw new Exception("can't connect to external services, please try again in few minutes");
            }
        }

        private async Task<int> request(Dictionary<string, string> requestInfo)
        {
            await handshake();
            var content = new FormUrlEncodedContent(requestInfo);
            HttpResponseMessage response = await client.PostAsync("https://cs-bgu-wsep.herokuapp.com", content);
            string responseString = await response.Content.ReadAsStringAsync();
            try
            {
                int res = Convert.ToInt32(responseString);
                return res;
            }
            catch (Exception e)
            {
                client.Dispose();
                string logMsg = string.Format("can't convert from string to int the server respones {0}\n request type:{1}", responseString, requestInfo["action_type"]);
                Logger.Log("error", logLevel.ERROR, logMsg);
                return -1;
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
