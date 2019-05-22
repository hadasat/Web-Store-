using Managment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public Boolean isActive { get; set; }
        [NotMapped]
        private Dictionary<int, Product> Stock; //USE ONLY GETTER FOR THIS FIELD
        public List<Stock> StockList { get; set; } //added for DB. Through "getStock" translates it to dictionary for backwards compatibility
        public List<IBooleanExpression> purchasePolicy { get; set; }
        public List<Discount> discountPolicy { get; set; }
        public List<IBooleanExpression> storePolicy { get; set; }


        public Store(int id, string name, int rank, Boolean isActive)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            //Stock = new Dictionary<int, Product>();
            StockList = new List<Stock>();

            //make purchasePolicy and storePolicy
            this.purchasePolicy = new List<IBooleanExpression>();
            this.storePolicy = new List<IBooleanExpression>();
            this.discountPolicy = new List<Discount>();

        }

        public Store() { }

        public Dictionary<int, Product> GetStock()
        {
            if (Stock == null)
            {
                Stock = getStockListAsDictionary();
            }
            return Stock;
        }

        public List<ProductAmountPrice> afterDiscount(List<ProductAmountPrice> products, User user)
        {
            foreach (Discount d in discountPolicy)
                products = d.Apply(products, user);

            return products;
        }

        public bool checkPolicies(List<ProductAmountPrice> products, User user)
        {
            foreach (IBooleanExpression b in purchasePolicy)
                if (!b.evaluate(products, user))
                    return false;

            foreach (IBooleanExpression b in storePolicy)
                if (!b.evaluate(products, user))
                    return false;
            return true;
        }

        public int addProduct(User user, string name, string desc, double price, string category)
        {
            Product pro = new Product(name, price, desc, category, 0, 0, id);
            return addProduct(user, pro);
        }

        public List<Product> searchProducts(string name, string category,
             double startPrice, double endPrice, int productRanking, int storeRanking)
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

            if (!GetStock().ContainsKey(productId))
                return null;
            return GetStock()[productId];

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

            GetStock().Add(p.getId(), p);
            Logger.Log("file", logLevel.INFO, "product " + p.getId() + " added");
            return p.getId();
        }



        public bool removeProductFromStore(User user, Product product)
        {
            if (!isActive)
                return false;

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            GetStock().Remove(product.getId());
            Logger.Log("file", logLevel.INFO, "product " + product.getId() + " removed");
            return true;
        }
        /*
        public Boolean addDiscount(User user, Discount discount)
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

            purchasePolicy = pPolicy;
            return true;

        }

        public Boolean removePurchasingPolicy(User user, PurchasePolicy pPolicy)
        {
            if (!user.hasAddRemovePurchasingPermission(this))   //Verify Premission
                return false;

            return true;
        }
        */
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
            callback callback = delegate ()
            {
                if (GetStock().ContainsKey(p.getId()))
                    GetStock()[p.getId()].amount += amountToBuy;
            };

            if (!GetStock().ContainsKey(p.getId()) || removeFromStock(GetStock()[p.getId()], amountToBuy) == -1)
                return null;

            string buyMessage = String.Format("the product {0}, was bought from the store {1}", p.name, name);
            Member.sendMessageToAllOwners(id, buyMessage);
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

        public bool addProductTostock(User user, Product product, int amountToAdd)
        {
            if (amountToAdd <= 0)
            {
                return false;
            }

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            if (!GetStock().ContainsKey(product.getId()))
                throw new Exception("Product not exist");

            product.amount += amountToAdd;
            Logger.Log("file", logLevel.INFO, amountToAdd + " of product " + product.getId() + " was added");
            return true;
        }


        public bool checkAvailability(Product product, int amount)
        {
            if (GetStock().ContainsKey(product.getId()))
            {
                return GetStock()[product.getId()].amount >= amount;
            }
            return false;
        }

        public bool changeProductInfo(User user, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!GetStock().ContainsKey(productId) || !user.hasAddRemoveProductsPermission(this))
                return false;
            Product product = GetStock()[productId];
            product.name = name;
            product.description = desc;
            product.setPrice(price);
            product.category = category;
            product.amount = amount;
            Logger.Log("file", logLevel.INFO, "product " + product.getId() + " info has changed");
            return true;
        }

        public Product findProduct(int productId)
        {
            Product output;
            GetStock().TryGetValue(productId, out output);
            return output;
        }

        //*****************POLICIES**************************8
        public int AddDiscountPolicy(User user, Discount discountPolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return -2;
            //check policy validation
            int newPolicyId = Discount.checkDiscount(discountPolicy);
            if (newPolicyId < 0)
                return -3;
            discountPolicy.id = newPolicyId;
            this.discountPolicy.Add(discountPolicy);
            return newPolicyId;
        }

        public int RemoveDiscountPolicy(User user, int discountId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return -2;
            if (discountPolicy == null)
                return -3;
            Discount fakeDis = new Discount(null, null);
            fakeDis.id = discountId;
            if(this.discountPolicy.Remove(fakeDis))
                    return discountId;
            return -1;
        }

        public int AddPurchasPolicy(User user, IBooleanExpression purchasPolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return -2;
            if (purchasePolicy == null)
                return -3;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(purchasPolicy);
            if (newPolicyId < 0)
                return -3;
            purchasPolicy.id = newPolicyId;
            this.purchasePolicy.Add(purchasPolicy);
            return newPolicyId;
        }

        public int RemovePurchasPolicy(User user, int policyId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return -2;
            if (purchasePolicy == null)
                return -3;
            IBooleanExpression temp = null;
            foreach (IBooleanExpression b in this.purchasePolicy)
                if (b.id == policyId)
                    temp = b;
            if(this.purchasePolicy.Remove(temp))
                return policyId;
            return -1;
        }

        public int AddStorePolicy(User user, IBooleanExpression storePolicy)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveStorePolicies(this))     //Verify Premission
                return -2;
            if (this.storePolicy == null)
                return -3;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(storePolicy);
            if (newPolicyId < 0)
                return -3;
            storePolicy.id = newPolicyId;
            this.storePolicy.Add(storePolicy);
            return newPolicyId;
        }

        public int RemoveStorePolicy(User user, int policyId)
        {
            if (!isActive)
                return -1;
            if (!user.hasAddRemoveStorePolicies(this))     //Verify Premission
                return -2;
            if (this.storePolicy == null)
                return -3;
            IBooleanExpression temp = null;
            foreach (IBooleanExpression b in this.storePolicy)
                if (b.id == policyId)
                    temp = b;
            if (this.storePolicy.Remove(temp))
                return policyId;
            return -1;
        }


        private Dictionary<int, Product> getStockListAsDictionary()
        {
            Dictionary<int, Product> ret = new Dictionary<int, Product>();
            foreach (Stock stock in this.StockList)
            {
                ret.Add(stock.amount, stock.product);
            }
            return ret;
        }

        //private void addToStockListAsDictionary(int amount, Product product)
        //{
        //    StockList.Add(new Stock(amount, product));
        //}

        //private Dictionary<int, Product> getStock()
        //{
        //    if(Stock == null)
        //    {
        //        Stock = getStockListAsDictionary();
        //    }
        //    return Stock;
        //}
    }


    public class Stock
    {
        [Key]
        public int id { get; set; }
        public int amount { get; set; }
        public Product product { get; set; }


        public Stock() { }

        public Stock(int amount, Product product)
        {
            this.amount = amount;
            this.product = product;
        }
    }

}
