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
            List<ProductAmountPrice> outcome = new List<ProductAmountPrice>();
            foreach (ProductAmountPrice pair in products)
            {
                if (productId == pair.product.id)
                {
                    int newPrice = 0;
                    int oldAmount = pair.amount;

                    if (oldAmount > this.amount)
                    {
                        //changing some existing products price to zero and the rest stays with the original price 
                        int restAmount = oldAmount - amount;
                        ProductAmountPrice freeProduct = new ProductAmountPrice(pair.product, amount, newPrice);
                        ProductAmountPrice restProduct = new ProductAmountPrice(pair.product, restAmount, pair.price);
                        outcome.Add(freeProduct);
                        outcome.Add(restProduct);
                    }
                    else
                    {
                        //changing all existing products price to zero
                        ProductAmountPrice freeProduct = new ProductAmountPrice(pair.product, oldAmount, newPrice);
                        outcome.Add(freeProduct);
                    }

                }
                //do not change
                else
                    outcome.Add(pair);
            }
            return outcome;
        }

    }


    public class Percentage : IOutcome
    {
        public int productId;
        public int amount;
        public double percentage;

        public Percentage(int productId, int amount, double percentage)
        {
            this.productId = productId;
            this.amount = amount;
            this.percentage = percentage;
        }
    
        public List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user)
        {
            {
                List<ProductAmountPrice> outcome = new List<ProductAmountPrice>();
                foreach (ProductAmountPrice pair in products)
                {
                    if (productId == pair.product.id)
                    {
                        double newPrice = calcPrice(pair.price);
                        int oldAmount = pair.amount;

                        if (oldAmount > this.amount)
                        {
                            //changing some existing products price to zero and the rest stays with the original price 
                            int restAmount = oldAmount - amount;
                            ProductAmountPrice freeProduct = new ProductAmountPrice(pair.product, amount, newPrice);
                            ProductAmountPrice restProduct = new ProductAmountPrice(pair.product, restAmount, pair.price);
                            outcome.Add(freeProduct);
                            outcome.Add(restProduct);
                        }
                        else
                        {
                            //changing all existing products price to zero
                            ProductAmountPrice freeProduct = new ProductAmountPrice(pair.product, oldAmount, newPrice);
                            outcome.Add(freeProduct);
                        }

                    }
                    //do not change
                    else
                        outcome.Add(pair);
                }
                return outcome;
            }
        }

        private double calcPrice(double oldPrice)
        {
            return ((percentage/100) * oldPrice);
        }
    }
}
