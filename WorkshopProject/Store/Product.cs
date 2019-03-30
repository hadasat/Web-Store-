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

        

        public Product(int id, string name , int price, Categories category,int rank)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.category = category;
            this.rank = rank;
        }

        /*List<Product> search(string name,string category,int priceRange,int ranking, int storeRanking)
        {
            return new List<Product>();
        }*/
    }
}
