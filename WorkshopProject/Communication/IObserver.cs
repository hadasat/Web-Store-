using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Communication
{
    public interface IObserver
    {
        void update(List<string> messages);
    }
}
