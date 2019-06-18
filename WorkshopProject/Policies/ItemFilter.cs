using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;
using WorkshopProject.System_Service;

namespace WorkshopProject.Policies
{
    public abstract class ItemFilter : IEntity
    {
        [Key]
        public int id { get; set; }
        public abstract List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products);

        //bool addStuff(List<ProductAmountPrice> products);
        //bool removeStuff(List<ProductAmountPrice> products);

        public override void Copy(IEntity other)
        {
            base.Copy(other);
        }

        public override int GetKey()
        {
            return id;
        }

        public override void SetKey(int key)
        {
            id = key;
        }

        public override void LoadMe()
        {

        }
    }

    public class AllProductsFilter : ItemFilter
    {
        public AllProductsFilter() { }

        public override List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products) { return products; }

        public override string ToString()
        {
            return "all products in store";
        }
    }
   

    public class ProductListFilter : ItemFilter
    {
        public List<int> productIds { get; set; }
        public ProductListFilter( ) { productIds = new List<int>(); }

        public ProductListFilter(List<int> productIds) {
            this.productIds = productIds;
        }



        public override List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products) {
            List<ProductAmountPrice> ret = new List<ProductAmountPrice>();
            foreach (ProductAmountPrice pair in products)
            {
                if (productIds.Contains(pair.product.id))
                {
                    ret.Add(pair);
                }
            }
            return ret;
        }

        public override string ToString()
        {
            string productsAns = "";
            foreach (Store currStore in StoreService.GetAllStores())
            {
                foreach (int currProdId in productIds)
                {
                    Product inStore = currStore.getProduct(currProdId);
                    if (inStore != null)
                    {
                        productsAns += inStore.name + " ";
                    }
                }
            }

            return "the prodcts: " + productsAns;
        }
    }

    public class CategoryFilter : ItemFilter
    {
        public string category { get; set; }

        public CategoryFilter() {/*forJson*/ }

        public CategoryFilter(string category) {
            this.category = category.ToLower();
        }

        public override List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products)
        {
            List<ProductAmountPrice> ret = new List<ProductAmountPrice>();
            foreach (ProductAmountPrice pair in products)
            {
                if (pair.product.category.ToLower().Equals(category))
                {
                    ret.Add(pair);
                }
            }
            return ret;
        }
    }
    
}
