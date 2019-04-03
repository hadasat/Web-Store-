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
        public int id;
        public string name;
        private int price;
        public Categories category;
        public int rank;
        public string description;
        public int amount;

        public DiscountPolicy discount;

        public Product(int id, string name , int price, Categories category,int rank,int amount)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
            discount = new DiscountPolicy();
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

        public int getPrice()
        {
            return price - discount.amount;
        }

        public void setPrice(int newPrice)
        {
            this.price = newPrice;
        }


    }
}
