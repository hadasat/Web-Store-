using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    public class Store
    {

        public string name;
        public int rank;
        public Boolean isActive;

        public Dictionary<int, Product> stock { get; set; }
        //public Dictionary<int, int> products;
        private PurchasePolicy purchase_policy;

        private Store(string name, int rank,Boolean isActive)
        {
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            stock = new Dictionary<int, Product>();

        }
        

        private Boolean addRemoveProductsPermission(Member member)
        {
           Roles roles = member.getStoreManagerRoles(this).addRemoveProducts;
            return roles != null && roles.addRemoveProducts;
        }

        private Boolean addRemoveDiscountPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this).addRemoveProducts;
            return roles != null && roles.addRemoveDiscountPolicy;
        }

        private Boolean addRemovePurchasingPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this).addRemoveProducts;
            return roles != null && roles.addRemovePurchasing;
        }

        private Boolean addRemoveStoreManagerPermission(Member member)
        {
            Roles roles = member.getStoreManagerRoles(this).addRemoveProducts;
            return roles != null && roles.addRemoveStoreManager;
        }

        Boolean addProduct(User user,Product p,int amountToAdd)
        {
            //Verify Premission
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user)))   //Verify Premission
                return false;

            if (stock.ContainsKey(p.id))           
                addTostock(stock[p.id],amountToAdd);     
            else        
                stock.Add(p.id, new Product(p,amountToAdd));
            
            return true;
            
        }

        

        Boolean removeProduct(User user, Product product)
        {
            //Verify Premission
            if (!(user is Member) || !(addRemoveProductsPermission((Member)user)))   //Verify Premission
                return false;

            stock.Remove(product.id);
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

            if (!stock.ContainsKey(p.id) || removeFromStock(stock[p.id],amountToBuy) == -1)
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
