using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGObserver
{
    public interface IPacmanObserver : IObserver
    {
        void ObserverUpdate(PacManState p);  //State from MGPacManComponents.Pac.PacMan
    }
}
