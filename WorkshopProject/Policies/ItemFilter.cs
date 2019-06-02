using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Policies
{
    public abstract class ItemFilter : IEntity
    {
        [Key]
        public int id { get; set; }
        [Include]
        public abstract List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products);

        //bool addStuff(List<ProductAmountPrice> products);
        //bool removeStuff(List<ProductAmountPrice> products);

        public override void Copy(IEntity other)
        {
            base.Copy(other);
        }

        public override void LoadMe()
        {

        }
    }

    public class AllProductsFilter : ItemFilter
    {
        public AllProductsFilter() { }

        public override List<ProductAmountPrice> getFilteredItems(List<ProductAmountPrice> products) { return products; }
    }
   

    public class ProductListFilter : ItemFilter
    {
        public List<int> productIds { get; set; }
        public ProductListFilter() {/*forJson*/ }

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
