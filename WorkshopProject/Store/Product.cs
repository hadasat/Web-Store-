using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.DataAccessLayer;
using static WorkshopProject.Category;

namespace WorkshopProject
{
    public class Product : IEntity
    {
        public static int idGenerator = 0;
        [Key]
        public int id { get; set; } 
        public string name { get; set; }
        public double price { get; set; } 
        public string category { get; set; } 
        public int rank { get; set; } 
        public string description { get; set; }
        public int amount { get; set; }
        public int storeId { get; set; }

        public Product(string name , double price, string desc, string category,int rank,int amount , int storeId)
        {
            //this.id = idGenerator++;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
            this.storeId = storeId;
            this.description = desc;
            this.amount = amount;
        }

        public Product() { }

        public int getId()
        {
            return id;
        }   

        public double getPrice()
        {
            return price;
        }

        public void setPrice(double newPrice)
        {
            this.price = newPrice;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Product))
                return false;
            Product p = (Product)obj;
            return (id == p.id && storeId == p.storeId);
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();
            int result = id;
            result = (result * 397) ^ storeId;
            return result;
        }
    }
}
