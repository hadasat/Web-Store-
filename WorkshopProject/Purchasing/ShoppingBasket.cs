using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    class ShoppingBasket
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

        public int addProduct(Store store, Product product)
        {
            int curr_amount = carts[store].getProductAmount(product);
            return carts[store].setProductAmount(product, curr_amount+1);
        }


        public Dictionary<Store, ShoppingCart> getCarts()
        {
            return carts;
        }

    }
}
