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
        private int id; 
        public string name;
        private double price;
        public string category;
        public int rank;
        public string description;
        public int amount;
        public int storeId;

        public DiscountPolicy discount;

        public Product(string name , double price, string category,int rank,int amount , int storeId)
        {
            this.id = idGenerator++;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
            this.storeId = storeId;
            discount = new DiscountPolicy();
        }

        public int getId()
        {
            return id;
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
