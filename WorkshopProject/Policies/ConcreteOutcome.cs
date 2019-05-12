using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    public class FreeProduct : IOutcome
    {
        int productId;
        int amount;

        public FreeProduct(int productId, int amount)
        {
            this.productId = productId;
            this.amount = amount;
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            Product product = WorkShop.findProduct(productId).Values.First();
            ProductAmountPrice freeProduct = new ProductAmountPrice(product, amount, 0);
            products.Add(freeProduct);
            return products;
        }

    }

    public class Percentage : IOutcome
    {
        public double percentage;

        public Percentage(double percentage)
        {
            this.percentage = percentage;
        }

        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            foreach (ProductAmountPrice p in products)
                p.price = calcPrice(p.price);
            return products;
        }

        private double calcPrice(double oldPrice)
        {
            return (((100 - percentage) / 100) * oldPrice);
        }
    }

   
}
