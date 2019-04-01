using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject
{
    class Store
    {

        public string name;
        public int rank;
        public Boolean isActive;

        public Dictionary<int, Product> products { get; set; }
        private PurchasePolicy purchase_policy;

        private Store(string name, int rank,Boolean isActive)
        {
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            products = new Dictionary<int, Product>();

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

        Boolean addProduct(User user,Product product)
        {
            //Verify Premission
            if(user is Member && addRemoveProductsPermission((Member)user)) //Verify Premission
            {
                products.Add(product.id, product);
                return true;
            }
            return false;
           
        }

        Boolean removeProduct(User user, Product product)
        {
            //Verify Premission
            if (user is Member && addRemoveProductsPermission((Member)user))   //Verify Premission
            {
                products.Remove(product.id);
                return true;
            }
            return false;
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




    }
}
