using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopProject.Communication;

namespace WorkshopProject.UsersN
{
    public interface IObserverSubject
    {
        bool subscribe(IObserver observer);
        bool unsbscribe(IObserver observer);
        void notifyAllObservers();
    }
}
