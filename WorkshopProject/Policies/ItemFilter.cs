using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;

namespace WorkshopProject.Policies
{
    public interface ItemFilter
    {
        List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products);

        //bool addStuff(List<ProductAmountPrice> products);
        //bool removeStuff(List<ProductAmountPrice> products);
    }

    public class AllProductsFilter : ItemFilter
    {
        public AllProductsFilter() { }

        public List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products) { return products; }
    }
   

    public class ProductListFilter : ItemFilter
    {
        public List<int> productIds;
        public ProductListFilter() {/*forJson*/ }

        public ProductListFilter(List<int> productIds) {
            this.productIds = productIds;
        }

        public List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products) {
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
    }

    public class CategoryFilter : ItemFilter
    {
        public string category;

        public CategoryFilter() {/*forJson*/ }

        public CategoryFilter(string category) {
            this.category = category.ToLower();
        }

        public List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products)
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
