using Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject
{
    public class Store
    {
        private int id;
        public string name;
        public int rank;
        public Boolean isActive;

        private Dictionary<int, Product> Stock;
        private PurchasePolicy purchase_policy;

        public int Id { get => id;  }

        public Store(int id,string name, int rank,Boolean isActive)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            Stock = new Dictionary<int, Product>();

        }

        public Dictionary<int, Product> GetStock()
        {
            return Stock;
        }


        Boolean addProduct (User user, string name, string desc, double price, string category)
        {
            Product pro = new Product(name, price, category, 0, 0);
            return addProduct(user, pro);
        }
    
        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        /// <param name="amountToAdd"></param>
        /// <returns></returns>
        Boolean addProduct(User user,Product p)
        {
            //Verify Premission
            
            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;
                
         
                    
            Stock.Add(p.getId(), new Product(p,0));
            return true;
            
        }        

        Boolean removeProduct(User user, Product product)
        {
            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            Stock.Remove(product.getId());
            return true;
            
        }

        Boolean addDiscount(User user, DiscountPolicy discount)
        {
            if (!user.hasAddRemoveDiscountPermission(this))   //Verify Premission
                return false;
            return true;
        }

        Boolean removeDiscount(User user, DiscountPolicy discount)
        {
            if (!user.hasAddRemoveDiscountPermission(this))   //Verify Premission
                return false;
            return true;
        }

        Boolean addPurchasingPolicy(User user,PurchasePolicy pPolicy)
        {
            if (!user.hasAddRemovePurchasingPermission(this))   //Verify Premission
                return false;

            purchase_policy = pPolicy;
            return true;

        }

        Boolean removePurchasingPolicy(User user,PurchasePolicy pPolicy)
        {
            if (!user.hasAddRemovePurchasingPermission(this))   //Verify Premission
                return false;

            return true;
        }

        /// <summary>
        /// this method called by shoppingBakset
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        /// <param name="amountToBuy"></param>
        /// <returns> succseed - callback that return the products to the stock., fail - returns null</returns>
        bool buyProduct(User user, Product p , int amountToBuy)
        {
            /*
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user))) //Verify Premission
                return false;

            if (!Stock.ContainsKey(p.id) || removeFromStock(Stock[p.id],amountToBuy) == -1)
                return false;
                */
            return true;
                       
        }

        /// <summary>
        /// Remove from stock 'amountToBuy' products
        /// </summary>
        /// <param name="numberToRemove"></param>
        /// <returns>new amount id succeed ,otherwise -1</returns>
        private int removeFromStock(Product p, int amountToBuy)
        {
            if (p.amount < amountToBuy)
                return -1;
            
            p.amount -= amountToBuy;
            return p.amount;                    
        }

        public bool addProductTostock(User user,Product product, int amountToAdd)
        {
            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            if (!Stock.ContainsKey(product.getId()))
                throw new Exception("Product not exist");

            product.amount += amountToAdd;
            return true;
        }

        public bool removeProductFromStore(User user, Product product)
        {
            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            Stock.Remove(product.getId());
            return true;
        }

        public bool checkAvailability(Product product, int amount)
        {
            if (Stock.ContainsKey(product.getId()))
            {
                return Stock[product.getId()].amount >= amount;
            }
            return false;
        }


    }


}
