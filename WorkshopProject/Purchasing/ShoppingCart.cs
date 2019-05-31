using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;

namespace Shopping
{

    public class ShoppingCart : IEntity
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        private Dictionary<Product, int> products; //USE ONLY GETTER
        [Include]
        public List<JsonShoppingCartValue> productsList { get; set; }
        public static int idCartCounter = 0;

        public ShoppingCart()
        {
            id = ++idCartCounter;
            if(productsList == null)
            {
                productsList = new List<JsonShoppingCartValue>();
            }
            //products = new Dictionary<Product, int>();
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
                getProducts().Remove(product);
                return true;
            }
            else if (amount > 0)
            {
                if (getProducts().ContainsKey(product))
                    getProducts()[product] = amount;
                else
                    getProducts().Add(product, amount);
                return true;
            }
            return false;
        }

        public bool addProducts(Product product, int amount)
        {
            if (amount >= 0)
            {
                if (getProducts().ContainsKey(product))
                    setProductAmount(product, getProducts()[product] + amount);
                else
                    setProductAmount(product, amount);
                return true;
            }
            return false;
        }

        public int getProductAmount(Product product)
        {
            foreach (KeyValuePair<Product, int> p in getProducts()) 
                if (p.Key.Equals(product))
                    return p.Value;
                    
            return 0;
        }

        public int getTotalAmount()
        {
            int total = 0;
            foreach (KeyValuePair<Product, int> c in getProducts())
            {
                total += c.Value;
            }
            return total;
        }

        public Dictionary<Product, int> getProducts()
        {
            if (products == null)
            {
                products = productListToDictionary();
            }
            return products;
        }

        private Dictionary<Product, int> productListToDictionary()
        {
            Dictionary<Product, int> ret = new Dictionary<Product, int>();
            foreach (JsonShoppingCartValue prod in productsList)
            {
                ret.Add(prod.product, prod.amount);
            }
            return ret;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ShoppingCart)
            {
                ShoppingCart _other = ((ShoppingCart)other);
                productsList = _other.productsList;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in productsList)
            {
                obj.LoadMe();
            }
        }
    }

    public class JsonShoppingCartValue : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public Product product { get; set; }
        public int amount { get; }

        public JsonShoppingCartValue(Product product,int amount)
        {
            this.product = product;
            this.amount = amount;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is JsonShoppingCartValue)
            {
                JsonShoppingCartValue _other = ((JsonShoppingCartValue)other);
                product = _other.product;
            }
        }

        public override void LoadMe()
        {
             product.LoadMe();
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
            Dictionary<Product, int> shoppingProducts = shopping.getProducts();
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

