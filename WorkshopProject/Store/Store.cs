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
        int id;
        public string name;
        public int rank;
        public Boolean isActive;

        private Dictionary<int, Product> Stock;
        private PurchasePolicy purchase_policy;


        public Store(string name, int rank,Boolean isActive)
        {
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            Stock = new Dictionary<int, Product>();

        }

        public Dictionary<int, Product> GetStock()
        {
            return Stock;
        }



        private Boolean addRemoveProductsPermission(Member member)
        {
           Roles roles = member.getStoreManagerRoles(this);
            return roles != null && roles.AddRemoveProducts;
        }

        private Boolean addRemoveDiscountPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this);
            return roles != null && roles.AddRemoveDiscountPolicy;
        }

        private Boolean addRemovePurchasingPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this);
            return roles != null && roles.AddRemovePurchasing;
        }

        private Boolean addRemoveStoreManagerPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this);
            return roles != null && roles.AddRemoveStoreManger;
        }

        Boolean addProduct(User user,Product p,int amountToAdd)
        {
            //Verify Premission
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user)))   //Verify Premission
                return false;

            if (Stock.ContainsKey(p.id))           
                addTostock(Stock[p.id],amountToAdd);     
            else        
                Stock.Add(p.id, new Product(p,amountToAdd));
            
            return true;
            
        }

        

        Boolean removeProduct(User user, Product product)
        {
            //Verify Premission
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user)))   //Verify Premission
                return false;

            Stock.Remove(product.id);
            return true;
            
        }

        Boolean addDiscount(User user, DiscountPolicy discount)
        {
            if (user is Member && addRemoveDiscountPermission((Member)user))    //Verify Premission
            {
                return true;
            }
            return false;
        }

        Boolean removeDiscount(User user, DiscountPolicy discount)
        {
            if (user is Member && addRemoveDiscountPermission((Member)user))    //Verify Premission
            {
                return true;
            }
            return false;
        }

        Boolean addPurchasingPolicy(User user,PurchasePolicy pPolicy)
        {
            if (user is Member && addRemovePurchasingPermission((Member)user))  //Verify Premission
            {
                purchase_policy = pPolicy;
                return true;
            }
            return false;
        }

        Boolean removePurchasingPolicy(User user,PurchasePolicy pPolicy)
        {
            if (user is Member && addRemovePurchasingPermission((Member)user)) //Verify Premission
            {
                return true;
            }
            return false;
        }



        /* Boolean addStoreManager(User user, User storeManager)
         {
             if (user is Member && addRemoveStoreManagerPermission((Member)user)) //Verify Premission
             {
                 return true;
             }
             return false;
         }*/

        Boolean buyProduct(User user, Product p , int amountToBuy)
        {
            
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user))) //Verify Premission
                return false;

            if (!Stock.ContainsKey(p.id) || removeFromStock(Stock[p.id],amountToBuy) == -1)
                return false;

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

        private int addTostock(Product p, int amountToAdd)
        {
            p.amount += amountToAdd;
            return p.amount;
        }

    }

     
}
