using Managment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.Log;
using WorkshopProject.Policies;

namespace WorkshopProject
{
    public class Store
    {
        private int id;
        public string name;
        public int rank;
        public Boolean isActive;

        public Dictionary<int, Product> Stock;
        public IBooleanExpression purchasePolcies;
        public IBooleanExpression discountPolcies;
        public IBooleanExpression storePolicies;

        public int Id { get => id; }

        public Store(int id, string name, int rank, Boolean isActive)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            Stock = new Dictionary<int, Product>();

        }

        public Store() { }

        public Dictionary<int, Product> GetStock()
        {
            return Stock;
        }


        public int addProduct(User user, string name, string desc, double price, string category)
        {
           
            Product pro = new Product(name, price, desc, category, 0, 0, id);
            return addProduct(user, pro);
        }

        public List<Product> searchProducts(string name, string category,
             double startPrice,  double endPrice, int productRanking, int storeRanking)
        {
            
            List<Product> matched_products = new List<Product>();
            Dictionary<int, Product> products = GetStock();
            foreach (Product item in products.Values)
            {
                if ((name == "" || name == item.name) && (category == "" || category == item.category)
                    && (endPrice == -1 || endPrice >= item.getPrice()) && (startPrice == -1 || startPrice <= item.getPrice())
                    && (storeRanking == -1 || storeRanking <= rank) && (productRanking == -1 || productRanking <= item.rank))
                {
                    //All the non-empty search filters has been matched
                    matched_products.Add(item);
                }
            }
            return matched_products;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>product. if fail returns null</returns>
        public Product getProduct(int productId)
        {

            if (!Stock.ContainsKey(productId))
                return null;
            return Stock[productId];

        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        /// <param name="amountToAdd"></param>
        /// <returns>product id if succeed. otherwise -1</returns>
        private int addProduct(User user, Product p)
        {
            //Verify Premission

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return -1;

            Stock.Add(p.getId(), p);
            Logger.Log("file", logLevel.INFO, "product " + p.getId() + " added");
            return p.getId();
        }

       

        public bool removeProductFromStore(User user, Product product)
        {
            if (!isActive)
                return false;

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;
            
            Stock.Remove(product.getId());
            Logger.Log("file", logLevel.INFO, "product " + product.getId() + " removed");
            return true;
        }

        public int getNewId()
        {
            return 
        }
        /*
        public Boolean addDiscount(User user, DiscountPolicy discount)
        {
            if (!isActive)
                return false;

            if (!user.hasAddRemoveDiscountPermission(this))   //Verify Premission
                return false;
            return true;
        }
        
        public Boolean removeDiscount(User user, DiscountPolicy discount)
        {
            if (!user.hasAddRemoveDiscountPermission(this))   //Verify Premission
                return false;
            return true;
        }

        public Boolean addPurchasingPolicy(User user, PurchasePolicy pPolicy)
        {
            if (!user.hasAddRemovePurchasingPermission(this))   //Verify Premission
                return false;

            purchase_policy = pPolicy;
            return true;

        }

        public Boolean removePurchasingPolicy(User user, PurchasePolicy pPolicy)
        {
            if (!user.hasAddRemovePurchasingPermission(this))   //Verify Premission
                return false;

            return true;
        }*/
        public delegate void callback();
        /// <summary>
        /// succseed - callback that return the products to the stock. if fail - returns null
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        /// <param name="amountToBuy"></param>
        /// <returns> succseed - callback that return the products to the stock., fail - returns null</returns>
        public callback buyProduct(Product p, int amountToBuy)
        {
            callback callback = delegate () {
                if (Stock.ContainsKey(p.getId()))
                    Stock[p.getId()].amount += amountToBuy;
            };

            if (!Stock.ContainsKey(p.getId()) || removeFromStock(Stock[p.getId()],amountToBuy) == -1)
                return null;

            return callback;
                       
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
            if(amountToAdd <= 0)
            {
                return false;
            }

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            if (!Stock.ContainsKey(product.getId()))
                throw new Exception("Product not exist");

            product.amount += amountToAdd;
            Logger.Log("file", logLevel.INFO, amountToAdd +" of product " + product.getId() + " was added");
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

        public bool changeProductInfo(User user, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!Stock.ContainsKey(productId) || !user.hasAddRemoveProductsPermission(this))
                return false;
            Product product = Stock[productId];
            product.name = name;
            product.description = desc;
            product.setPrice(price);
            product.category = category;
            product.amount = amount;
            Logger.Log("file", logLevel.INFO,   "product " + product.getId() + " info has changed");
            return true;
        }

        public Product findProduct(int productId)
        {
            Product output;
            Stock.TryGetValue(productId, out output);
            return output;
        }

        //policies

        public int AddDiscountPolicy(User user, IBooleanExpression discountPolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return -2;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(discountPolicy);
            if (newPolicyId < 0)
                return -3;
            this.discountPolcies = discountPolicy;
            return newPolicyId;
        }

        public int RemoveDiscountPolicy(User user,int policyId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return -2;
            if (discountPolcies == null)
                return -3;
            this.discountPolcies = discountPolcies.removePolicy(policyId);
            return policyId;
        }

        public int AddPurchasPolicy(User user, IBooleanExpression purchasPolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return -2;
            if (purchasePolcies == null)
                return -3;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(purchasPolicy);
            if (newPolicyId < 0)
                return -3;
            this.purchasePolcies = purchasPolicy;
            return newPolicyId;
        }

        public int RemovePurchasPolicy(User user, int policyId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return -2;
            if (purchasePolcies == null)
                return -3;
            this.discountPolcies = discountPolcies.removePolicy(policyId);
            return policyId;
        }

        public int AddStorePolicy(User user, IBooleanExpression storePolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveStorePolicies(this))     //Verify Premission
                return -2;
            if (storePolicies == null)
                return -3;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(storePolicy);
            if (newPolicyId < 0)
                return -3;
            this.storePolicies = storePolicy;
            return newPolicyId;
        }

        public int RemoveStorePolicy(User user, int policyId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveStorePolicies(this))     //Verify Premission
                return -2;
            if (storePolicies == null)
                return -3;
            this.storePolicies = storePolicies.removePolicy(policyId);
            return policyId;
        }

    }
}
