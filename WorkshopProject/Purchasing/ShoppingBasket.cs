using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    public class ShoppingBasket
    {
        Dictionary<Store,ShoppingCart> carts;

        public ShoppingBasket()
        {
            carts = new Dictionary<Store, ShoppingCart>();
        }
        
        public int addProduct(Store store,Product product,int amount)
        {
            return ((ShoppingCart)carts[store]).setProductAmount(product, amount);
        }

        public Dictionary<Store, ShoppingCart> getCarts()
        {
            return carts;
        }

    }



}
