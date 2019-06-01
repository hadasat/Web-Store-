using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shopping;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Policies
{
    public class ProductAmountPrice : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual Product product { get; set; }
        public virtual int amount { get; set; }
        public virtual double price { get; set; }

        public ProductAmountPrice(Product product, int amount, double price)
        {
            this.product = product;
            this.amount = amount;
            this.price = price;
        }

        public ProductAmountPrice(){ }
        //public static Product FromJson(string json)
        //{
        //    return JsonConvert.DeserializeObject<Product>(json);
        //}

        public override int GetKey()
        {
            return id;
        }

        public static List<ProductAmountPrice> translateCart(ShoppingCart cart)
        {
            List<ProductAmountPrice> output = new List<ProductAmountPrice>();
            List<ProductAmount> products = cart.getProducts();
            foreach(ProductAmount p in products)
            {
                ProductAmountPrice newProduct = new ProductAmountPrice(p.product, p.amount, p.product.price);
                output.Add(newProduct);
            }
            return output;
        }

        public static int sumProduct(List<ProductAmountPrice> products)
        {
            int sum = 0;
            foreach (ProductAmountPrice p in products)
                sum += p.amount;
            return sum;
        }

        public override bool Equals(object obj)
        {
            ProductAmountPrice p;
            if (obj is ProductAmountPrice)
            {
                p = (ProductAmountPrice)obj;
                if (p.product.id == product.id)
                    return true;
            }
            return false;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ProductAmountPrice)
            {
                ProductAmountPrice _other = ((ProductAmountPrice)other);
                product = _other.product;
            }
        }

        public override void LoadMe()
        {
            product.LoadMe();   
        }
    }
}
