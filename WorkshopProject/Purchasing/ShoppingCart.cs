using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;

namespace Shopping
{

    public class ShoppingCart
    {
        [Key]
        public int id { get; set; }
        //[NotMapped]
        //private Dictionary<Product, int> products; //USE ONLY GETTER
        [Include]
        public List<JsonShoppingCartValue> products { get; set; }
        public static int idCartCounter = 0;

        public ShoppingCart()
        {
            id = ++idCartCounter;
            if (products == null)
            {
                products = new List<JsonShoppingCartValue>();
            }
            //products = new Dictionary<Product, int>();
        }


        //public ShoppingCart(JsonShoppingCart cart)
        //{
        //    if (cart != null)
        //    {
        //        id = cart.id;
        //        products = new Dictionary<Product, int>();
        //        foreach (JsonShoppingCartValue p in cart.products)
        //        {
        //            products.Add(p.product, p.amount);
        //        }
        //    }
        //}
        public bool containProduct(Product product)
        {
            Predicate<JsonShoppingCartValue> p = s => ((JsonShoppingCartValue)s).product.Equals(product);
            return products.Find(p) != null;
        }

        public JsonShoppingCartValue getCartValue(int id)
        {
            Predicate<JsonShoppingCartValue> productPredicat = s => ((JsonShoppingCartValue)s).product.id == id;
            return products.Find(productPredicat);
        }

        public Product getProduct(int id)
        {
            return getCartValue(id).product;
        }

        public bool setProductAmount(Product product, int amount)
        {
            JsonShoppingCartValue wannaBeCart;
            if (amount == 0 && containProduct(product))
            {
                wannaBeCart = getCartValue(product.id);
                products.Remove(wannaBeCart);
                return true;
            }
            else if (amount > 0)
            {
                if (containProduct(product))
                {
                    JsonShoppingCartValue p = getCartValue(product.id);
                    p.amount = amount;
                }
                else
                {
                    wannaBeCart = new JsonShoppingCartValue(product, amount);
                    products.Add(wannaBeCart);
                }
                return true;
            }
            return false;
        }

        public bool addProducts(Product product, int amount)
        {
            if (amount >= 0)
            {
                if (containProduct(product))
                {
                    int currentAmount = getProductAmount(product);
                    setProductAmount(product, currentAmount + amount);
                }
                else
                    setProductAmount(product, amount);
                return true;
            }
            return false;
        }

        public int getProductAmount(Product product)
        {
            Predicate<JsonShoppingCartValue> productPredicat = s => ((JsonShoppingCartValue)s).product.Equals(product);
            JsonShoppingCartValue cartValue = products.Find(productPredicat);
            if (cartValue == null)
                return 0;
            return cartValue.amount;
        }

        public int getTotalAmount()
        {
            int total = 0;
            foreach(JsonShoppingCartValue c in products)
            {
                total += c.amount;
            }
            return total;
        }

        public List<JsonShoppingCartValue> getProducts()
        {
            return products;
        }

        /*
        private Dictionary<Product, int> productListToDictionary()
        {
            Dictionary<Product, int> ret = new Dictionary<Product, int>();
            foreach (JsonShoppingCartValue prod in productsList)
            {
                ret.Add(prod.product, prod.amount);
            }
            return ret;
        }
        */
    }

    public class JsonShoppingCartValue
    {
        [Key]
        public int id { get; set; }
        [Include]
        public Product product { get; set; }
        public int amount { get; set; }

        public JsonShoppingCartValue(Product product, int amount)
        {
            this.product = product;
            this.amount = amount;
        }
        
    }
}   
    //public class JsonShoppingCart
    //{
    //    public List<JsonShoppingCartValue> products { get; set; }
    //    public int id { get; set; }

    //    public JsonShoppingCart(ShoppingCart shopping)
    //    {
    //        products = new List<JsonShoppingCartValue>();
    //        this.id = shopping.id;
    //        copyCart(shopping);
    //    }

    //    public JsonShoppingCart(int id,List<JsonShoppingCartValue> list)
    //    {
    //        this.id = id;
    //        products = list;
            
    //    }
    //    public JsonShoppingCart()
    //    {
    //    }

    //    private void copyCart(ShoppingCart shopping)
    //    {
    //        Dictionary<Product, int> shoppingProducts = shopping.getProducts();
    //        foreach (KeyValuePair<Product,int> pair in shoppingProducts)
    //        {
    //            Product p = pair.Key;
    //            int amount = pair.Value;
    //            JsonShoppingCartValue item = new JsonShoppingCartValue(p, amount);
    //            products.Add(item);
    //        }
    //    }

    //}
