using System.Collections.Generic;
using WorkshopProject;

namespace Shopping
{
    public class ShoppingBasket
    {
        public Dictionary<Store, ShoppingCart> carts { get; }
        public static int idBasketCounter = 0;
        public int id {get;}

        public ShoppingBasket()
        {
            id = idBasketCounter++;
            carts = new Dictionary<Store, ShoppingCart>();
        }


        public bool setProductAmount(Store store, Product product, int amount)
        {
            //remove product from list
            if (amount == 0)
            {
                //check if the storecart is empty now
                if (carts.ContainsKey(store))
                {
                    //set product amount to zero
                    carts[store].setProductAmount(product, amount);
                    int storeAmount = carts[store].getTotalAmount();
                    if (storeAmount == 0)
                        carts.Remove(store);
                }
                return true;
            }
            else if (amount > 0)
            {
                if (!carts.ContainsKey(store))
                    carts.Add(store, new ShoppingCart());
                return carts[store].addProducts(product, amount);
            }
            return false;

        }

        public bool addProduct(Store store, Product product, int amount)
        {
            if (!carts.ContainsKey(store))
                carts.Add(store, new ShoppingCart());
            return carts[store].addProducts(product, amount);
        }

        public int getProductAmount(Product product)
        {
            foreach (KeyValuePair<Store, ShoppingCart> c in carts)
            {
                ShoppingCart shopping = c.Value;
                int amount = shopping.getProductAmount(product);
                if (amount > 0)
                    return amount;
            }
            return 0;
        }

    }

    public class JsonShoppingBasketValue
    {
        Store store { get; set; }
        JsonShoppingCart shoppingCart { get; set; }

        public JsonShoppingBasketValue(Store store, JsonShoppingCart shoppingCart)
        {
            this.store = store;
            this.shoppingCart = shoppingCart;
        }
    }

    public class JsonShoppingBasket
    {
        public List<JsonShoppingBasketValue> shoppingCarts { get; set; }
        public int id { get; }

        public JsonShoppingBasket(ShoppingBasket basket)
        {
            shoppingCarts = new List<JsonShoppingBasketValue>();
            id = basket.id;
            copyBasket(basket);
        }

        private void copyBasket(ShoppingBasket basket)
        {
            Dictionary<Store, ShoppingCart> shoppingCartsForCopy = basket.carts;
            foreach (KeyValuePair<Store, ShoppingCart> pair in shoppingCartsForCopy)
            {
                Store store = pair.Key;
                JsonShoppingCart shoppingCart =new JsonShoppingCart(pair.Value);
                JsonShoppingBasketValue item = new JsonShoppingBasketValue(store, shoppingCart);
                shoppingCarts.Add(item);
            }
        }



    }
}



/*public ShoppingBasket(ShoppingBasket s)
{
    this.carts = new Dictionary<Store, ShoppingCart>();
    Dictionary<Store, ShoppingCart> carts = s.carts;
    foreach (KeyValuePair<Store, ShoppingCart> c in carts)
    {
        Store store = c.Key;
        ShoppingCart shopping = new ShoppingCart(c.Value);
        this.carts[store] = shopping;
    }
}*/
