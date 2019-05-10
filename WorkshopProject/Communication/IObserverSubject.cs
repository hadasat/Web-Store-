using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopProject.Communication
{
    public interface IObserverSubject
    {
        bool subscribe(IObserver observer);
        bool unsbscribe(IObserver observer);
        void notifyAllObservers();
    }
}
