using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;

namespace Shopping
{
    public class ShoppingBasket : IEntity
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        private Dictionary<Store, ShoppingCart> carts; //ONLY USE GETTER
        [Include]
        public List<ShoppingCartAndStore> cartsList { get; set; }
        public static int idBasketCounter = 0;

        public ShoppingBasket()
        {
            id = idBasketCounter++;
            //carts = new Dictionary<Store, ShoppingCart>();
            cartsList = new List<ShoppingCartAndStore>();
        }

        public ShoppingBasket(JsonShoppingBasket basket)
        {
            if (basket != null)
            {
                id = basket.id;
                carts = new Dictionary<Store, ShoppingCart>();
                foreach (JsonShoppingBasketValue c in basket.shoppingCarts)
                {
                    ShoppingCart cart = new ShoppingCart(c.shoppingCart);
                    getCarts().Add(c.store, cart);
                }
            }
        }

        public bool setProductAmount(Store store, Product product, int amount)
        {
            //remove product from list
            if (amount == 0)
            {
                //check if the storecart is empty now
                if (getCarts().ContainsKey(store))
                {
                    //set product amount to zero
                    getCarts()[store].setProductAmount(product, amount);
                    int storeAmount = getCarts()[store].getTotalAmount();
                    if (storeAmount == 0)
                        getCarts().Remove(store);
                }
                return true;
            }
            else if (amount > 0)
            {
                if (getCarts().ContainsKey(store))
                    return getCarts()[store].setProductAmount(product, amount);
                /*if (!carts.ContainsKey(store))
                    carts.Add(store, new ShoppingCart());
                return carts[store].setProductAmount(product, amount);*/
            }
            return false;

        }

        public bool addProduct(Store store, Product product, int amount)
        {
            if (!getCarts().ContainsKey(store))
                getCarts().Add(store, new ShoppingCart());
            return getCarts()[store].addProducts(product, amount);
        }

        public int getProductAmount(Product product)
        {
            foreach (KeyValuePair<Store, ShoppingCart> c in getCarts())
            {
                ShoppingCart shopping = c.Value;
                int amount = shopping.getProductAmount(product);
                if (amount > 0)
                    return amount;
            }
            return 0;
        }

        public bool isEmpty()
        {
            return getCarts().Count == 0;
        }

        public bool cleanBasket()
        {
            getCarts().Clear();
            return true;
        }

        public Dictionary<Store, ShoppingCart>  getCarts()
        {
            if(carts == null)
            {
                carts = cartsListAsDictionary();
            }
            return carts;
        }

        private Dictionary<Store, ShoppingCart> cartsListAsDictionary()
        {
            Dictionary<Store, ShoppingCart> ret = new Dictionary<Store, ShoppingCart>();
            foreach (ShoppingCartAndStore cart in cartsList)
            {
                ret.Add(cart.store, cart.cart);
            }
            return ret;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ShoppingBasket)
            {
                ShoppingBasket _other = ((ShoppingBasket)other);
                cartsList = _other.cartsList;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in cartsList)
            {
                obj.LoadMe();
            }
        }

    }

    public class ShoppingCartAndStore : IEntity
    {
        [Key]
        public int id { get; set; }
        public Store store { get; set; }
        public ShoppingCart cart {get; set;}

        public ShoppingCartAndStore() { }

        public ShoppingCartAndStore(Store store , ShoppingCart cart)
        {
            this.store = store;
            this.cart = cart;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ShoppingCartAndStore)
            {
                ShoppingCartAndStore _other = ((ShoppingCartAndStore)other);
                store = _other.store;
                cart = _other.cart;
            }
        }

        public override void LoadMe()
        {
            store.LoadMe();
            cart.LoadMe();
        }
    }



    public class JsonShoppingBasketValue
    {

        public Store store { get; set; }
        public JsonShoppingCart shoppingCart { get; set; }

        public JsonShoppingBasketValue() { }

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

        public JsonShoppingBasket()
        {   
        }
        

        private void copyBasket(ShoppingBasket basket)
        {
            Dictionary<Store, ShoppingCart> shoppingCartsForCopy = basket.getCarts();
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
