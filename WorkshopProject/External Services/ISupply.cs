using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.External_Services
{
    public interface ISupply
    {
        /// <summary>
        /// try to send package
        /// </summary>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="zip"></param>
        /// <returns>transaction id - num between  [10000,100000], -1 on failure</returns>
        Task<int> supply(string name, string address, string city, string country, string zip);

        Task<bool> cancelSupply(int transactionId);
    }
}
