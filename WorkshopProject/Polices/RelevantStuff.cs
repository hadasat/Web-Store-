using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;

namespace Polices
{
    public interface RelevantStuff
    {
        List<ProductAmountPrice> getStuff(List<ProductAmountPrice> products);

        bool addStuff(List<ProductAmountPrice> products);

        bool removeStuff(List<ProductAmountPrice> products);

    }

    public class ProductAmountPrice
    {
        public Product product;
        public int amount;
        public double price;

        public ProductAmountPrice(Product product, int amount, double price)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;

        }
    }

    public class AllProducts : RelevantStuff
    {
        public AllProducts() { }

        public List<ProductAmountPrice> getStuff(List<ProductAmountPrice> products) { return products; }

        public bool addStuff(List<ProductAmountPrice> products) { return true; }

        public bool removeStuff(List<ProductAmountPrice> products) { return true; }
    }
   

    public class ProductList : RelevantStuff
    {
        List<ProductAmountPrice> products;

        public ProductList(List<ProductAmountPrice> products) {this.products = products;}

        public List<ProductAmountPrice> getStuff(List<ProductAmountPrice> products) {
            return products; }

        public bool addStuff(List<ProductAmountPrice> products) { return true; }      

        public bool removeStuff(List<ProductAmountPrice> products) { return true; }
    }

    public class Category : RelevantStuff
    {
        public Category() { }

        public bool addStuff(List<ProductAmountPrice> products) { return true; }

        public List<ProductAmountPrice> getStuff(List<ProductAmountPrice> products) { return products; }

        public bool removeStuff(List<ProductAmountPrice> products) { return true; }
    }
    
}
