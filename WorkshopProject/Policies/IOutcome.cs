using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace WorkshopProject.Policies
{
    public abstract class IOutcome
    {
        [Key]
        public int id { get; set; }
        [Include]
        public abstract List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user);
    }
}
