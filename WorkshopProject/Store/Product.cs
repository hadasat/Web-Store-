using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;

namespace WorkshopProject
{
    class Product
    {
        public int id;
        public string name;
        public int price;
        public Categories category;
        public int rank;
        public string description;
        public int amount;

        public Product(int id, string name , int price, Categories category,int rank,int amount)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
        }

        public Product(Product p , int amount)
        {
            id = p.id;
            name = p.name;
            price = p.price;
            category = p.category;
            rank = p.rank;
            this.amount = amount;
        }


        /// <summary>
        /// remove from stock numberToRemove products
        /// </summary>
        /// <param name="numberToRemove"></param>
        /// <returns>new amount id succeed ,otherwise -1</returns>
        public int removeFromStock(int numberToRemove)
        {
            if (amount >= numberToRemove)
            {
                amount -= numberToRemove;
                return amount;
            }
            return -1;
        }


        public int addTostock(int amountToAdd)
        {
            amount += amountToAdd;
            return amount;
        }
    }
}
