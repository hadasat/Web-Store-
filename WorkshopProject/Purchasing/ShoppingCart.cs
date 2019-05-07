using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;

namespace Shopping
{

    public class ShoppingCart
    {
        public Dictionary<Product, int> products;
        public static int idCartCounter = 0;
        public int id;

        public ShoppingCart()
        {
            id = ++idCartCounter;
            products = new Dictionary<Product, int>();
        }


        public ShoppingCart(JsonShoppingCart cart)
        {
            if (cart != null)
            {
                id = cart.id;
                products = new Dictionary<Product, int>();
                foreach (JsonShoppingCartValue p in cart.products)
                {
                    products.Add(p.product, p.amount);
                }
            }
        }

        public bool setProductAmount(Product product, int amount)
        {
            if(amount == 0 && products.ContainsKey(product))
            {
                products.Remove(product);
                return true;
            }
            else if (amount > 0)
            {
                if (products.ContainsKey(product))
                    products[product] = amount;
                else
                    products.Add(product, amount);
                return true;
            }
            return false;
        }

        public bool addProducts(Product product, int amount)
        {
            if (amount >= 0)
            {
                if (products.ContainsKey(product))
                    setProductAmount(product, products[product] + amount);
                else
                    setProductAmount(product, amount);
                return true;
            }
            return false;
        }

        public int getProductAmount(Product product)
        {
            foreach (KeyValuePair<Product, int> p in products) 
                if (p.Key.Equals(product))
                    return p.Value;
                    
            return 0;
        }

        public int getTotalAmount()
        {
            int total = 0;
            foreach (KeyValuePair<Product, int> c in products)
            {
                total += c.Value;
            }
            return total;
        }

        public Dictionary<Product, int> getProducts()
        {
            return products;
        }

    }

    public class JsonShoppingCartValue
    {
        public Product product { get; }
        public int amount { get; }

        public JsonShoppingCartValue(Product product,int amount)
        {
            this.product = product;
            this.amount = amount;
        }
    }

    public class JsonShoppingCart
    {
        public List<JsonShoppingCartValue> products { get; set; }
        public int id { get; set; }

        public JsonShoppingCart(ShoppingCart shopping)
        {
            products = new List<JsonShoppingCartValue>();
            this.id = shopping.id;
            copyCart(shopping);
        }

        public JsonShoppingCart(int id,List<JsonShoppingCartValue> list)
        {
            this.id = id;
            products = list;
            
        }
        public JsonShoppingCart()
        {
        }

        private void copyCart(ShoppingCart shopping)
        {
            Dictionary<Product, int> shoppingProducts = shopping.products;
            foreach (KeyValuePair<Product,int> pair in shoppingProducts)
            {
                Product p = pair.Key;
                int amount = pair.Value;
                JsonShoppingCartValue item = new JsonShoppingCartValue(p, amount);
                products.Add(item);
            }
        }

    }
}
/*public ShoppingCart(ShoppingCart s)
{
    this.products = new Dictionary<Product, int>();
    Dictionary<Product, int> products = s.getProducts();
    foreach (KeyValuePair<Product, int> c in products)
    {
        Product product = c.Key;
        int amount = c.Value;
        this.products[product] = amount;
    }
}*/

/*public bool addProduct(Product product)
   {
       if (products.ContainsKey(product))
           products[product]++;
       products[product] = 1;
       return true;
   }*/

