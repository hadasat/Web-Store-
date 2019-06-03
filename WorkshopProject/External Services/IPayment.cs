using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.External_Services
{
    public interface IPayment : IDisposable
    {
        /// <summary>
        /// payment method for external services
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="holder"></param>
        /// <param name="ccv"></param>
        /// <param name="id"></param>
        /// <returns> transaction id a number between [10000,100000] , -1 in case of failure</returns>
        Task<int> payment(string cardNumber, int month, int year, string holder, int ccv, int id);

        /// <summary>
        /// cancels a payment made
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<bool> cancelPayment(int transactionId);

    }
}
