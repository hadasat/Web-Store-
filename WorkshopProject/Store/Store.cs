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
        private List<DiscountPolicy> discountPolicy;

        public int Id { get => id; }

        public Store(int id, string name, int rank, Boolean isActive)
        {
            this.id = id;
            this.name = name;
            this.rank = rank;
            this.isActive = isActive;
            Stock = new Dictionary<int, Product>();
            discountPolicy = new List<DiscountPolicy>();

        }

        public Dictionary<int, Product> GetStock()
        {
            return Stock;
        }


        public int addProduct(User user, string name, string desc, double price, string category)
        {
            Product pro = new Product(name, price, desc, category, 0, 0, id);
            return addProduct(user, pro);
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
            return p.getId();
        }

       

        public bool removeProductFromStore(User user, Product product)
        {
            if (!user.hasAddRemoveProductsPermission(this))   //Verify Premission
                return false;

            Stock.Remove(product.getId());
            return true;
        }

        public Boolean addDiscount(User user, DiscountPolicy discount)
        {
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

        public Product findProduct(int productId)
        {
            Product output;
            Stock.TryGetValue(productId, out output);
            return output;
        }

        public bool ChangeProductInfo(User user, int productId, string name, string desc, double price, string category, int amount)
        { 
            if (!Stock.ContainsKey(productId) || !user.hasAddRemoveProductsPermission(this))
                return false;
            Product product = Stock[productId];
            product.name = name;
            product.description = desc;
            product.setPrice(price);
            product.category = category;
            product.amount = amount;
            return true;
        }

    }


}
