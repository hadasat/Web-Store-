using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;

namespace Shopping
{

    public class ShoppingCart : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual List<ProductAmount> products { get; set; }
        public static int idCartCounter = 0;

        public ShoppingCart()
        {
            id = ++idCartCounter;
            if (products == null)
            {
                products = new List<ProductAmount>();
            }
        }

        public override int GetKey()
        {
            return id;
        }
        public bool containProduct(Product product)
        {
            Predicate<ProductAmount> p = s => ((ProductAmount)s).product.Equals(product);
            return products.Find(p) != null;
        }

        public ProductAmount getCartValue(int id)
        {
            Predicate<ProductAmount> productPredicat = s => ((ProductAmount)s).product.id == id;
            return products.Find(productPredicat);
        }

        public Product getProduct(int id)
        {
            return getCartValue(id).product;
        }

        public bool setProductAmount(Product product, int amount)
        {
            ProductAmount wannaBeCart;
            if (amount == 0 && containProduct(product))
            {
                wannaBeCart = getCartValue(product.id);
                products.Remove(wannaBeCart);
                return true;
            }
            else if (amount > 0)
            {
                if (containProduct(product))
                {
                    ProductAmount p = getCartValue(product.id);
                    p.amount = amount;
                }
                else
                {
                    wannaBeCart = new ProductAmount(product, amount);
                    products.Add(wannaBeCart);
                }
                return true;
            }
            return false;
        }

        public bool addProducts(Product product, int amount)
        {
            if (amount >= 0)
            {
                if (containProduct(product))
                {
                    int currentAmount = getProductAmount(product);
                    setProductAmount(product, currentAmount + amount);
                }
                else
                    setProductAmount(product, amount);
                return true;
            }
            return false;
        }

        public int getProductAmount(Product product)
        {
            Predicate<ProductAmount> productPredicat = s => ((ProductAmount)s).product.Equals(product);
            ProductAmount cartValue = products.Find(productPredicat);
            if (cartValue == null)
                return 0;
            return cartValue.amount;
        }

        public int getTotalAmount()
        {
            int total = 0;
            foreach (ProductAmount c in products)
            {
                total += c.amount;
            }
            return total;
        }

        public List<ProductAmount> getProducts()
        {
            return products;
        }

        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ShoppingCart)
            {
                ShoppingCart _other = ((ShoppingCart)other);
                products = _other.products;
            }
        }

        public override void LoadMe()
        {
            foreach (IEntity obj in products)
            {
                obj.LoadMe();


            }
        }
    }

    public class ProductAmount : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public virtual Product product { get; set; }
        public int amount { get; set; }

        public ProductAmount(Product product, int amount)
        {
            this.product = product;
            this.amount = amount;
        }

        public override int GetKey()
        {
            return id;
        }
        public override void Copy(IEntity other)
        {
            base.Copy(other);
            if (other is ProductAmount)
            {
                ProductAmount _other = ((ProductAmount)other);
                product = _other.product;
            }
        }

        public override void LoadMe()
        {
            product.LoadMe();
        }
    }
}