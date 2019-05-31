using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.DataAccessLayer
{
    public interface IEntity
    {
        int GetKey();

        void Copy(IEntity other);
    }
}
