using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Log;

namespace Shopping
{
    public class ShoppingBasket : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual List<ShoppingCartAndStore> cartsList { get; set; }
        public static int idBasketCounter = 0;

        public ShoppingBasket()
        {
            id = idBasketCounter++;
            cartsList = new List<ShoppingCartAndStore>();
        }

        public List<ShoppingCartAndStore> getCarts() { return cartsList; }

        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
        }
        public ShoppingCart getCart(int storeId)
        {
            Predicate<ShoppingCartAndStore> cartPredicat = s => ((ShoppingCartAndStore)s).store.id == storeId;
            return cartsList.Find(cartPredicat).cart;
        }

        public ShoppingCart getCart(Store store)
        {
            Predicate<ShoppingCartAndStore> cartPredicat = s => ((ShoppingCartAndStore)s).store.Equals(store);
            return cartsList.Find(cartPredicat).cart;

        }

        public bool containStore(Predicate<ShoppingCartAndStore> p)
        {
            return cartsList.Find(p) != null;
        }

        public bool setProductAmount(Store store, Product product, int amount)
        {
            Predicate<ShoppingCartAndStore> cartPredicat = s => ((ShoppingCartAndStore)s).store.Equals(store);
            ShoppingCartAndStore cartAndStore = cartsList.Find(cartPredicat);
            //remove product from list
            if (amount == 0)
            {
                //check if the storecart is empty now
                if (containStore(cartPredicat))
                {
                    ShoppingCart cart = cartAndStore.cart;
                    //set product amount to zero
                    cart.setProductAmount(product, amount);
                    int storeAmount = cart.getTotalAmount();
                    if (storeAmount == 0)
                        cartsList.Remove(cartAndStore);
                    WorkshopProject.Log.Logger.Log("event", logLevel.INFO, $"set product {product.getId()} amount {amount} to basket {id}");
                }
                return true;
            }
            else if (amount > 0)
            {
                if (containStore(cartPredicat))
                {
                    ShoppingCart cart = cartAndStore.cart;
                    WorkshopProject.Log.Logger.Log("event", logLevel.INFO, $"set product {product.getId()} amount {amount} to basket {id}");
                    return cart.setProductAmount(product, amount);

                }
            }
            return false;
        }

        public bool addProduct(Store store, Product product, int amount)
        {
            Predicate<ShoppingCartAndStore> cartPredicat = s => ((ShoppingCartAndStore)s).store.Equals(store);
            ShoppingCartAndStore cartAndStore = cartsList.Find(cartPredicat);
            ShoppingCart cart;
            if (!containStore(cartPredicat))
            {
                cart = new ShoppingCart();
                cartsList.Add(new ShoppingCartAndStore(store, cart));
                WorkshopProject.Log.Logger.Log("event", logLevel.INFO, $"add product {product.getId()} amount {amount} to basket {id}");
            }
            else
            {
                cart = cartsList.Find(cartPredicat).cart;
                WorkshopProject.Log.Logger.Log("event", logLevel.INFO, $"set product {product.getId()} amount {amount} to basket {id}");
            }
            return cart.addProducts(product, amount);
        }

        public int getProductAmount(Product product)
        {
            foreach (ShoppingCartAndStore c in cartsList)
            {
                ShoppingCart shopping = c.cart;
                int amount = shopping.getProductAmount(product);
                if (amount > 0)
                    return amount;
            }
            return 0;
        }

        public bool isEmpty()
        {
            return cartsList.Count == 0;
        }

        public bool cleanBasket()
        {
            cartsList.Clear();
            return true;
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
        [Include]
        public virtual Store store { get; set; }
        [Include]
        public virtual ShoppingCart cart { get; set; }

        public ShoppingCartAndStore() { }

        public ShoppingCartAndStore(Store store, ShoppingCart cart)
        {
            this.store = store;
            this.cart = cart;
        }
        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
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
}