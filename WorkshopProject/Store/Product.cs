using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WorkshopProject.Category;

namespace WorkshopProject
{
    public class Product
    {
        public static int idGenerator = 0;
        public int id { get; } 
        public string name;
        private double price;
        public string category;
        public int rank;
        public string description;
        public int amount;
        public int storeId;

        public DiscountPolicy discount;

        public Product(string name , double price, string desc, string category,int rank,int amount , int storeId)
        {
            this.id = idGenerator++;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
            this.storeId = storeId;
            discount = new DiscountPolicy();
            this.description = desc;
            this.amount = amount;
        }

        public int getId()
        {
            return id;
        }

       

        public double getPrice()
        {
            return price - discount.amount;
        }

        public void setPrice(double newPrice)
        {
            this.price = newPrice;
        }




    }
}
