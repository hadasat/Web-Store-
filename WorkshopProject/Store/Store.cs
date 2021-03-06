﻿using Managment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.Log;
using WorkshopProject.Policies;

namespace WorkshopProject
{
    public class Store : IEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public int rank { get; set; }
        public Boolean isActive { get; set; }
        //[NotMapped]
       // private Dictionary<int, Product> Stock; //USE ONLY GETTER FOR THIS FIELD
        [Include]
        public virtual List<Stock> Stocks { get; set; } //added for DB. Through "getStock" translates it to dictionary for backwards compatibility
        [Include]
        public virtual List<IBooleanExpression> purchasePolicies { get; set; }
        [Include]
        public virtual List<Discount> discountPolicies { get; set; }
        [Include]
        public virtual List<IBooleanExpression> storePolicies { get; set; }

        public int storeBankNum;
        public int storeAccountNum;
        public string storeAddress;


        public Store()
        {
            Stocks = new List<Stock>();
            this.purchasePolicies = new List<IBooleanExpression>();
            this.storePolicies = new List<IBooleanExpression>();
            this.discountPolicies = new List<Discount>();
            
        }

        public Store(int id, string name, int rank, Boolean isActive)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            //Stock = new Dictionary<int, Product>();
            Stocks = new List<Stock>();

            //make purchasePolicy and storePolicy
            this.purchasePolicies = new List<IBooleanExpression>();
            this.storePolicies = new List<IBooleanExpression>();
            this.discountPolicies = new List<Discount>();

        }

        public Store(int id, string name, int rank, Boolean isActive, int storeBankNum, int storeAccountNum, string storeAddress)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            //Stock = new Dictionary<int, Product>();
            Stocks = new List<Stock>();

            //make purchasePolicy and storePolicy
            this.purchasePolicies = new List<IBooleanExpression>();
            this.storePolicies = new List<IBooleanExpression>();
            this.discountPolicies = new List<Discount>();
            this.storeBankNum = storeBankNum;
            this.storeAccountNum = storeAccountNum;
            this.storeAddress = storeAddress;

        }

        public List<IBooleanExpression> getAllPolicies()
        {
            List<IBooleanExpression> allPolicies = new List<IBooleanExpression>(purchasePolicies);
            allPolicies.Concat(storePolicies);
            return allPolicies;
        }

        public List<Discount> getAllDiscounts()
        {
            return discountPolicies;
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
            if (other is Store)
            {
                Store _other = ((Store)other);
                Stocks = _other.Stocks;
                purchasePolicies = _other.purchasePolicies;
                discountPolicies = _other.discountPolicies;
                storePolicies = _other.storePolicies;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in Stocks)
            {
                obj.LoadMe();
            }
            foreach (IEntity obj in purchasePolicies)
            {
                obj.LoadMe();
            }
            foreach (IEntity obj in discountPolicies)
            {
                obj.LoadMe();
            }
            foreach (IEntity obj in storePolicies)
            {
                obj.LoadMe();
            }
        }
        public Dictionary<int, Product> GetStockAsDictionary()
        {
            Dictionary<int, Product> dicStock = new Dictionary<int, Product>();
            foreach(Stock s in Stocks)
            {
                dicStock.Add(s.id, s.product);
            }
            return dicStock;
           
        }

        public ICollection<Stock> getStock()
        {
            return Stocks;
        }

        public List<ProductAmountPrice> afterDiscount(List<ProductAmountPrice> products, User user)
        {
            foreach (Discount d in discountPolicies)
                products = d.Apply(products, user);

            return products;
        }

        public bool checkPolicies(List<ProductAmountPrice> products, User user)
        {
            foreach (IBooleanExpression b in purchasePolicies)
                if (!b.evaluate(products, user))
                    return false;

            foreach (IBooleanExpression b in storePolicies)
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
            Dictionary<int, Product> products = GetStockAsDictionary();
            foreach (Product item in products.Values)
            {
                if ((name == "" || item.name.Contains(name)) && (category == "" || item.category.Contains(category) )
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

            if (!GetStockAsDictionary().ContainsKey(productId))
                return null;
            return GetStockAsDictionary()[productId];

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

            getStock().Add(new Stock(p));
            Logger.Log("event", logLevel.INFO, "product " + p.getId() + " added");
            return p.getId();
        }



        public bool removeProductFromStore(User user, Product product)
        {
            if (!isActive)
                return false;

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;
            
            getStock().Remove(getStockFromProductId(product.getId()));
            Logger.Log("event", logLevel.INFO, "product " + product.getId() + " removed");
            return true;
        }

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
                if (GetStockAsDictionary().ContainsKey(p.getId()))
                    GetStockAsDictionary()[p.getId()].amount += amountToBuy;
                WorkShop.Update(this);
            };

            if (!GetStockAsDictionary().ContainsKey(p.getId()) || removeFromStock(GetStockAsDictionary()[p.getId()], amountToBuy) == -1)
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
            if (!amountIsLegal(p.amount - amountToBuy))
                return -1;

            p.amount -= amountToBuy;
            WorkShop.Update(this);
            return p.amount;
        }

        public bool addProductTostock(User user, Product product, int amountToAdd)
        {
            if (!amountIsLegal(amountToAdd))
            {
                throw new Exception("the amount is illegal");

            }

            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            if (!GetStockAsDictionary().ContainsKey(product.getId()))
                throw new Exception("Product not exist");

            product.amount += amountToAdd;
            Logger.Log("event", logLevel.INFO, amountToAdd + " of product " + product.getId() + " was added");
            return true;
        }

        private bool amountIsLegal(int amountToAdd)
        {
            if (amountToAdd < 0)
                return false;
            return true;
        }

        public bool checkAvailability(Product product, int amount)
        {
            if (GetStockAsDictionary().ContainsKey(product.getId()))
            {
                return GetStockAsDictionary()[product.getId()].amount >= amount;
            }
            return false;
        }

        public bool changeProductInfo(User user, int productId, string name, string desc, double price, string category, int amount)
        {
            if (!GetStockAsDictionary().ContainsKey(productId) || !user.hasAddRemoveProductsPermission(this))
                return false;
            if (!amountIsLegal(amount))
                throw new Exception("new amount is illegal");

            Stock s = getStockFromProductId(productId);
            if (s == null)
                return false;
            Product product = s.product;
            product.name = name;
            product.description = desc;
            product.setPrice(price);
            product.category = category;
            product.amount = amount;
            Logger.Log("event", logLevel.INFO, "product " + product.getId() + " info has changed");
            return true;
        }

        public Product findProduct(int productId)
        {
            Product output;
            GetStockAsDictionary().TryGetValue(productId, out output);
            return output;
        }

        //*****************POLICIES**************************8
        public Policystatus AddDiscountPolicy(User user, Discount discountPolicy)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return Policystatus.UnauthorizedUser;
            //check policy validation
            int newPolicyId = Discount.checkDiscount(discountPolicy);
            if (newPolicyId < 0)
                return Policystatus.BadPolicy;
            //check consistency
            if (!Discount.confirmListConsist(discountPolicy, this.discountPolicies))
                return Policystatus.InconsistPolicy;
            discountPolicy.id = newPolicyId;
            this.discountPolicies.Add(discountPolicy);
            return Policystatus.Success;
        }

        public Policystatus RemoveDiscountPolicy(User user, int discountId)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemoveDiscountPolicies(this))   //Verify Premission
                return Policystatus.UnauthorizedUser;
            if (discountPolicies == null)
                return Policystatus.BadPolicy;
            Discount fakeDis = new Discount(null, null);
            fakeDis.id = discountId;
            if (this.discountPolicies.Remove(fakeDis))
                return Policystatus.Success;
            return Policystatus.BadPolicy;
        }

        public Policystatus AddPurchasPolicy(User user, IBooleanExpression purchasPolicy)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return Policystatus.UnauthorizedUser;
            if (purchasePolicies == null)
                return Policystatus.BadPolicy;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(purchasPolicy);
            //check consistency
            if (!IBooleanExpression.confirmListConsist(purchasPolicy, purchasePolicies))
                return Policystatus.InconsistPolicy;
            if (newPolicyId < 0)
                return Policystatus.BadPolicy;
            purchasPolicy.id = newPolicyId;
            this.purchasePolicies.Add(purchasPolicy);
            return Policystatus.Success;
        }

        public Policystatus RemovePurchasPolicy(User user, int policyId)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return Policystatus.UnauthorizedUser;
            if (purchasePolicies == null)
                return Policystatus.BadPolicy;
            IBooleanExpression temp = null;
            foreach (IBooleanExpression b in this.purchasePolicies)
                if (b.id == policyId)
                    temp = b;
            if (this.purchasePolicies.Remove(temp))
                return Policystatus.Success;
            return Policystatus.BadPolicy;
        }

        public Policystatus AddStorePolicy(User user, IBooleanExpression newstorePolicy)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemoveStorePolicies(this))     //Verify Premission
                return Policystatus.UnauthorizedUser;
            if (newstorePolicy == null)
                return Policystatus.BadPolicy;
            //check policy validation
            int newPolicyId = IBooleanExpression.checkExpression(newstorePolicy);
            //check consistency
            if (!IBooleanExpression.confirmListConsist(newstorePolicy, storePolicies))
                return Policystatus.InconsistPolicy;
            if (newPolicyId < 0)
                return Policystatus.BadPolicy;
            newstorePolicy.id = newPolicyId;
            this.storePolicies.Add(newstorePolicy);
            return Policystatus.Success;
        }

        public Policystatus RemoveStorePolicy(User user, int policyId)
        {
            if (!isActive)
                return Policystatus.UnactiveStore;
            if (!user.hasAddRemovePurchasingPolicies(this))     //Verify Premission
                return Policystatus.UnauthorizedUser;
            if (this.storePolicies == null)
                return Policystatus.BadPolicy;
            IBooleanExpression temp = null;
            foreach (IBooleanExpression b in this.storePolicies)
                if (b.id == policyId)
                    temp = b;
            if (this.storePolicies.Remove(temp))
                return Policystatus.Success;
            return Policystatus.BadPolicy;
        }


        //private Dictionary<int, Product> getStockListAsDictionary()
        //{
        //    Dictionary<int, Product> ret = new Dictionary<int, Product>();
        //    foreach (Stock stock in this.Stocks)
        //    {
        //        ret.Add(stock.amount, stock.product);
        //    }
        //    return ret;
        //}

        public Discount getDiscountPolicy(int id)
        {
            if (id == -1)
                return this.discountPolicies.Last();
            foreach (Discount dis in discountPolicies)
            {
                if (dis.id == id)
                    return dis;
            }
            return null;
        }

        public IBooleanExpression getPolicy(int id)
        {
            //store policy
            if (id == -1)
                return this.storePolicies.Last();
            //purchase policy
            else if (id == -2)
                return this.purchasePolicies.Last();

            foreach (IBooleanExpression pol in this.storePolicies)
            {
                if (pol.id == id)
                    return pol;
            }
            foreach (IBooleanExpression pol in this.purchasePolicies)
            {
                if (pol.id == id)
                    return pol;
            }
            return null;
        }

        public List<Product> getAllProducst()
        {
            List<Product> ans = new List<Product>();
            if (Stocks == null || Stocks.Count == 0)
            {
                return ans;
            }
            foreach (Stock s in Stocks)
            {

                ans.Add(s.product);
            }

            return ans;
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

        public Stock getStockFromProductId(int pid)
        {
            foreach (Stock s in Stocks)
                if (s.id == pid)
                    return s;
            return null;
        }
    }

   


    public class Stock : IEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //public int amount { get; set; }
        [Include]
        public virtual Product product { get; set; }


        public Stock() { }

        public Stock( Product product)
        {
            //this.amount = amount;
            this.product = product;
            this.id = product.id;
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
            if (other is Stock)
            {
                Stock _other = ((Stock)other);
                product = _other.product;
            }
        }

        public override void LoadMe()
        {
            product.LoadMe();
        }
    }

}
