using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.Policies
{
    public abstract class IOutcome : IEntity
    {
        [Key]
        public int id { get; set; }

        public abstract List<ProductAmountPrice> Apply(List<ProductAmountPrice> products, User user);

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
        }

        public override void LoadMe()
        {

        }
    }
}
